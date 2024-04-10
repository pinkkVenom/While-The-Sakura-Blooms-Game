using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

public class LibrarianCollision : MonoBehaviour
{
    //Intro
    [SerializeField] private TextAsset LibIntro = null;
    [SerializeField] private TextAsset LibBusy = null;
    //Romance Questing
    //QUEST 1
    public TextAsset libQuest1Start = null;
    public TextAsset libQuest1End = null;
    public static bool quest1Done = false;
    //QUEST 2
    public TextAsset libQuest2End = null;
    public static bool quest2Done = false;
    //QUEST 3
    public TextAsset libQuest3End = null;
    public static bool quest3Done = false;
    //QUEST 4
    public TextAsset libQuest4End = null;
    public static bool quest4Done = false;


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
                    StartConversation(LibIntro);
                    hasSpoken = true;
                    return;
                }
                else
                {
                    StartConversation(LibBusy);
                }
            }
            if (StoryManager.storyIndex == 3)
            {
                if (NurseCollision.chosenRomance == false && CEOCollision.chosenRomance == false && ArtistCollision.chosenRomance == false)
                {
                    Input.ResetInputAxes();
                    chosenRomance = true;
                    StartConversation(libQuest1Start);
                }
            }
            if (StoryManager.storyIndex == 19)
            {
                //Lib Quest 1 (Find Scroll)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(libQuest1End);
                    finishedQuest = false;
                    quest1Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(LibBusy);
                }
            }
            if (StoryManager.storyIndex == 20)
            {
                //Lib Quest 2 (Get overdue book)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(libQuest2End);
                    finishedQuest = false;
                    quest2Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(LibBusy);
                }
            }
            if (StoryManager.storyIndex == 21)
            {
                //Lib Quest 3 (find a pen)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(libQuest3End);
                    finishedQuest = false;
                    quest3Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(LibBusy);
                }
            }
            if (StoryManager.storyIndex == 22)
            {
                //Lib Quest 4 (find lost book)

                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(libQuest4End);
                    finishedQuest = false;
                    quest4Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(LibBusy);
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
