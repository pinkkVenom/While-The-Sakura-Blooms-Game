using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private bool isNearNPC => PlayerController.isNearNPC;
    string itemname;
    private const string CLONE_NAME = "(Clone)";

    private void Start()
    {
        itemname = this.name;
    }

    //do cases for each item type and each NPC
    public void Use()
    {
        if (CEOCollision.inRange == true && itemname == "CoffeeButton"+CLONE_NAME && StoryManager.storyIndex == 10)
        {
            Debug.Log($"its the coffee {itemname}");
            CEOCollision.finishedQuest = true;
            Destroy(gameObject);
        }
        else if(CEOCollision.inRange == true && itemname == "CloakButton"+CLONE_NAME && StoryManager.storyIndex == 12)
        {
            Debug.Log($"its the cloak {itemname}");
            CEOCollision.finishedQuest = true;
            Destroy(gameObject);
        }
        else if(CEOCollision.inRange == true && itemname == "RingButton"+CLONE_NAME && StoryManager.storyIndex == 13)
        {
            Debug.Log($"its the heirloom ring {itemname}");
            CEOCollision.finishedQuest = true;
            Destroy(gameObject);
        }
        else if (ArtistCollision.inRange == true && itemname == "RoseButton" + CLONE_NAME && StoryManager.storyIndex == 14)
        {
            Debug.Log($"its the rose {itemname}");
            ArtistCollision.finishedQuest = true;
            Destroy(gameObject);
        }
        else if (ArtistCollision.inRange == true && itemname == "BrushButton" + CLONE_NAME && StoryManager.storyIndex == 16)
        {
            Debug.Log($"its the brush {itemname}");
            ArtistCollision.finishedQuest = true;
            Destroy(gameObject);
        }
        else if (ArtistCollision.inRange == true && itemname == "MoonstoneButton" + CLONE_NAME && StoryManager.storyIndex == 18)
        {
            Debug.Log($"its the moonstone {itemname}");
            ArtistCollision.finishedQuest = true;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"ewwww");
        }
    }
}
