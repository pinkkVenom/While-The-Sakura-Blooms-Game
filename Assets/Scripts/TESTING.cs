using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.Video;
using CHARACTERS;

public class TESTING : MonoBehaviour
{
    [SerializeField] private TextAsset filetoread = null;
    //public GameObject VN;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Running());
        StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
       List<string> lines = FileManager.ReadTextAsset(filetoread);
        
       DialogueSystem.instance.Say(lines);
    }
    bool hasSpoken = true;

    IEnumerator Running()
    {
        Character Hanako = CharacterManager.instance.CreateCharacter("Hanako");
        Hanako.Show();
        List<string> lines = new List<string>()
        {
            "hello",
            "im hanako"
        };

        yield return Hanako.Say(lines);

        Hanako.UnHighlight();
        //GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        //GraphicLayer layer = panel.GetLayer(0, true);

        //yield return new WaitForSeconds(1);

        //Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");
        ////layer.SetTexture("Graphics/BG Images/2", blendingTexture: blendTex);
        //layer.SetVideo("Graphics/BG Videos/Fantasy Landscape");

        yield return new WaitForSeconds(1);

        //layer.currentGraphic.FadeOut();
    }
}
