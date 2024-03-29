using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//controller for dialoguecontainers speaker name field
//this controls the visibility and other logic separate from other classes
namespace DIALOGUE
{
    [System.Serializable]
    public class NameContainer
    {
        //we want the root (the VN controller) to show the name
        [SerializeField] private GameObject root;
        [field:SerializeField] public TextMeshProUGUI nameText { get; private set; }
        public void Show(string nameToShow = "")
        {
            root.SetActive(true);
            if (nameToShow != string.Empty)
            {
                nameText.text = nameToShow;
            }
        }
        public void Hide()
        {
            root.SetActive(false);
        }

        public void SetNameColor(Color color) => nameText.color = color;
        public void SetNameFont(TMP_FontAsset font) => nameText.font = font;
        public void SetNameFontSize(float size) => nameText.fontSize = size;
    }
}
