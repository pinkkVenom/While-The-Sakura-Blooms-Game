#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.Video;
using CHARACTERS;
using UnityEditor;
using System.IO;
using VISUALNOVEL;

public class TESTING : MonoBehaviour
{
    [SerializeField] private TextAsset filetoread = null;
    //public GameObject VN;
    // Start is called before the first frame update
    void Start()
    {
        StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
        string fullPath = AssetDatabase.GetAssetPath(filetoread);

        int resourcesIndex = fullPath.IndexOf("Resources/");
        string relativePath = fullPath.Substring(resourcesIndex + 10);

        string filePath = Path.ChangeExtension(relativePath, null);

        LoadFile(filePath);
    }

    public void LoadFile(string filePath)
    {
        List<string> lines = new List<string>();
        TextAsset file = Resources.Load<TextAsset>(filePath);

        //catch any loading errors
        try
        {
            lines = FileManager.ReadTextAsset(file);
        }
        catch
        {
            Debug.LogError($"Dialogue file at path 'Resources/{filePath}' does not exist!");
            return;
        }

        DialogueSystem.instance.Say(lines, filePath);
    }


}
#endif