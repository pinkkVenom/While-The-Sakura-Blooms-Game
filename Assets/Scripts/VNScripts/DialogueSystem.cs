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
        private ConversationManager conversationManager = new ConversationManager();

        public static DialogueSystem instance;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            //only 1 dialogue system allowed at a time
            if (instance = null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

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
