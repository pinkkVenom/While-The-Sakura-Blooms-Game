using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

//this code is the main controller for starting and controlling dialogue conversations on screen

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        //we don't want anything to change the config so we make a second private declaration
        [SerializeField] private DialogueSystemConfigSO _config;
        public DialogueSystemConfigSO config => _config;

        //accessible from inspector
        public DialogueContainer dialogueContainer = new DialogueContainer();
        public ConversationManager conversationManager { get; private set; }
        private TextManager textManager;
        public AutoReader autoReader { get; private set; }

        [SerializeField] private CanvasGroup mainCanvas;

        public static DialogueSystem instance { get; private set; }

        //event that listens to button press to skip through dialogue faster
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;
        public event DialogueSystemEvent OnClear;

        public bool isRunningConversation => conversationManager.isRunning;

        public DialogueContinuePrompt prompt;
        public static CanvasGroupController cgController;

        private void Awake()
        {
            //only 1 dialogue system allowed at a time
            if (instance == null)
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

            cgController = new CanvasGroupController(this, mainCanvas);
            dialogueContainer.Initialize();

            autoReader = GetComponent<AutoReader>();
            //get auto reader if enabled
            if(autoReader != null)
            {
                autoReader.Initialize(conversationManager);
            }
        }

        //function called to trigger the keypress event
        public void OnUserPrompt_Next()
        {
            //the question mark means that if its null then nothing will happen
            onUserPrompt_Next?.Invoke();
            if(autoReader !=null && autoReader.isOn)
            {
                autoReader.Disable();
            }
        }
        public void OnSystemPrompt_Next()
        {
            //the question mark means that if its null then nothing will happen
            onUserPrompt_Next?.Invoke();
        }
        public void OnSystemPrompt_Clear()
        {
            OnClear?.Invoke();
        }

        public void OnStartViewingHistory()
        {
            prompt.Hide();
            autoReader.allowToggle = false;
            conversationManager.allowUserPrompts = false;
            if (autoReader.isOn)
            {
                autoReader.Disable();
            }
        }

        public void OnStopViewingHistory()
        {
            prompt.Show();
            autoReader.allowToggle = true;
            conversationManager.allowUserPrompts = true;
        }

        //apply config data to speaker
        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.instance.GetCharacter(speakerName);
            //get configuration assigned to character
            CharacterConfigData config = character != null ? character.config : CharacterManager.instance.GetCharacterConfig(speakerName);

            ApplySpeakerDataToDialogueContainer(config);
        }

        //goes through config to find character
        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config)
        {
            dialogueContainer.SetDialogueColor(config.dialogueColor);
            dialogueContainer.SetDialogueFont(config.dialogueFont);
            float fontSize = this.config.defaultDialogueFontSize * this.config.dialogueFontScale * config.dialogueFontScale;
            dialogueContainer.SetDialogueFontSize(fontSize);

            dialogueContainer.nameContainer.SetNameColor(config.nameColor);
            dialogueContainer.nameContainer.SetNameFont(config.nameFont);
            fontSize = this.config.defaultNameFontSize * config.nameFontScale;
            dialogueContainer.nameContainer.SetNameFontSize(fontSize);

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
                dialogueContainer.nameContainer.nameText.text = "";
            }
        }
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        //method called when we want anything to show up in-game on-screen
        //Type 1: individual lines passed into string which becomes the conversation
        public Coroutine Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        //Type 2: entire conversation (array)
        public Coroutine Say(List<string> lines, string filePath = "")
        {
            Conversation conversation = new Conversation(lines, file: filePath);
            return conversationManager.StartConversation(conversation);
        }

        public Coroutine Say(Conversation conversation)
        {
            return conversationManager.StartConversation(conversation);
        }

        public static bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);

        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
    }
}
