using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using VISUALNOVEL;
using HISTORY;

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

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

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
        else if (function == SaveAndLoadMenu.MenuFunction.save)
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
        else if(function == SaveAndLoadMenu.MenuFunction.load)
        {
            titleText.text = $"{fileNumber}. {file.timeStamp}";
            deleteButton.gameObject.SetActive(true);
            loadSaveButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFunction.load);
            saveButton.gameObject.SetActive(false);

            byte[] data = File.ReadAllBytes(file.screenshotPath);
            Texture2D screenshotPreview = new Texture2D(1, 1);
            ImageConversion.LoadImage(screenshotPreview, data);
            previewImage.texture = screenshotPreview;
        }
    }

    public void Delete()
    {
        uiChoiceMenu.Show(
            //Title
            "Delete this file (This cannot be undone!)",
            //Choice 1
            new UIConfirmationMenu.ConfirmationButton("Yes", () =>
                {
                    uiChoiceMenu.Show("Are you sure?",
                        //choice 1
                        new UIConfirmationMenu.ConfirmationButton("Yes", OnConfirmDelete),
                        //choice 2
                        new UIConfirmationMenu.ConfirmationButton("No", null));
                },
                autoCloseOnClick: false
            ),
            //Choice 2
            new UIConfirmationMenu.ConfirmationButton("No", null)); ;
    }

    private void OnConfirmDelete()
    {
        File.Delete(filePath);
        PopulateDetails(SaveAndLoadMenu.instance.menuFunction);
    }

    public void Load()
    {
        VNGameSave file = VNGameSave.VNLoad(filePath, false);
        SaveAndLoadMenu.instance.Close(closeAllMenus: true);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == MainMenu.MAIN_MENU_SCENE)
        {
            MainMenu.instance.LoadGame(file);
        }
        else
        {
            file.Activate();
        }

    }

    public void Save()
    {
        if (HistoryManager.instance.isViewingHistory)
        {
            UIConfirmationMenu.instance.Show("Cannot save while looking at history", new UIConfirmationMenu.ConfirmationButton("OK", null));
            return;
        }

        var activeSave = VNGameSave.activeFile;
        activeSave.slotNumber = fileNumber;
        activeSave.Save();

        PopulateDetailsFromFile(SaveAndLoadMenu.instance.menuFunction, activeSave);
    }
}
