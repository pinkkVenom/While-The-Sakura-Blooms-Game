using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SettingsMenu : MenuPage
{
    [SerializeField] private GameObject[] panels;
    private GameObject activePanel;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
        activePanel = panels[0];
    }

    public void OpenPanel(string panelName)
    {
        GameObject panel = panels.First(p => p.name.ToLower() == panelName.ToLower());

        if(panel == null)
        {
            Debug.LogError($"Did not find panel called '{panelName}' in config menu");
            return;
        }

        if(activePanel != null && activePanel != panel)
        {
            activePanel.SetActive(false);
        }
        panel.SetActive(true);
        activePanel = panel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
