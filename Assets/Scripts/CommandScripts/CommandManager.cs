using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using COMMAND;
using CHARACTERS;
using UnityEngine.Events;

namespace COMMAND
{

    //responsible for validating and executing commands found in text files
    //this class will handle everything independantly
    //is connected to gameobject in unity scene
    public class CommandManager : MonoBehaviour
    {
        private const char SUB_COMMAND_IDENTIFIER = '.';
        public const string DATABASE_CHARACTERS_BASE = "characters";
        public const string DATABASE_CHARACTERS_SPRITE = "characters_sprite";

        public static CommandManager instance { get; private set; }

        private CommandDatabase database;
        private Dictionary<string, CommandDatabase> subDatabases = new Dictionary<string, CommandDatabase>();

        private List<CommandProcess> activeProcesses = new List<CommandProcess>();
        private CommandProcess topProcess => activeProcesses.Last();

        private void Awake()
        {
            //check if theres only one commandmanager running at a time (should only be 1)
            if (instance == null)
            {
                instance = this;
                database = new CommandDatabase();
                //assign this to current executing assembly, gets reference to current executing assembly
                Assembly assembly = Assembly.GetExecutingAssembly();
                //type array, we dont know every extension that we will have (theyre all different)
                //finds all types of our database extension
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                //execute extend method on each extension class found
                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { database });
                }
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public CoroutineWrapper Execute(string commandName, params string[] args)
        {
            if (commandName.Contains(SUB_COMMAND_IDENTIFIER))
            {
                return ExecuteSubCommand(commandName, args);
            }
            Delegate command = database.GetCommand(commandName);
            if (command == null)
            {
                return null;
            }

            return StartProcess(commandName, command, args);
        }

        private CoroutineWrapper ExecuteSubCommand(string commandName, string[] args)
        {
            string[] parts = commandName.Split(SUB_COMMAND_IDENTIFIER);
            //take all the pieces minus the last one
            string databaseName = string.Join(SUB_COMMAND_IDENTIFIER, parts.Take(parts.Length - 1));
            //last part goes in here
            string subCommandName = parts.Last();

            //check if database exists
            if (subDatabases.ContainsKey(databaseName))
            {
                Delegate command = subDatabases[databaseName].GetCommand(subCommandName);
                //if we have a command
                if(command != null)
                {
                    return StartProcess(commandName, command, args);
                }
                else
                {
                    Debug.LogError($"No command called '{subCommandName}' was found in sub database '{databaseName}'");
                    return null;
                }
            }

            string characterName = databaseName;
            //if were here, try to run this as a character command
            if (CharacterManager.instance.HasCharacter(characterName))
            {
                List<string> newArgs = new List<string>(args);
                newArgs.Insert(0, characterName);
                args = newArgs.ToArray();

                return ExecuteCharacterCommand(subCommandName, args);
            }

            Debug.LogError($"No sub database called '{databaseName}' exists! Command '{subCommandName}' could not be run.");
            return null;
        }

        private CoroutineWrapper ExecuteCharacterCommand(string commandName, params string[] args)
        {
            Delegate command = null;

            CommandDatabase db = subDatabases[DATABASE_CHARACTERS_BASE];
            if (db.HasCommand(commandName))
            {
                command = db.GetCommand(commandName);
                return StartProcess(commandName, command, args);
            }

            //if base database doesnt have command, then search through character type specific one
            //not nessesary for us since we only have a sprite character type but switch is included based on tutorial
            CharacterConfigData characterConfigData = CharacterManager.instance.GetCharacterConfig(args[0]);
            switch (characterConfigData.characterType)
            {
                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    db = subDatabases[DATABASE_CHARACTERS_SPRITE];
                    break;
            }
            command = db.GetCommand(commandName);
            if(command != null)
            {
                return StartProcess(commandName, command, args);
            }
            Debug.LogError($"Command Manager was unable to execute command '{commandName}' on character '{args[0]}'. The type might be invalid.");
            return null;
        }

        private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
        {
            System.Guid processID = System.Guid.NewGuid();
            CommandProcess cmd = new CommandProcess(processID, commandName, command, null, args, null);
            activeProcesses.Add(cmd);

            Coroutine co = StartCoroutine(RunningProcess(cmd));
            cmd.runningProcess = new CoroutineWrapper(this, co);

            return cmd.runningProcess;
        }

        public void StopCurrentProces()
        {
            if (topProcess != null)
            {
                KillProcess(topProcess);
            }
        }

        public void StopAllProcesses()
        {
            foreach (var c in activeProcesses)
            {
                if(c.runningProcess != null && !c.runningProcess.IsDone)
                {
                    c.runningProcess.Stop();
                }
                c.onTerminateAction?.Invoke();
            }
            //clear the list of processes after theyre stopped
            activeProcesses.Clear();
        }

        private IEnumerator RunningProcess(CommandProcess process)
        {
            yield return WaitingForProcessToComplete(process.command, process.args);

            KillProcess(process);
        }

        public void KillProcess(CommandProcess cmd)
        {
            activeProcesses.Remove(cmd);

            if(cmd.runningProcess != null && !cmd.runningProcess.IsDone)
            {
                cmd.runningProcess.Stop();
            }
            //if cmd is null then dont try to invoke it
            cmd.onTerminateAction?.Invoke();
        }


        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
            {
                command.DynamicInvoke();
            }
            else if (command is Action<string>)
            {
                command.DynamicInvoke(args[0]);
            }
            else if (command is Action<string[]>)
            {
                command.DynamicInvoke((object)args);
            }
            else if (command is Func<IEnumerator>)
            {
                yield return ((Func<IEnumerator>)command)();
            }
            else if (command is Func<string, IEnumerator>)
            {
                yield return ((Func<string, IEnumerator>)command)(args[0]);
            }
            else if (command is Func<string[], IEnumerator>)
            {
                yield return ((Func<string[], IEnumerator>)command)(args);
            }
        }

        public void AddTerminationActionToCurrentProcess(UnityAction action)
        {
            CommandProcess process = topProcess;
            if(process == null)
            {
                //do nothing if its already null
                return;
            }
            process.onTerminateAction = new UnityEvent();
            process.onTerminateAction.AddListener(action);
        }

        //function that creates a subdatabase when needed
        public CommandDatabase CreateSubDatabase(string name)
        {
            name = name.ToLower();

            //if the database already exists
            if(subDatabases.TryGetValue(name, out CommandDatabase db))
            {
                Debug.LogWarning($"A database by the name of '{name}' already exists!");
                return db;
            }

            //if were here, we dont have an existing database so create one
            CommandDatabase newDataBase = new CommandDatabase();
            subDatabases.Add(name, newDataBase);
            return newDataBase;
        }
    }
}