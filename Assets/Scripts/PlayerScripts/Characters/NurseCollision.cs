using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class NurseCollision : MonoBehaviour
{
    //Intro
    [SerializeField]TextAsset NurseIntro = null;
    [SerializeField] private TextAsset NurseBusy = null;
    //Romance Questing
    //QUEST 1
    public TextAsset nurseQuest1Start = null;
    public TextAsset nurseQuest1End = null;
    public static bool quest1Done = false;
    //QUEST 2
    public TextAsset nurseQuest2End = null;
    public static bool quest2Done = false;
    //QUEST 3
    public TextAsset nurseQuest3End = null;
    public static bool quest3Done = false;
    //QUEST 4
    public TextAsset nurseQuest4End = null;
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
                    StartConversation(NurseIntro);
                    hasSpoken = true;
                    return;
                }
                else
                {
                    StartConversation(NurseBusy);
                }
            }
            if (StoryManager.storyIndex == 3)
            {
                if (CEOCollision.chosenRomance == false && LibrarianCollision.chosenRomance == false && ArtistCollision.chosenRomance == false)
                {
                    Input.ResetInputAxes();
                    chosenRomance = true;
                    StartConversation(nurseQuest1Start);
                }
            }
            if (StoryManager.storyIndex == 4)
            {
                //CEO Quest 2 (Coffee)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(nurseQuest1End);
                    finishedQuest = false;
                    quest1Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(NurseBusy);
                }
            }
            if (StoryManager.storyIndex == 5)
            {
                //CEO Quest 2 (Coffee)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(nurseQuest2End);
                    finishedQuest = false;
                    quest2Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(NurseBusy);
                }
            }
            if (StoryManager.storyIndex == 6)
            {
                //CEO Quest 2 (Coffee)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(nurseQuest3End);
                    finishedQuest = false;
                    quest3Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(NurseBusy);
                }
            }
            if (StoryManager.storyIndex == 7)
            {
                //CEO Quest 2 (Coffee)
                if (finishedQuest == true)
                {
                    Input.ResetInputAxes();
                    StartConversation(nurseQuest4End);
                    finishedQuest = false;
                    quest4Done = true;
                }
                else if (finishedQuest == false)
                {
                    StartConversation(NurseBusy);
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
