using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

public class ArtistCollision : MonoBehaviour
{
    //Intro
    public TextAsset ArtistIntro = null;
    public TextAsset ArtistBusy = null;
    //Romance Questing
    //QUEST 1
    public TextAsset artistQuest1Start = null;
    public TextAsset artistQuest1End = null;
    public static bool quest1Done = false;
    //QUEST 2
    public TextAsset artistQuest2End = null;
    public static bool quest2Done = false;
    //QUEST 3
    public TextAsset artistQuest3End = null;
    public static bool quest3Done = false;
    //QUEST 4
    public TextAsset artistQuest4End = null;
    public static bool quest4Done = false;
    //QUEST 5
    public TextAsset artistQuest5End = null;
    public static bool quest5Done = false;

    [SerializeField] private CanvasGroup icon;
    public static bool hasSpoken;
    public static bool chosenRomance;
    public static bool inRange = false;
    public static bool finishedQuest = false;

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

    public static void StartConversation(TextAsset asset)
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
                if (NurseCollision.chosenRomance == false && LibrarianCollision.chosenRomance == false && CEOCollision.chosenRomance == false)
                {
                    Input.ResetInputAxes();
                    chosenRomance = true;
                    StartConversation(artistQuest1Start);
                }
            }
            if (StoryManager.storyIndex == 14)
            {
                //Artist Quest 1 (Find Rose)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(artistQuest1End);
                    finishedQuest = false;
                    quest1Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(ArtistBusy);
                }
            }
            if (StoryManager.storyIndex == 15)
            {
                //Artist Quest 2 (Go To Museum)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(artistQuest2End);
                    finishedQuest = false;
                    quest2Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(ArtistBusy);
                }
            }
            if (StoryManager.storyIndex == 16)
            {
                //Artist Quest 2 (Go To Museum)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(artistQuest3End);
                    finishedQuest = false;
                    quest3Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(ArtistBusy);
                }
            }
            if (StoryManager.storyIndex == 17)
            {
                //Artist Quest 2 (Go To Museum)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(artistQuest4End);
                    finishedQuest = false;
                    quest4Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(ArtistBusy);
                }
            }
            if (StoryManager.storyIndex == 18)
            {
                //Artist Quest 2 (Go To Museum)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(artistQuest5End);
                    finishedQuest = false;
                    quest5Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(ArtistBusy);
                }
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
