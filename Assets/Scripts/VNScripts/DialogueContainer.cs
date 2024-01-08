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
        public TextMeshProUGUI nameText;
        //for displaying the spoken dialogue
        public TextMeshProUGUI dialogueText;
    }
}
