using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    public class AutoReader : MonoBehaviour
    {
        private const int DEFAULT_CHAR_READ_PER_SECOND = 18;
        private const float READ_TIME_PADDING = 1f;
        private const float MIN_READ_TIME = 1f;
        private const float MAX_READ_TIME = 99f;
        private const string STATUS_TEXT_AUTO = "Auto";
        private const string STATUS_TEXT_SKIP = "Skipping";

        private ConversationManager conversationManager;
        private TextManager textManager => conversationManager.textManager;

        public bool skip { get; set; } = false;
        public float speed { get; set; } = 1f;

        public bool isOn => co_running != null;
        private Coroutine co_running = null;

        [SerializeField] private TextMeshProUGUI statusText;

        public void Initialize(ConversationManager conversationManager)
        {
            this.conversationManager = conversationManager;
            statusText.text = string.Empty;
        }

        public void Enable()
        {
            if (isOn)
            {
                return;
            }
            co_running = StartCoroutine(AutoRead());
        }

        public void Disable()
        {
            if (!isOn)
            {
                return;
            }
            StopCoroutine(co_running);
            skip = false;
            co_running = null;
            statusText.text = string.Empty;
        }

        private IEnumerator AutoRead()
        {
            //do nothing if there is no conversation to monitor
            if (!conversationManager.isRunning)
            {
                Disable();
                yield break;
            }

            if(!textManager.isBuilding && textManager.currentText != string.Empty)
            {
                DialogueSystem.instance.OnSystemPrompt_Next();
            }

            while (conversationManager.isRunning)
            {
                //read and wait
                if (!skip)
                {
                    while (!textManager.isBuilding && !conversationManager.isWaitingOnAutoTimer)
                    {
                        yield return null;
                    }
                    float timeStart = Time.time;

                    while (textManager.isBuilding || conversationManager.isWaitingOnAutoTimer)
                    {
                        yield return null;
                    }
                    float timeToRead = Mathf.Clamp(((float)textManager.tmpro.textInfo.characterCount / DEFAULT_CHAR_READ_PER_SECOND), MIN_READ_TIME, MAX_READ_TIME);
                    timeToRead = Mathf.Clamp((timeToRead - (Time.time - timeStart)), MIN_READ_TIME, MAX_READ_TIME);

                    timeToRead = (timeToRead / speed) + READ_TIME_PADDING;

                    yield return new WaitForSeconds(timeToRead);
                }
                //skip
                else
                {
                    textManager.ForceComplete();
                    yield return new WaitForSeconds(2);
                }
                DialogueSystem.instance.OnSystemPrompt_Next();
            }
            Disable();
        }

        public void Toggle_Auto()
        {

            if (skip)
            {
                Enable();
            }
            else
            {
                if (!isOn)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }
            skip = false;
            statusText.text = STATUS_TEXT_AUTO;
        }

        public void Toggle_Skip()
        {

            if (!skip)
            {
                Enable();
            }
            else
            {
                if (!isOn)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }
            skip = true;
            statusText.text = STATUS_TEXT_SKIP;
        }


    }
}
