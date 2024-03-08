using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DIALOGUE;

namespace COMMAND
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        private static string[] PARAM_SPEED => new string[] { "-spd", "-speed" };
        private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate" };
        private static string[] PARAM_FILEPATH => new string[] { "-f", "-file", "-filepath" };
        private static string[] PARAM_ENQUEUE => new string[] { "-e", "-enqueue" };

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));

            //dialogue system controls
            database.AddCommand("showui", new Func<string[], IEnumerator>(ShowDialogueSystem));
            database.AddCommand("hideui", new Func<string[], IEnumerator>(HideDialogueSystem));

            //dialogue controls
            database.AddCommand("showdb", new Func<string[], IEnumerator>(ShowDialogueBox));
            database.AddCommand("hidedb", new Func<string[], IEnumerator>(HideDialogueBox));

            //dynamic story controlling
            database.AddCommand("load", new Action<string[]>(LoadNewDialogueFile));
        }

        private static void LoadNewDialogueFile(string[] data)
        {
            string fileName = string.Empty;
            bool enqueue = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_FILEPATH, out fileName);
            parameters.TryGetValue(PARAM_ENQUEUE, out enqueue, defaultValue: false);

            string filePath = FilePaths.GetPathToResources(FilePaths.resources_dialogueFiles, fileName);
            TextAsset file = Resources.Load<TextAsset>(filePath);

            if(file == null)
            {
                Debug.LogWarning($"File '{filePath}' could not be loaded from dialogue files.");
                return;
            }

            List<string> lines = FileManager.ReadTextAsset(file, includeBlankLines: true);
            Conversation newConversation = new Conversation(lines);

            if (enqueue)
            {
                DialogueSystem.instance.conversationManager.Enqueue(newConversation);
            }
            else
            {
                DialogueSystem.instance.conversationManager.StartConversation(newConversation);
            }
        }

        //takes in a single value for time
        //time is in seconds
        private static IEnumerator Wait(string data)
        {
            //try parsing to get a type
            if(float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
        }

        private static IEnumerator ShowDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);


            yield return DialogueSystem.instance.Show(speed, immediate);
        }

        private static IEnumerator HideDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Hide(speed, immediate);
        }

        private static IEnumerator ShowDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Show(speed, immediate);
        }

        private static IEnumerator HideDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Hide(speed, immediate);
        }
    }
}
