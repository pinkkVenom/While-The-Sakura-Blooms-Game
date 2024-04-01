using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.UI;

public class NurseCollision : MonoBehaviour
{
    [SerializeField] private TextAsset NurseFile = null;
    [SerializeField] private TextAsset NurseFile2 = null;
    [SerializeField] private CanvasGroup icon;
    bool hasSpoken;
    bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        hasSpoken = false;
        icon.alpha = 0;
    }

    private void Update()
    {
        if(inRange == true)
        {
            StartConvo();
        }
    }

    void StartConversation(TextAsset asset)
    {
        List<string> lines = FileManager.ReadTextAsset(asset);

        DialogueSystem.instance.Say(lines);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            icon.alpha = Mathf.MoveTowards(icon.alpha, 1, 1);
        }

    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }

    }

    private void StartConvo()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            icon.alpha = 0;
            if (hasSpoken == false)
            {
                StartConversation(NurseFile);
                hasSpoken = true;
                return;
            }
            else
            {
                StartConversation(NurseFile2);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        icon.alpha = 0;
        inRange = false;
    }
}
