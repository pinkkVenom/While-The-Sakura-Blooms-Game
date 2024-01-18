using UnityEngine;
using TMPro;

//this code controls the graphic display of dialogue (text, box, other elements)

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        //for hiding the dialogue
        public GameObject UIRoot;
        //for displaying characters name
        public NameContainer nameContainer;
        //for displaying the spoken dialogue
        public TextMeshProUGUI dialogueText;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
    }
}
