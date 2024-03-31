using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalPage : MonoBehaviour
{
    [SerializeField] private Animator charTabAnim;
    [SerializeField] private Animator mapTabAnim;
    [SerializeField] private Animator journalAnim;

    //Character Tab
    [SerializeField] private CanvasGroup Hanako;
    [SerializeField] private CanvasGroup Emi;
    [SerializeField] private CanvasGroup Kenji;
    [SerializeField] private CanvasGroup Yuki;

    [SerializeField] private CanvasGroup Characters;

    //Map Tab
    [SerializeField] private CanvasGroup Map;

    private bool journalOpen;

    // Start is called before the first frame update
    void Start()
    {
        journalOpen = false;
        journalAnim.SetTrigger("Closed");
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && journalOpen == false)
        {
            journalAnim.SetTrigger("Open");
            journalOpen = true;
        }
        else if(Input.GetKeyDown(KeyCode.J) && journalOpen == true)
        {
            journalAnim.SetTrigger("Close");
            journalOpen = false;
        }
    }

    public void OpenHanako()
    {
        Close();
        Hanako.alpha = 1;
    }
    public void OpenEmi()
    {
        Close();
        Emi.alpha = 1;
    }
    public void OpenYuki()
    {
        Close();
        Yuki.alpha = 1;
    }
    public void OpenKenji()
    {
        Close();
        Kenji.alpha = 1;
    }

    private void Close()
    {
        Hanako.alpha = 0;
        Emi.alpha = 0;
        Kenji.alpha = 0;
        Yuki.alpha = 0;
    }

    public void OpenCharPage()
    {
        mapTabAnim.Play("Map_Close");
        Map.alpha = 0;
        Map.interactable = false;
        Map.blocksRaycasts = false;

        Characters.alpha = 1;
        Characters.interactable = true;
        Characters.blocksRaycasts = true;

        charTabAnim.Play("Char_Open");
    }

    public void OpenMapPage()
    {
        charTabAnim.Play("Char_Close");
        Characters.alpha = 0;
        Characters.interactable = false;
        Characters.blocksRaycasts = false;

        Map.alpha = 1;
        Map.interactable = true;
        Map.blocksRaycasts = true;
        mapTabAnim.Play("Map_Open");
    }


}
