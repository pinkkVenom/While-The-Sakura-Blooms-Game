using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using TMPro;

//defines the parameters for configuring the dialogue system as a whole
namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigAsset;

        //define the default color and font
        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;

        public float dialogueFontScale = 1f;
        public float defaultDialogueFontSize = 18;
        public float defaultNameFontSize = 22;
    }
}