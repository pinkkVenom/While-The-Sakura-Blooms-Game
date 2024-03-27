using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VNMenuManager : MonoBehaviour
{
    public static VNMenuManager instance;

    private MenuPage activePage = null;
    private bool isOpen = false;

    [SerializeField] private CanvasGroup root;
    [SerializeField] private MenuPage[] pages;

    private CanvasGroupController rootCG;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rootCG = new CanvasGroupController(this, root);
    }

    private MenuPage GetPage(MenuPage.PageType pageType)
    {
        return pages.FirstOrDefault(page => page.pageType == pageType);
    }


    public void OpenSavePage()
    {
        var page = GetPage(MenuPage.PageType.SaveAndLoad);
        var slm = page.anim.GetComponentInParent<SaveAndLoadMenu>();
        slm.menuFunction = SaveAndLoadMenu.MenuFunction.save;
        OpenPage(page);
    }

    public void OpenLoadPage()
    {
        var page = GetPage(MenuPage.PageType.SaveAndLoad);
        var slm = page.anim.GetComponentInParent<SaveAndLoadMenu>();
        slm.menuFunction = SaveAndLoadMenu.MenuFunction.load;
        OpenPage(page);

    }

    public void OpenSettingsPage()
    {
        var page = GetPage(MenuPage.PageType.Settings);
        OpenPage(page);
    }

    public void OpenInstructionsPage()
    {
        var page = GetPage(MenuPage.PageType.Instructions);
        OpenPage(page);
    }

    public void OpenPauseMenu()
    {
        var page = GetPage(MenuPage.PageType.PauseScreen);
        OpenPage(page);
    }

    private void OpenPage(MenuPage page)
    {
        if (page == null)
        {
            return;
        }
        if (activePage != null && activePage != page)
        {
            activePage.Close();
        }
        page.Open();
        activePage = page;
        if (!isOpen)
        {
            OpenRoot();
        }
    }

    public void OpenRoot()
    {
        rootCG.Show();
        rootCG.SetInteractableState(true);
        isOpen = true;
        DayNightCycle.SetTick(0.0f);
    }

    public void CloseRoot()
    {
        if (activePage == GetPage(MenuPage.PageType.PauseScreen))
        {
             activePage.Close();
            activePage = null;
        }
        rootCG.Hide();
        rootCG.SetInteractableState(false);
        isOpen = false;
        DayNightCycle.SetTick(50.0f);
    }
}
