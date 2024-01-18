using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

//responsible for validating and executing commands found in text files
//this class will handle everything independantly
//is connected to gameobject in unity scene
public class CommandManager : MonoBehaviour
{
    public static CommandManager instance { get; private set; }
    private static Coroutine process = null;
    public static bool isRunningProcess => process != null;

    private CommandDatabase database;

    private void Awake()
    {
        //check if theres only one commandmanager running at a time (should only be 1)
        if(instance == null)
        {
            instance = this;
            database = new CommandDatabase();
            //assign this to current executing assembly, gets reference to current executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            //type array, we dont know every extension that we will have (theyre all different)
            //finds all types of our database extension
            Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

            //execute extend method on each extension class found
            foreach(Type extension in extensionTypes)
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

    public Coroutine Execute(string commandName, params string[] args)
    {
        Delegate command = database.GetCommand(commandName);
        if (command != null)
        {
            return null;
        }

        return StartProcess(commandName, command, args);
    }

    private Coroutine StartProcess(string commandName, Delegate command, string[] args)
    {
        StopCurrentProces();
        process = StartCoroutine(RunningProcess(command, args));
        return process;
    }

    private void StopCurrentProces()
    {
        if(process != null)
        {
            StopCoroutine(process);
            process = null;
        }
    }

    private IEnumerator RunningProcess(Delegate command, string[] args)
    {
        yield return WaitingForProcessToComplete(command, args);

        process = null;
    }

    private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
    {
        if(command is Action)
        {
            command.DynamicInvoke();
        }
        else if(command is Action<string>)
        {
            command.DynamicInvoke(args[0]);
        }
        else if (command is Action<string[]>)
        {
            command.DynamicInvoke((object)args);
        }
        else if(command is Func<IEnumerator>)
        {
            yield return ((Func<IEnumerator>)command)();
        }
        else if(command is Func<string, IEnumerator>)
        {
            yield return ((Func<string, IEnumerator>)command)(args[0]);
        }
        else if(command is Func<string[], IEnumerator>)
        {
            yield return ((Func<string[], IEnumerator>)command)(args);
        }
    }
}
