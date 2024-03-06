using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadMenu : MenuPage
{
    private const int MAX_FILES = 99;
    //private string savePath = FilePaths

    private int currentPage = 1;
    private bool loadedFilesForFirstTime = false;

    public enum MenuFunction { save, load}
    private MenuFunction menuFunction = MenuFunction.save;

    public SaveLoadSlot[] saveSlots;

    public override void Open()
    {
        base.Open();

        if (!loadedFilesForFirstTime)
        {
            PopulateSaveSlotsForPage(currentPage);
        }
    }

    private void PopulateSaveSlotsForPage(int pageNumber)
    {

    }
}
