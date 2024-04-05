using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

//keep rigidbody 2D rotation frozen

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public TextAsset diamondStore;
    public TextAsset coffeeShop;
    public static bool isNearNPC;
    public CanvasGroup cg;
    [SerializeField]public TextMeshProUGUI money;

    
    private void Start()
    {
        //transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
        isNearNPC = false;
        anim = GetComponent<Animator>();
    }

    public float moveSpeed = 5.0f;
    float horizontal;
    float vertical;

    public void Update()
    {
        Vector3 moveDirection = Vector3.up * vertical + Vector3.right * horizontal;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        //PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        //PlayerPrefs.SetFloat("PlayerY", transform.position.y);

        anim.SetFloat("Horizontal", moveDirection.x);
        anim.SetFloat("Vertical", moveDirection.y);
        anim.SetFloat("Speed", moveDirection.sqrMagnitude);
    }



    public void OnMoveInput(float horizontal, float vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        //Debug.Log($"Player Controller Move Input: {vertical}, {horizontal}");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            isNearNPC = true;
        }
        if (collision.CompareTag("DiamondStore") && StoryManager.storyIndex == 9)
        {
            CEOCollision.StartConversation(diamondStore);
            money.text = "5000";
            CEOCollision.finishedQuest = true;
        }
        if(collision.CompareTag("CoffeeShop") && StoryManager.storyIndex == 11)
        {
            CEOCollision.StartConversation(coffeeShop);
            CEOCollision.finishedQuest = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            isNearNPC = false;
        }
    }


}
    


