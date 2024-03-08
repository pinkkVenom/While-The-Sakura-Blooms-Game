using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

public class InputPanelTESTING : MonoBehaviour
{
    //public InputPanel inputPanel;
    ChoicePanel panel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }

    IEnumerator Running()
    {
        List<string> lines = new List<string>()
        {
            "this is a line",
            "this is another line",
            "wow, another line smh"
        };
        yield return DialogueSystem.instance.Say(lines);

        DialogueSystem.instance.Hide();
    }

    private void Update()
    {
        List<string> lines = new List<string>();
        Conversation conversation = null;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lines = new List<string>(){
                "this is the start of enqueue conversation",
                "lmao cool" 
            };
            conversation = new Conversation(lines);
            DialogueSystem.instance.conversationManager.Enqueue(conversation);

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            lines = new List<string>(){
                "yet another queue",
                "lmao cool x2"
            };
            conversation = new Conversation(lines);
            DialogueSystem.instance.conversationManager.EnqueuePriority(conversation);
        }
    }
}
