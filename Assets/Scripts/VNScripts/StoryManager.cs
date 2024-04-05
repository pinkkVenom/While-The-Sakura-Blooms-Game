using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class StoryManager : MonoBehaviour
{
    public static int storyIndex = 4;
    [SerializeField] CanvasGroup VN;
    [SerializeField] Animator sakuraTree;
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
    public bool chosenCEO;
    public bool chosenLibrarian;
    public bool chosenArtist;

    void Start()
    {
        doctor.SetActive(false);
        factionGood.SetActive(false);
        SetStoryIndex();
    }

    // Update is called once per frame
    void Update()
    {
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
            //Quest 4: Pick a single Immortal to romance
            //romancing happens in individual characters scripts
            //4, 5, 6, 7, 8 are nurse
            //9, 10, 11, 12, 13 are CEO
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
        }
        if(storyIndex >= 24)
        {
            sakuraTree.Play("Sakura4");
        }
    }


}
