using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class CEOCollision : MonoBehaviour
{
    [SerializeField] private TextAsset NurseFile = null;
    [SerializeField] private TextAsset NurseFile2 = null;
    bool hasSpoken;
    // Start is called before the first frame update
    void Start()
    {
        hasSpoken = false;
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
            if (hasSpoken == false)
            {
                StartConversation(NurseFile);
                hasSpoken = true;
                return;
            }
            else
            {
                StartConversation(NurseFile2);
            }

        }

    }
}
