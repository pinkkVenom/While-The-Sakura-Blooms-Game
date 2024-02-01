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

        public const bool ENABLE_ON_START = true;

        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Animator animator;
        public Color color { get; protected set; } = Color.white;


        //reference to character manager
        protected CharacterManager manager => CharacterManager.instance;

        //reference to dialogue system
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        //coroutines
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;

        //public bools that check if coroutines are active
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public bool isChangingColor => co_changingColor != null;
        //check if character is visible in the scene
        public virtual bool isVisible { get; set; }

        //create character constructor (empty here, but inheritance classes use it to make their own)
        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if(prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.name = manager.FormatCharacterPath(manager.characterPrefabNameFormat, name);
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

        public virtual void SetPosition(Vector2 position)
        {
            if(root == null)
            {
                return;
            }
            //operates based on anchor positions, not rect transform
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (root == null)
            {
                return null;
            }
            if (isMoving)
            {
                manager.StopCoroutine(co_moving);
            }
            co_moving = manager.StartCoroutine(MovingToPosition(position, speed, smooth));
            return co_moving;
        }

        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            //loop until we reach our desired position
            while(root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = smooth ?
                    Vector2.Lerp(root.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                    : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                root.anchorMax = root.anchorMin + padding;
                //threshold for auto complete movement
                if(smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;
                    break;
                }

                yield return null;
            }
            Debug.Log("Done Moving");
            co_moving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position)
        {
            Vector2 padding = root.anchorMax - root.anchorMin;
            //normalized space
            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }

        //changing character colors
        public virtual void SetColor(Color color)
        {
            this.color = color;
        }

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            this.color = color;
            if (isChangingColor)
            {
                manager.StopCoroutine(co_changingColor);
            }
            co_changingColor = manager.StartCoroutine(ChangingColor(color, speed));
            return co_changingColor;
        }

        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Color changing is not applicable on this character type!");
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