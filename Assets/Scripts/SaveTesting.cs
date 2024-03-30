#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VISUALNOVEL;

public class SaveTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VNGameSave.activeFile = new VNGameSave();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            VNGameSave.activeFile.Save();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            VNGameSave.VNLoad($"{FilePaths.gameSaves}1{VNGameSave.FILE_TYPE}", activateOnLoad: true);
        }
    }
}
#endif