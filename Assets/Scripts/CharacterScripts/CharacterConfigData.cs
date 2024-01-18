using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DIALOGUE;

//data container that defines the configuration parameters for a character in the visual novel
namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfigData
    {
        //character info
        public string name;
        //if the character has a long name, we give them an alias to simplify coding
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;


        //make a copy of the character data to preserve the scriptable object data
        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            //the data
            result.name = name;
            result.alias = alias;
            result.characterType = characterType;
            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;

            //different for colors
            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            return result;
        }

        //pointers to the default data in dialoguesystemconfigso
        private static Color defaultColor => DialogueSystem.instance.config.defaultTextColor;
        private static TMP_FontAsset defaultFont => DialogueSystem.instance.config.defaultFont;

        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                //the data
                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                result.nameFont =defaultFont;
                result.dialogueFont = defaultFont;
                //different for colors
                result.nameColor = defaultColor;
                result.dialogueColor = defaultColor;

                return result;
            }
        }
    }


}