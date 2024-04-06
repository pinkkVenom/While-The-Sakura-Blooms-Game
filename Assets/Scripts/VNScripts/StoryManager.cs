using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;


public class StoryManager : MonoBehaviour
{ 
    public static int storyIndex;
    [SerializeField] CanvasGroup VN;
    [SerializeField] Animator sakuraTree;
    public Animator likeHeart;
    public Animator badHeart;
    public static bool animLike;
    public static bool animDislike;
    
    //Quest Prefabs
    [SerializeField] GameObject Quest1;
    [SerializeField] GameObject Quest2;
    [SerializeField] GameObject Quest3;
    

    //Story part 1
    private bool hasMetNurse => NurseCollision.hasSpoken;
    private bool hasMetArtist => ArtistCollision.hasSpoken;
    private bool hasMetCEO => CEOCollision.hasSpoken;
    private bool hasMetLibrarian => LibrarianCollision.hasSpoken;

    //Story part 2
    [SerializeField] private TextAsset monologue1 = null;
    [SerializeField] GameObject doctor;
    bool hasSpokenDoc => DoctorCollision.hasSpoken;

    //Story part 3
    bool hasMetFG => FactionMCollision.hasSpoken;
    [SerializeField] GameObject factionGood;

    //Story part 4
    public bool chosenNurse => NurseCollision.chosenRomance;
    public bool chosenCEO => CEOCollision.chosenRomance;
    public bool chosenLibrarian;
    public bool chosenArtist;

    //CEO Questing
    public GameObject CEOQuest1;
    public GameObject CEOQuest2;
    public GameObject CEOQuest3;
    public GameObject CEOQuest4;
    public GameObject CEOQuest5;

    void Start()
    {
        doctor.SetActive(false);
        factionGood.SetActive(false);
        SetStoryIndex();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"{animLike}");
        //Debug.Log($"{storyIndex}");
        if (VN.alpha == 0)
        {
            //Quest 1: Meet all characters
            if (hasMetArtist == true && hasMetCEO == true && hasMetLibrarian == true && hasMetNurse == true && storyIndex == 0)
            {
                storyIndex = 1;
                SetStoryIndex();
            }
            //Quest 2: Go back to Doctor
            if(hasSpokenDoc == true && storyIndex == 1)
            {
                storyIndex = 2;
                SetStoryIndex();
            }
            //Quest 3: Talk to Good Faction Leader for the first time by sakura tree
            if(hasMetFG == true && storyIndex == 2)
            {
                storyIndex = 3;
                SetStoryIndex();
            }
            if (chosenCEO == true && storyIndex == 3)
            {
                storyIndex = 9;
                SetStoryIndex();
            }
            //Quest 4: Pick a single Immortal to romance
            //romancing happens in individual characters scripts
            //4, 5, 6, 7, 8 are nurse
            //9, 10, 11, 12, 13 are CEO
            if(CEOCollision.quest1Done == true && storyIndex == 9)
            {
                storyIndex = 10;
                SetStoryIndex();
            }
            if(CEOCollision.quest2Done == true && storyIndex == 10)
            {
                storyIndex = 11;
                SetStoryIndex();
            }
            if (CEOCollision.quest3Done == true && storyIndex == 11)
            {
                storyIndex = 12;
                SetStoryIndex();
            }
            if (CEOCollision.quest4Done == true && storyIndex == 12)
            {
                storyIndex = 13;
                SetStoryIndex();
            }
            if (CEOCollision.quest5Done == true && storyIndex == 13)
            {
                storyIndex = 24;
                SetStoryIndex();
            }
            //14, 15, 16, 17, 18 are Artist
            //19, 20, 21, 22, 23 are Librarian
            //24 is start of the ending
            if (storyIndex == 24)
            {
                
            }
        }
        
    }
    void StartConversation(TextAsset asset)
    {
        List<string> lines = FileManager.ReadTextAsset(asset);
        DialogueSystem.instance.Say(lines);
    }

    void SetStoryIndex()
    {
        if(storyIndex == 0)
        {
            sakuraTree.Play("Sakura1");
            //badHeart.Play("LikedAnswer");
            Quest1.SetActive(true);
            return;
        }
        if(storyIndex == 1)
        {
            sakuraTree.Play("Sakura1");
            Quest2.SetActive(true);
            Quest1.SetActive(false);
            StartConversation(monologue1);
            doctor.SetActive(true);
        }
        if (storyIndex == 2)
        {
            sakuraTree.Play("Sakura1");
            Quest1.SetActive(false);
            Quest2.SetActive(false);
            Quest3.SetActive(true);
            doctor.SetActive(true);
            factionGood.SetActive(true);
        }
        if(storyIndex == 3)
        {
            sakuraTree.Play("Sakura2");
            Quest1.SetActive(false);
            Quest2.SetActive(false);
            Quest3.SetActive(false);
            factionGood.SetActive(false);
            //insert monologue 2 about picking 1 immortal
        }
        if(storyIndex <= 23 && storyIndex >= 4)
        {
            sakuraTree.Play("Sakura3");
            //romance is happening
            doctor.SetActive(true);
            factionGood.SetActive(false);
            if(storyIndex == 9)
            {
                CEOQuest1.SetActive(true);
                CEOQuest2.SetActive(false);
                CEOQuest3.SetActive(false);
                CEOQuest4.SetActive(false);
                CEOQuest5.SetActive(false);
            }
            if(storyIndex == 10)
            {
                CEOQuest1.SetActive(false);
                CEOQuest2.SetActive(true);
                CEOQuest3.SetActive(false);
                CEOQuest4.SetActive(false);
                CEOQuest5.SetActive(false);
            }
            if (storyIndex == 11)
            {
                CEOQuest1.SetActive(false);
                CEOQuest2.SetActive(false);
                CEOQuest3.SetActive(true);
                CEOQuest4.SetActive(false);
                CEOQuest5.SetActive(false);
            }
            if (storyIndex == 12)
            {
                CEOQuest1.SetActive(false);
                CEOQuest2.SetActive(false);
                CEOQuest3.SetActive(false);
                CEOQuest4.SetActive(true);
                CEOQuest5.SetActive(false);
            }
            if (storyIndex == 13)
            {
                CEOQuest1.SetActive(false);
                CEOQuest2.SetActive(false);
                CEOQuest3.SetActive(false);
                CEOQuest4.SetActive(false);
                CEOQuest5.SetActive(true);
            }
        }
        if(storyIndex >= 24)
        {
            sakuraTree.Play("Sakura4");
        }
    }


}
