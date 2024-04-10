using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class FactionFCollision : MonoBehaviour
{
    [SerializeField] TextAsset endNurse;
    [SerializeField] TextAsset endCEO;
    [SerializeField] TextAsset endLib;
    [SerializeField] TextAsset endArtist;
    [SerializeField] private CanvasGroup icon;
    public static bool hasSpoken;
    bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        hasSpoken = false;
        icon.alpha = 0;
    }

    private void Update()
    {
        if (inRange == true)
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
            if(StoryManager.storyIndex == 24)
            {
                StartConversation(endCEO);
            }
            if (StoryManager.storyIndex == 25)
            {
                StartConversation(endNurse);
            }
            if (StoryManager.storyIndex == 26)
            {
                StartConversation(endLib);
            }
            if (StoryManager.storyIndex == 27)
            {
                StartConversation(endArtist);
            }

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        icon.alpha = 0;
        inRange = false;
    }
}
