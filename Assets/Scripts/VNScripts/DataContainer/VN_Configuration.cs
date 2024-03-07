using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//configurable from menu and user editable

[System.Serializable]
public class VN_Configuration
{
    public static VN_Configuration activeConfig { get; private set; }

    public static string filePath => $"{FilePaths.root}vnconfig.cfg";

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

    }
}
