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
    public bool chosenLibrarian => LibrarianCollision.chosenRomance;
    public bool chosenArtist => ArtistCollision.chosenRomance;

    //End Game
    [SerializeField] GameObject factionbad;

    //CEO Questing
    public GameObject CEOQuest1;
    public GameObject CEOQuest2;
    public GameObject CEOQuest3;
    public GameObject CEOQuest4;
    public GameObject CEOQuest5;

    //Artist Questing
    public GameObject artistQuest1;
    public GameObject artistQuest2;
    public GameObject artistQuest3;
    public GameObject artistQuest4;
    public GameObject artistQuest5;

    //Librarian Questing
    public GameObject libQuest1;
    public GameObject libQuest2;
    public GameObject libQuest3;
    public GameObject libQuest4;

    //Nurse Questing
    public GameObject nurseQuest1;
    public GameObject nurseQuest2;
    public GameObject nurseQuest3;
    public GameObject nurseQuest4;

    void Start()
    {
        doctor.SetActive(false);
        factionbad.SetActive(false);
        factionGood.SetActive(false);

        CEOQuest1.SetActive(false);
        CEOQuest2.SetActive(false);
        CEOQuest3.SetActive(false);
        CEOQuest4.SetActive(false);
        CEOQuest5.SetActive(false);
        artistQuest1.SetActive(false);
        artistQuest2.SetActive(false);
        artistQuest3.SetActive(false);
        artistQuest4.SetActive(false);
        artistQuest5.SetActive(false);
        libQuest1.SetActive(false);
        libQuest2.SetActive(false);
        libQuest3.SetActive(false);
        libQuest4.SetActive(false);
        nurseQuest1.SetActive(false);
        nurseQuest2.SetActive(false);
        nurseQuest3.SetActive(false);
        nurseQuest4.SetActive(false);

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
            if (chosenArtist == true && storyIndex == 3)
            {
                storyIndex = 14;
                SetStoryIndex();
            }
            if (chosenLibrarian == true && storyIndex == 3)
            {
                storyIndex = 19;
                SetStoryIndex();
            }
            if (chosenNurse == true && storyIndex == 3)
            {
                storyIndex = 4;
                SetStoryIndex();
            }
            //Quest 4: Pick a single Immortal to romance
            //romancing happens in individual characters scripts
            //4, 5, 6, 7, 8 are nurse
            if (NurseCollision.quest1Done == true && storyIndex == 4)
            {
                storyIndex = 5;
                SetStoryIndex();
            }
            if (NurseCollision.quest2Done == true && storyIndex == 5)
            {
                storyIndex = 6;
                SetStoryIndex();
            }
            if (NurseCollision.quest3Done == true && storyIndex == 6)
            {
                storyIndex = 7;
                SetStoryIndex();
            }
            if (NurseCollision.quest4Done == true && storyIndex == 7)
            {
                storyIndex = 25;
                SetStoryIndex();
            }
            //9, 10, 11, 12, 13 are CEO
            if (CEOCollision.quest1Done == true && storyIndex == 9)
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
            if (ArtistCollision.quest1Done == true && storyIndex == 14)
            {
                storyIndex = 15;
                SetStoryIndex();
            }
            if (ArtistCollision.quest2Done == true && storyIndex == 15)
            {
                storyIndex = 16;
                SetStoryIndex();
            }
            if (ArtistCollision.quest3Done == true && storyIndex == 16)
            {
                storyIndex = 17;
                SetStoryIndex();
            }
            if (ArtistCollision.quest4Done == true && storyIndex == 17)
            {
                storyIndex = 18;
                SetStoryIndex();
            }
            if (ArtistCollision.quest5Done == true && storyIndex == 18)
            {
                storyIndex = 27;
                SetStoryIndex();
            }
            //19, 20, 21, 22, 23 are Librarian
            if (LibrarianCollision.quest1Done == true && storyIndex == 19)
            {
                storyIndex = 20;
                SetStoryIndex();
            }
            if (LibrarianCollision.quest2Done == true && storyIndex == 20)
            {
                storyIndex = 21;
                SetStoryIndex();
            }
            if (LibrarianCollision.quest3Done == true && storyIndex == 21)
            {
                storyIndex = 22;
                SetStoryIndex();
            }
            if (LibrarianCollision.quest4Done == true && storyIndex == 22)
            {
                storyIndex = 26;
                SetStoryIndex();
            }
            //24 is CEO ending
            if (storyIndex == 24)
            {
                SetStoryIndex();
            }
            //25 is Nurse ending
            if (storyIndex == 25)
            {
                SetStoryIndex();
            }
            //26 is Librarian ending
            if (storyIndex == 26)
            {
                SetStoryIndex();
            }
            //27 is Artist ending
            if (storyIndex == 27)
            {
                SetStoryIndex();
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
            //NURSE
            if (storyIndex == 4)
            {
                nurseQuest1.SetActive(true);
                nurseQuest2.SetActive(false);
                nurseQuest3.SetActive(false);
                nurseQuest4.SetActive(false);
            }
            if (storyIndex == 5)
            {
                nurseQuest1.SetActive(false);
                nurseQuest2.SetActive(true);
                nurseQuest3.SetActive(false);
                nurseQuest4.SetActive(false);
            }
            if (storyIndex == 6)
            {
                nurseQuest1.SetActive(false);
                nurseQuest2.SetActive(false);
                nurseQuest3.SetActive(true);
                nurseQuest4.SetActive(false);
            }
            if (storyIndex == 7)
            {
                nurseQuest1.SetActive(false);
                nurseQuest2.SetActive(false);
                nurseQuest3.SetActive(false);
                nurseQuest4.SetActive(true);
            }
            //CEO
            if (storyIndex == 9)
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
            //ARTIST
            if (storyIndex == 14)
            {
                artistQuest1.SetActive(true);
                artistQuest2.SetActive(false);
                artistQuest3.SetActive(false);
                artistQuest4.SetActive(false);
                artistQuest5.SetActive(false);
            }
            if (storyIndex == 15)
            {
                artistQuest1.SetActive(false);
                artistQuest2.SetActive(true);
                artistQuest3.SetActive(false);
                artistQuest4.SetActive(false);
                artistQuest5.SetActive(false);
            }
            if (storyIndex == 16)
            {
                artistQuest1.SetActive(false);
                artistQuest2.SetActive(false);
                artistQuest3.SetActive(true);
                artistQuest4.SetActive(false);
                artistQuest5.SetActive(false);
            }
            if (storyIndex == 17)
            {
                artistQuest1.SetActive(false);
                artistQuest2.SetActive(false);
                artistQuest3.SetActive(false);
                artistQuest4.SetActive(true);
                artistQuest5.SetActive(false);
            }
            if (storyIndex == 18)
            {
                artistQuest1.SetActive(false);
                artistQuest2.SetActive(false);
                artistQuest3.SetActive(false);
                artistQuest4.SetActive(false);
                artistQuest5.SetActive(true);
            }
            //LIBRARIAN
            if (storyIndex == 19)
            {
                libQuest1.SetActive(true);
                libQuest2.SetActive(false);
                libQuest3.SetActive(false);
                libQuest4.SetActive(false);
            }
            if (storyIndex == 20)
            {
                libQuest1.SetActive(false);
                libQuest2.SetActive(true);
                libQuest3.SetActive(false);
                libQuest4.SetActive(false);
            }
            if (storyIndex == 21)
            {
                libQuest1.SetActive(false);
                libQuest2.SetActive(false);
                libQuest3.SetActive(true);
                libQuest4.SetActive(false);
            }
            if (storyIndex == 22)
            {
                libQuest1.SetActive(false);
                libQuest2.SetActive(false);
                libQuest3.SetActive(false);
                libQuest4.SetActive(true);
            }
        }
        if(storyIndex >= 24)
        {
            sakuraTree.Play("Sakura4");
            doctor.SetActive(true);
            factionGood.SetActive(false);

            factionbad.SetActive(true);
        }
    }


}
