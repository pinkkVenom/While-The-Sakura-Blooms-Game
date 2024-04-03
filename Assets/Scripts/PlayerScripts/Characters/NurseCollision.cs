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
    [SerializeField] TextAsset NurseQ1Start = null;
    [SerializeField] TextAsset NurseQ1End = null;

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
            if (StoryManager.storyIndex <= 2 && StoryManager.storyIndex >=0)
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
            if(StoryManager.storyIndex == 3)
            {
                if(CEOCollision.chosenRomance == false && LibrarianCollision.chosenRomance == false && ArtistCollision.chosenRomance == false)
                {
                    chosenRomance = true;
                    StoryManager.storyIndex = 4;
                }
            }
            if(StoryManager.storyIndex == 4)
            {
                //Nurse Quest 1
                if (finishedQuest == false)
                {
                    StartConversation(NurseQ1Start);
                }
                if(finishedQuest == true)
                {
                    StartConversation(NurseQ1End);
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
