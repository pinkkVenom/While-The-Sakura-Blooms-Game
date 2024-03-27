using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using VISUALNOVEL;

public class SaveLoadSlot : MonoBehaviour
{
    public GameObject root;
    public RawImage previewImage;
    public TextMeshProUGUI titleText;
    public Button loadSaveButton;
    public Button deleteButton;
    public Button saveButton;

    [HideInInspector] public int fileNumber = 0;
    [HideInInspector] public string filePath = "";

    public void PopulateDetails(SaveAndLoadMenu.MenuFunction function)
    {
        if (File.Exists(filePath))
        {
            VNGameSave file = VNGameSave.VNLoad(filePath);
            PopulateDetailsFromFile(function, file);
        }
        else
        {
            PopulateDetailsFromFile(function, null);
        }
    }

    private void PopulateDetailsFromFile(SaveAndLoadMenu.MenuFunction function, VNGameSave file)
    {
        if(file == null)
        {
            titleText.text = $"{fileNumber}. Empty File";
            deleteButton.gameObject.SetActive(false);
            loadSaveButton.gameObject.SetActive(false);
            saveButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFunction.save);
            previewImage.texture = SaveAndLoadMenu.instance.emptyFileImage;
        }
        else
        {
            titleText.text = $"{fileNumber}. {file.timeStamp}";
            deleteButton.gameObject.SetActive(true);
            loadSaveButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFunction.save);
            saveButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFunction.save);

            byte[] data = File.ReadAllBytes(file.screenshotPath);
            Texture2D screenshotPreview = new Texture2D(1, 1);
            ImageConversion.LoadImage(screenshotPreview, data);
            previewImage.texture = screenshotPreview;
        }
    }

    public void Delete()
    {
        File.Delete(filePath);
        PopulateDetails(SaveAndLoadMenu.instance.menuFunction);
    }

    public void Load()
    {
        VNGameSave file = VNGameSave.VNLoad(filePath, true);

        SaveAndLoadMenu.instance.Close(closeAllMenus: true);

    }

    public void Save()
    {
        var activeSave = VNGameSave.activeFile;
        activeSave.slotNumber = fileNumber;
        activeSave.Save();

        PopulateDetailsFromFile(SaveAndLoadMenu.instance.menuFunction, activeSave);
    }
}
