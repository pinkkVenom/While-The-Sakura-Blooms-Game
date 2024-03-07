using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SettingsMenu : MenuPage
{
    [SerializeField] private GameObject[] panels;
    private GameObject activePanel;

    [SerializeField] private UI_ITEMS ui;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
        activePanel = panels[0];

        SetAvailableResolutions();
        LoadConfig();
    }

    private void LoadConfig()
    {
        if (File.Exists(VN_Configuration.filePath))
        {
            //VN_Configuration.activeConfig = FileManager.Load<VN_Configuration>(VN_Configuration.filePath);
        }
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

    private void SetAvailableResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<string> options = new List<string>();

        for(int i = resolutions.Length -1; i >= 0; i--)
        {
            options.Add($"{resolutions[i].width}x{resolutions[i].height}");
        }

        ui.resolutions.ClearOptions();
        ui.resolutions.AddOptions(options);
    }

    [System.Serializable]
    public class UI_ITEMS 
    {
        [Header("General")]
        public Button fullscreen;
        public Button windowed;
        public TMP_Dropdown resolutions;
        public Button skippingContinue, skippingStop;
        public Slider textSpeed;
        public Slider autoReadSpeed;


        [Header("Audio")]
        public Slider musicVolume;
        public Slider sfxVolume;
        public Slider voicesVolume;

    }

}
