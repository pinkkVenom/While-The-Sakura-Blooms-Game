using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

public class CEOCollision : MonoBehaviour
{
    //Intro
    public TextAsset CEOIntro = null;
    public TextAsset CEOBusy = null;
    //Romance Questing
    //QUEST 1
    public TextAsset CEOQuest1Start = null;
    public TextAsset CEOQuest1End = null;
    public TextMeshProUGUI money;
    public static bool quest1Done = false;
    //QUEST 2
    public TextAsset CEOQuest2End = null;
    public static bool quest2Done = false;
    //QUEST 3
    public TextAsset CEOQuest3End = null;
    public static bool quest3Done = false;
    //QUEST 4
    public TextAsset CEOQuest4End = null;
    public static bool quest4Done = false;
    //QUEST 5
    public TextAsset CEOQuest5End = null;
    public static bool quest5Done = false;

    public CanvasGroup icon;
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
        if(inRange == true)
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
                    Input.ResetInputAxes();
                    chosenRomance = true;
                    StartConversation(CEOQuest1Start);
                }
            }
            if (StoryManager.storyIndex == 9)
            {
                //CEO Quest 1 (Diamond Store)
                
                if(finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(CEOQuest1End);
                    finishedQuest = false;
                    quest1Done = true;
                    money.text = "0";
                }
                else if (finishedQuest == false)
                {
                    StartConversation(CEOBusy);
                }
            }
            if(StoryManager.storyIndex == 10)
            {
                //CEO Quest 2 (Coffee)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(CEOQuest2End);
                    finishedQuest = false;
                    quest2Done = true;
                }
                else if(finishedQuest == false)
                {
                    StartConversation(CEOBusy);
                }
            }
            if(StoryManager.storyIndex == 11)
            {
                //CEO Quest 3 (Talk to Coffee Shop Owner)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(CEOQuest3End);
                    finishedQuest = false;
                    quest3Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(CEOBusy);
                }
            }
            if(StoryManager.storyIndex == 12)
            {
                //CEO Quest 4 (Find Cloak)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(CEOQuest4End);
                    finishedQuest = false;
                    quest4Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(CEOBusy);
                }
            }
            if(StoryManager.storyIndex == 13)
            {
                //CEO Quest 5 (Find family heirloom)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(CEOQuest5End);
                    finishedQuest = false;
                    quest5Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(CEOBusy);
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
