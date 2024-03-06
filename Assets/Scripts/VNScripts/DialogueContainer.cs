using UnityEngine;
using TMPro;
using System.Collections;

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

        private CanvasGroupController cgController;

        public void SetDialogueColor(Color color) => dialogueText.color = color;
        public void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;
        public void SetDialogueFontSize(float size) => dialogueText.fontSize = size;

        private bool initialized = false;
        public void Initialize()
        {
            if (initialized)
            {
                return;
            }
            cgController = new CanvasGroupController(DialogueSystem.instance, UIRoot.GetComponent<CanvasGroup>());
        }

        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);

        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
    }
}
