using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Quest : MonoBehaviour
{
    public Image questItem;
    public Color completedColor;
    public Color activeColor;
    public Color currentColor;

    //public QuestArrow arrow;

    public Quest[] allQuests; 

    public void Start()
    {
        allQuests = FindObjectsOfType<Quest>();
        currentColor = questItem.color; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { 
            FinishQuest();  
        }
        
    }

    void FinishQuest()
    {
        currentColor = completedColor; 
        questItem.color = completedColor; 
    }
    
    public void onQuestClick()
    {

        foreach (Quest quest in allQuests)
        {
            quest.questItem.color = quest.currentColor;
                    }
        questItem.color = activeColor; 
    }


}
