using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//shows a prompt for users on screen that will indicate to them to progress when in a dialogue scene
namespace DIALOGUE
{
    public class DialogueContinuePrompt : MonoBehaviour
    {
        private RectTransform root;
        [SerializeField] private Animator anim;
        [SerializeField] private TextMeshProUGUI tmpro;

        public bool isShowing => anim.gameObject.activeSelf;

        // Start is called before the first frame update
        void Start()
        {
            root = GetComponent<RectTransform>();
        }
        
        public void Show()
        {
            if(tmpro.text == string.Empty)
            {
                if (isShowing)
                {
                    Hide();
                }
                return;
            }

            tmpro.ForceMeshUpdate();

            anim.gameObject.SetActive(true);
            root.transform.SetParent(tmpro.transform);

            TMP_CharacterInfo finalCharacter = tmpro.textInfo.characterInfo[tmpro.textInfo.characterCount - 1];
            Vector3 targetPos = finalCharacter.bottomRight;

            //account for the size of the final character (w is larger than !)
            float characterWidth = finalCharacter.pointSize * 0.65f;
            targetPos = new Vector3(targetPos.x + characterWidth, targetPos.y + 5f, 0);

            root.localPosition = targetPos;
        }

        public void Hide()
        {
            anim.gameObject.SetActive(false);
        }
    }
}
