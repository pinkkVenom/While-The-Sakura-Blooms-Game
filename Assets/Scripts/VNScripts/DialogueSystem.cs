using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this code is the main controller for starting and controlling dialogue conversations on screen

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        //accessible from inspector
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextManager textManager;

        public static DialogueSystem instance { get; private set; }

        //event that listens to button press to skip through dialogue faster
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            //only 1 dialogue system allowed at a time
            if (instance = null)
            {
                instance = this;
                Initialize();
            }
            else
            {
                DestroyImmediate(gameObject);
            }

        }

        //initializing the textmanager
        //we only want this class to run once
        bool _initialized = false;
        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            textManager = new TextManager(dialogueContainer.dialogueText);
            //getting access to private class
            conversationManager = new ConversationManager(textManager);
        }

        //function called to trigger the keypress event
        public void OnUserPrompt_Next()
        {
            //the question mark means that if its null then nothing will happen
            onUserPrompt_Next?.Invoke();
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            //if the speaker isnt named narrator
            if(speakerName.ToLower() !="narrator")
            {
                dialogueContainer.nameContainer.Show(speakerName);
            }
            else
            {
                HideSpeakerName();
            }
        }
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        //method called when we want anything to show up in-game on-screen
        //Type 1: individual lines passed into string which becomes the conversation
        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }

        //Type 2: entire conversation (array)
        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }
    }
}
