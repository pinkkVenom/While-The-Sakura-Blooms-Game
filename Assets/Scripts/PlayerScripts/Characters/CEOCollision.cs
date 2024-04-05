using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class CEOCollision : MonoBehaviour
{
    //Intro
    [SerializeField] private TextAsset CEOIntro = null;
    [SerializeField] private TextAsset CEOBusy = null;
    //Romance Questing
    //QUEST 1
    [SerializeField] TextAsset CEOQuest1Start = null;
    [SerializeField] GameObject shopskeep;
    [SerializeField] TextAsset CEOQuest1End = null;
    //QUEST 2
    [SerializeField] GameObject coffee;
    [SerializeField] TextAsset CEOQuest2Start = null;
    [SerializeField] TextAsset CEOQuest2End = null;

    [SerializeField] private CanvasGroup icon;
    public static bool hasSpoken;
    public static bool chosenRomance;
    bool inRange = false;
    bool finishedQuest = false;

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
            if (StoryManager.storyIndex == 0)
            {
                icon.alpha = 0;
                if (hasSpoken == false)
                {
                    StartConversation(CEOIntro);
                    hasSpoken = true;
                    return;
                }
                else
                {
                    StartConversation(CEOBusy);
                }
            }
            if (StoryManager.storyIndex == 3)
            {
                if (NurseCollision.chosenRomance == false && LibrarianCollision.chosenRomance == false && ArtistCollision.chosenRomance == false)
                {
                    chosenRomance = true;
                    StoryManager.storyIndex = 9;
                }
            }
            if (StoryManager.storyIndex == 9)
            {
                //Nurse Quest 1
                if (finishedQuest == false)
                {
                    //StartConversation();
                }
                if (finishedQuest == true)
                {
                    //StartConversation();
                    finishedQuest = false;
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        icon.alpha = 0;
        inRange = false;
    }
}
