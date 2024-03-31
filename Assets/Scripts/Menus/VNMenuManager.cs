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

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

    private bool inMenu = false;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rootCG = new CanvasGroupController(this, root);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inMenu == false)
        {
            //add something to check if we are in main menu so this wont showup

            inMenu = true;

            if (activePage != null)
            {
                ClosePage(activePage);
            }

            activePage = GetPage(MenuPage.PageType.PauseScreen);
            OpenPauseMenu();
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && inMenu == true)
        {
            inMenu = false;
            ClosePage(activePage);
            Time.timeScale = 1;
        }
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

    public void ClosePage(MenuPage page)
    {
        if (page == null)
        {
            return;
        }
        page.Close();
        activePage = null;
        if (isOpen)
        {
            CloseRoot();
        }
    }

    public void OpenRoot()
    {
        Time.timeScale = 0;
        rootCG.Show();
        rootCG.SetInteractableState(true);
        isOpen = true;
    }

    public void CloseRoot()
    {
        Time.timeScale = 1;
        if (activePage == GetPage(MenuPage.PageType.PauseScreen))
        {
             activePage.Close();
            activePage = null;
        }
        rootCG.Hide();
        rootCG.SetInteractableState(false);
        isOpen = false;
    }

    public void Click_Home()
    {
        VN_Configuration.activeConfig.Save();

        uiChoiceMenu.Show("Return to Main Menu?", new UIConfirmationMenu.ConfirmationButton("Yes", () => UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenu.MAIN_MENU_SCENE)), new UIConfirmationMenu.ConfirmationButton("No", null));
    }

    public void Click_Quit()
    {
        uiChoiceMenu.Show("Quit to desktop?", new UIConfirmationMenu.ConfirmationButton("Yes", ()=> Application.Quit()), new UIConfirmationMenu.ConfirmationButton("No", null));
    }
}
