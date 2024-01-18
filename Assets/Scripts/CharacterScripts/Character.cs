using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

//base class from which all character types derive from
namespace CHARACTERS
{
    public abstract class Character
    {
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;

        //pointer to dialogue system
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        //create character constructor (empty here, but inheritance classes use it to make their own)
        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            displayName = name;
            this.config = config;
        }

        //these two classes are only for external use cases where the characters aren't speaking from a dialogue file
        //converts dialogue to list
        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });

        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationsOnScreen();
            return dialogueSystem.Say(dialogue);
        }
        ///////////////////////////////////////////////

        //if we change these values, we set them here
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name);

        //force updating the text customization on screen
        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        //types of characters that can exist in the game
        public enum CharacterType 
        { 
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }

    }
}