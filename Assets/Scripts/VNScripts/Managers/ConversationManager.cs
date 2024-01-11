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

        //gives the conversationmanager access to the text manager so that they
        //can share data without other classes having access to text manager
        private TextManager textManager = null;
        //for subscribing to the event in DialogueSystem
        private bool userPrompt = false;
        public ConversationManager(TextManager textManager)
        {
            this.textManager = textManager;
            //this triggers the onuserprompt method
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

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
            //shows the speaker name if they have one
            if (line.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(line.speaker);
            }

            //build dialogue
            yield return BuildLineSegments(line.dialogue);

            //wait for user input
            yield return WaitForUserInput();
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        //enumerator that builds dialogue from segments
        IEnumerator BuildLineSegments (DL_DIALOGUE_DATA line)
        {
            for(int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];
                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        //wait for user input to trigger signal
        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                //if something else happens (unlikely)
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            //build dialogue
            if (!append)
            {
                textManager.Build(dialogue);
            }
            else
            {
                textManager.Append(dialogue);
            }

            //wait for dialogue to finish building
            while (textManager.isBuilding)
            {
                if (userPrompt)
                {
                    if (!textManager.hurryUpText)
                    {
                        textManager.hurryUpText = true;
                    }
                    else
                    {
                        textManager.ForceComplete();
                    }
                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
            {
                yield return null;
            }
            userPrompt = false;
        }

    }
}
