using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class ArtistCollision : MonoBehaviour
{
    //Intro
    [SerializeField] private TextAsset ArtistIntro = null;
    [SerializeField] private TextAsset ArtistBusy = null;
    //Romance Questing

    [SerializeField] private CanvasGroup icon;
    public static bool hasSpoken;
    public static bool chosenRomance;
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

    private void StartConvo()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (StoryManager.storyIndex == 0)
            {
                icon.alpha = 0;
                if (hasSpoken == false)
                {
                    StartConversation(ArtistIntro);
                    hasSpoken = true;
                    return;
                }
                else
                {
                    StartConversation(ArtistBusy);
                }
            }
            if (StoryManager.storyIndex == 3)
            {

            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        icon.alpha = 0;
        inRange = false;
    }
}
