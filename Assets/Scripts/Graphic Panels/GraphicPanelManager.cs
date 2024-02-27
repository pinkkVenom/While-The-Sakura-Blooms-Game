using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//manager that controls all graphic panels such as the background, cinematic, and foreground

public class GraphicPanelManager : MonoBehaviour
{
    public static GraphicPanelManager instance { get; private set; }

    //list of all available graphic panels
    [SerializeField] private GraphicPanel[] allPanels;

    //constants
    public float DEFAULT_TRANSITION_SPEED = 3f;

    private void Awake()
    {
        instance = this;
    }

    //return panel based on its name
    public GraphicPanel GetPanel(string name)
    {
        name = name.ToLower();
        foreach (var panel in allPanels)
        {
            if (panel.panelName.ToLower() == name)
            {
                return panel;
            }
        }
        //panel does not exist
        return null;
    }
}
