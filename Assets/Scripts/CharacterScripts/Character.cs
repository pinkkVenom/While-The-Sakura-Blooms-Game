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
        public Animator animator;


        //reference to character manager
        protected CharacterManager manager => CharacterManager.instance;

        //reference to dialogue system
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        //coroutines
        protected Coroutine co_revealing, co_hiding;
        //public bools that check if coroutines are active
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        //check if character is visible in the scene
        public virtual bool isVisible => false;

        //create character constructor (empty here, but inheritance classes use it to make their own)
        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if(prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
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

        //this will reveal characters
        public virtual Coroutine Show()
        {
            if (isRevealing)
            {
                return co_revealing;
            }
            if (isHiding)
            {
                manager.StopCoroutine(co_hiding);
            }
            co_revealing = manager.StartCoroutine(ShowingOrHiding(true));
            return co_revealing;
        }

        //this will hide characters
        public virtual Coroutine Hide()
        {
            if (isHiding)
            {
                return co_hiding;
            }
            if (isRevealing)
            {
                manager.StopCoroutine(co_revealing);
            }
            co_hiding = manager.StartCoroutine(ShowingOrHiding(false));
            return co_hiding;
        }

        //check if hide or show is already happening
        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.Log("Show/Hide cannot be called from base character type");
            yield return null;
        }

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