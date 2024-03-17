using COMMAND;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE.LogicalLines;

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
        public bool isOnLogicalLine { get; private set; } = false;

        //gives the conversationmanager access to the text manager so that they
        //can share data without other classes having access to text manager
        public TextManager textManager = null;
        //for subscribing to the event in DialogueSystem
        private bool userPrompt = false;

        private LogicalLineManager logicalLineManager;

        public Conversation conversation => (conversationQueue.IsEmpty() ? null : conversationQueue.top);
        public int conversationProgress => (conversationQueue.IsEmpty() ? -1 : conversationQueue.top.GetProgress());
        private ConversationQueue conversationQueue;

        public bool allowUserPrompts = true;

        public ConversationManager(TextManager textManager)
        {
            this.textManager = textManager;
            //this triggers the onuserprompt method
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;

            logicalLineManager = new LogicalLineManager();

            conversationQueue = new ConversationQueue();
        }

        public Conversation[] GetConversationQueue() => conversationQueue.GetReadOnly();

        public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);
        public void EnqueuePriority(Conversation conversation) => conversationQueue.EnqueuePriority(conversation);

        private void OnUserPrompt_Next()
        {
            if (allowUserPrompts)
            {
                userPrompt = true;
            }
        }

        public Coroutine StartConversation(Conversation conversation)
        {
            StopConversation();
            conversationQueue.Clear();
            Enqueue(conversation);
            process = dialogueSystem.StartCoroutine(RunningConversation());
            return process;
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

        IEnumerator RunningConversation()
        {
            //loop through all lines
            while(!conversationQueue.IsEmpty())
            {
                Conversation currentConversation = conversation;

                if (currentConversation.HasReachedEnd())
                {
                    conversationQueue.Dequeue();
                    continue;
                }

                string rawLine = conversation.CurrentLine();
                //skip all empty lines
                if(string.IsNullOrWhiteSpace(rawLine))
                {
                    TryAdvanceConversation(currentConversation);
                    continue;
                }
                DIALOGUE_LINE line = DialogueParser.Parse(rawLine);

                //Processing Logical Lines
                if (logicalLineManager.TryGetLogic(line, out Coroutine logic))
                {
                    isOnLogicalLine = true;
                    yield return logic;
                }
                else
                {
                    //Show dialogue
                    if (line.hasDialogue)
                        yield return Line_RunDialogue(line);

                    //Run any commands
                    if (line.hasCommands)
                        yield return Line_RunCommands(line);

                    //wait for user input if dialogue was in this line
                    if (line.hasDialogue)
                    {
                        //Wait for user input
                        yield return WaitForUserInput();

                        CommandManager.instance.StopAllProcesses();

                        dialogueSystem.OnSystemPrompt_Clear();
                    }
                }

                TryAdvanceConversation(currentConversation);
                isOnLogicalLine = false;
            }

            process = null;
        }

        private void TryAdvanceConversation(Conversation conversation)
        {
            conversation.IncrementProgress();

            if(conversation != conversationQueue.top)
            {
                return;
            }

            if (conversation.HasReachedEnd())
            {
                conversationQueue.Dequeue();
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //shows the speaker name if they have one
            if (line.hasSpeaker)
            {
                HandleSpeakerLogic(line.speakerData);
            }
            if (!dialogueSystem.dialogueContainer.isVisible)
            {
                dialogueSystem.dialogueContainer.Show();
            }

            //build dialogue
            yield return BuildLineSegments(line.dialogueData);

        }
        private void HandleSpeakerLogic(DL_SPEAKER_DATA speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);

            //check if character is forced to enter
            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
            {
                character.Show();
            }

            //add character name to the UI
            dialogueSystem.ShowSpeakerName(TagManager.Inject(speakerData.displayName));

            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            //check if we need to cast position
            if (speakerData.isCastingPosition)
            {
                character.MoveToPosition(speakerData.castPosition);
            }

            //check if we need to cast expressions
            if (speakerData.isCastingExpressions)
            {
                //ce is cast expressions
                foreach(var ce in speakerData.CastExpressions)
                {
                    character.OnRecieveCastingExpression(ce.layer, ce.expression);
                }
            }
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            //Debug.Log(line.commandData);
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;
            foreach(DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion || command.name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);
                    while (!cw.IsDone)
                    {
                        if (userPrompt)
                        {
                            CommandManager.instance.StopCurrentProces();
                            userPrompt = false;
                        }
                        yield return null;
                    }
                }
                else
                {
                    CommandManager.instance.Execute(command.name, command.arguments);
                }
            }
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


        public bool isWaitingOnAutoTimer { get; private set; } = false;
        //wait for user input to trigger signal
        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                    yield return WaitForUserInput();
                    dialogueSystem.OnSystemPrompt_Clear();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                    isWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(segment.signalDelay);
                    isWaitingOnAutoTimer = false;
                    dialogueSystem.OnSystemPrompt_Clear();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    isWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(segment.signalDelay);
                    isWaitingOnAutoTimer = false;
                    break;
                //if something else happens (unlikely)
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            dialogue = TagManager.Inject(dialogue);

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
            dialogueSystem.prompt.Show();

            while (!userPrompt)
            {
                yield return null;
            }
            dialogueSystem.prompt.Hide();

            userPrompt = false;
        }

    }
}
