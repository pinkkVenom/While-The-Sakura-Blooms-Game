using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TESTING : MonoBehaviour
{
    public GameObject VN;
    // Start is called before the first frame update
    void Start()
    {
            StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("testfile");
        DialogueSystem.instance.Say(lines);
    }
}
