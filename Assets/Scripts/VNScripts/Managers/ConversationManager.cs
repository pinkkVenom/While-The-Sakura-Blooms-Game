using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all logic to run dialogue on screen
//one dialogue line at a time

namespace DIALOGUE
{


    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        //check if there is an ongoing conversation
        private Coroutine process = null;
        public bool isRunning => process != null;

        public void StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
            {
                return;
            }
            else
            {
                dialogueSystem.StopCoroutine(process);
                process = null;
            }
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            //loop through all lines
            for(int i = 0; i < conversation.Count; i++)
            {
                //skip all empty lines
                if(string.IsNullOrWhiteSpace(conversation[i]))
                {
                    continue;
                }
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //show the dialogue on screen
                if (line.hasDialogue)
                {
                    if (line.hasSpeaker)
                    {
                        //call the dialogue coroutine
                        yield return Line_RunDialogue(line);
                    }
                }

                //identify and use command lines
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {

        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {

        }

    }
}
