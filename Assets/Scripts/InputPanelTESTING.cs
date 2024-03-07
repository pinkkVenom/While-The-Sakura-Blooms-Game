using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

public class InputPanelTESTING : MonoBehaviour
{
    //public InputPanel inputPanel;
    ChoicePanel panel;
    // Start is called before the first frame update
    void Start()
    {
        panel = ChoicePanel.instance;
        //StartCoroutine(Running());

        string[] choices = new string[]
        {
            "thats funny",
            "you are so insane did you know that?you are so insane did you know that?"
        };

        panel.Show(choices);
    }

    // Update is called once per frame
    //IEnumerator Running()
    //{
       
    //}
}
