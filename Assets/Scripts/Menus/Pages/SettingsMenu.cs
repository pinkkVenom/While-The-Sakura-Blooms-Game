using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.IO;
using DIALOGUE;
using UnityEngine.Audio;

public class SettingsMenu : MenuPage
{
    public static SettingsMenu instance { get; private set; }

    [SerializeField] private GameObject[] panels;
    private GameObject activePanel;

    public UI_ITEMS ui;
    private VN_Configuration config => VN_Configuration.activeConfig;

    private void Awake()
    {
        instance = this;
    }

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
            VN_Configuration.activeConfig = FileManager.Load<VN_Configuration>(VN_Configuration.filePath, encrypt: VN_Configuration.ENCRYPT);
        }
        else
        {
            VN_Configuration.activeConfig = new VN_Configuration();
        }

        VN_Configuration.activeConfig.Load();
    }

    private void OnApplicationQuit()
    {
        VN_Configuration.activeConfig.Save();
        VN_Configuration.activeConfig = null;
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
        //Resolution[] resolutions = Screen.resolutions;
        Resolution[] resolutions = Screen.resolutions;
        List<string> options = new List<string>();

        for(int i = resolutions.Length -1; i >= 0; i--)
        {
            if (resolutions[i].width != 1000) 
            {
                options.Add($"{resolutions[i].width}x{resolutions[i].height}");
            }
        }

        ui.resolutions.ClearOptions();
        ui.resolutions.AddOptions(options);
    }

    [System.Serializable]
    public class UI_ITEMS 
    {
        private static Color button_selectedColor = new Color(0.4f, 0.1f, 0.25f, 1f);
        private static Color button_unselectedColor = new Color(1f, 1f, 1f, 1f);
        private static Color text_selectedColor = new Color(1f, 1f, 1f, 1f);
        private static Color text_unselectedColor = new Color(0.1f, 0.1f, 0.1f, 1f);

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

        public void SetButtonColors(Button A, Button B, bool selectedA)
        {
            A.GetComponent<Image>().color = selectedA ? button_selectedColor : button_unselectedColor;
            B.GetComponent<Image>().color = !selectedA ? button_selectedColor : button_unselectedColor;

            A.GetComponentInChildren<TextMeshProUGUI>().color = selectedA ? text_selectedColor : text_unselectedColor;
            B.GetComponentInChildren<TextMeshProUGUI>().color = !selectedA ? text_selectedColor : text_unselectedColor;
        }
    }

    //UI CALLABLE FUNCTIONS
    public void SetDisplayToFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        ui.SetButtonColors(ui.fullscreen, ui.windowed, fullscreen);
    }

    public void SetDisplayResolution()
    {
        string resolution = ui.resolutions.captionText.text;
        string[] values = resolution.Split('x');

        if(int.TryParse(values[0], out int width) && int.TryParse(values[1], out int height))
        {
            //we have valid width and height configuration
            Screen.SetResolution(width, height, Screen.fullScreen);
            config.display_resolution = resolution;
        }
        else
        {
            Debug.LogError($"Parsing error for screen resolution! [{resolution}] could not be pased into widthXheight");
        }
    }

    public void SetContinueSkippingAfterChoice(bool continueSkipping)
    {
        config.continueSkippingAfterChoice = continueSkipping;
        ui.SetButtonColors(ui.skippingContinue, ui.skippingStop, continueSkipping);
    }

    public void SetTextArchitectSpeed()
    {
        config.dialogueTextSpeed = ui.textSpeed.value;
        if(DialogueSystem.instance != null)
            DialogueSystem.instance.conversationManager.textManager.speed = config.dialogueTextSpeed;
    }

    public void SetAutoReaderSpeed()
    {
        config.dialogueAutoReadSpeed = ui.autoReadSpeed.value;

        if(DialogueSystem.instance == null)
        {
            return;
        }

        AutoReader autoReader = DialogueSystem.instance.autoReader;
        if(autoReader != null)
        {
            autoReader.speed = config.dialogueAutoReadSpeed;
        }
    }

    public void SetMusicVolume()
    {
        config.musicVolume = ui.musicVolume.value;
        AudioMixerGroup mixer = AudioManager.instance.musicMixer;
        AnimationCurve audioFalloff = AudioManager.instance.audioFalloffCurve;

        mixer.audioMixer.SetFloat(AudioManager.MUSIC_VOLUME_PARAM_NAME, audioFalloff.Evaluate(config.musicVolume));
    }

    public void SetSFXVolume()
    {
        config.sfxVolume = ui.sfxVolume.value;
        AudioMixerGroup mixer = AudioManager.instance.sfxMixer;
        AnimationCurve audioFalloff = AudioManager.instance.audioFalloffCurve;

        mixer.audioMixer.SetFloat(AudioManager.SFX_VOLUME_PARAM_NAME, audioFalloff.Evaluate(config.sfxVolume));
    }

    public void SetVoicesVolume()
    {
        config.voicesVolume = ui.voicesVolume.value;
        AudioMixerGroup mixer = AudioManager.instance.voicesMixer;
        AnimationCurve audioFalloff = AudioManager.instance.audioFalloffCurve;

        mixer.audioMixer.SetFloat(AudioManager.VOICES_VOLUME_PARAM_NAME, audioFalloff.Evaluate(config.voicesVolume));
    }

}
