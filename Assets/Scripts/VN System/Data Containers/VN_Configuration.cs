using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//configurable from menu and user editable

[System.Serializable]
public class VN_Configuration
{
    public static VN_Configuration activeConfig;

    public static string filePath => $"{FilePaths.root}vnconfig.cfg";

    public const bool ENCRYPT = false;

    //General Settings
    public bool display_fullscreen = true;
    public string display_resolution = "1920x1080";
    public bool continueSkippingAfterChoice = false;
    public float dialogueTextSpeed = 1f;
    public float dialogueAutoReadSpeed = 1f;

    //Audio Settings
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float voicesVolume = 1f;

    //Other Settings
    public float historyLogScale = 1f;

    public void Load()
    {
        var UI = SettingsMenu.instance.ui;

        //GENERAL SETTINGS
        //apply window size
        SettingsMenu.instance.SetDisplayToFullscreen(display_fullscreen);
        UI.SetButtonColors(UI.fullscreen, UI.windowed, display_fullscreen);

        //apply screen resolution
        int resIndex = 0;
        for (int i = 0; i < UI.resolutions.options.Count; i++)
        {
            string resolution = UI.resolutions.options[i].text;
            if(resolution == display_resolution)
            {
                resIndex = i;
                break;
            }
        }

        UI.resolutions.value = resIndex;

        //apply continue after skipping option
        UI.SetButtonColors(UI.skippingContinue, UI.skippingStop, continueSkippingAfterChoice);

        //apply value of architect and auto reader speed
        UI.textSpeed.value = dialogueTextSpeed;
        UI.autoReadSpeed.value = dialogueAutoReadSpeed;

        //apply audio mixer volumes
        UI.musicVolume.value = musicVolume;
        UI.sfxVolume.value = sfxVolume;
        UI.voicesVolume.value = voicesVolume;
    }

    public void Save()
    {
        FileManager.Save(filePath, JsonUtility.ToJson(this), encrypt: ENCRYPT);
    }
}
