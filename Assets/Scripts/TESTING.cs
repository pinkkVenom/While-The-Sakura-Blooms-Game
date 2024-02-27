using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.Video;

public class TESTING : MonoBehaviour
{
    public GameObject VN;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
        StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("testfile");
        DialogueSystem.instance.Say(lines);
    }

    IEnumerator Running()
    {
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0, true);

        yield return new WaitForSeconds(1);

        Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");
        //layer.SetTexture("Graphics/BG Images/2", blendingTexture: blendTex);
        layer.SetVideo("Graphics/BG Videos/Fantasy Landscape");

        yield return new WaitForSeconds(1);

        layer.currentGraphic.FadeOut();
    }
}
