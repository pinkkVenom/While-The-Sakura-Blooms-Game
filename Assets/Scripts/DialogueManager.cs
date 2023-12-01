using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject contButton; 


    public TextMeshProUGUI textComponent;
    public string[] lines; 
    public float textSpeed;

    private int index;

    public bool playerIsClose; 

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDailouge(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {


                if (textComponent.text == lines[index])
                {
                    zeroText();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }

            }
            else
            {
                dialoguePanel.SetActive(true); 
            }
        }

        if(textComponent.text == lines[index])
        {
            contButton.SetActive(true);
        }

    }

    public void zeroText()
    {
        textComponent.text = ""; 
        index = 0;
        dialoguePanel.SetActive(false); 
    }


    void StartDailouge()
    {
        index = 0;
        StartCoroutine(TypeLine()); 

    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); 
        }
    }

    void NextLine()
    {
        contButton.SetActive(false); 

        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false); 
            zeroText(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
