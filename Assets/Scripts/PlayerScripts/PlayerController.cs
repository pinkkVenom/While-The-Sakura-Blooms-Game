using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

//keep rigidbody 2D rotation frozen

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    public GameObject vnscene;
    bool sceneActive;

    //private Inventory inventory;

    //[SerializeField] private UI_Inventory uiInventory; 

    /*
    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        ItemWorld.SpawnItemWorld(new Vector3(-30, 18), new Item {itemType = Item.ItemType.Coin, amount = 1});
        ItemWorld.SpawnItemWorld(new Vector3(10, 18), new Item {itemType = Item.ItemType.Coin, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(32, 15), new Item { itemType = Item.ItemType.Flower, amount = 1 });
    }
    
   private void OnTriggerEnter2D(Collider2D collider)
    {
        //ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        //if ((itemWorld != null) )
        //{
            // if the player is touching item 
            //inventory.AddItem(ItemWorld.GetItem());
          //  itemWorld.DestroySelf(); 
        }
    }
    */
    private void Start()
    {
        anim = GetComponent<Animator>();
       // vnscene.SetActive(false);
        sceneActive = true;
    }

    public float moveSpeed = 5.0f;
    float horizontal;
    float vertical;

    public void Update()
    {
        Vector3 moveDirection = Vector3.up * vertical + Vector3.right * horizontal;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        //if (moveDirection != Vector3.zero)
        //{
        //    anim.SetBool("isRunning", true); 
        //}
        //else
        //{
         //   anim.SetBool("isRunning", false);
        //}
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            PromptAdvance();
        }

        if (Input.GetKeyDown(KeyCode.E) && sceneActive == false)
        {
            vnscene.SetActive(true);
            sceneActive = true;
        }
        else if(Input.GetKeyDown(KeyCode.E) && sceneActive == true)
        {
            vnscene.SetActive(false);
            sceneActive = false;
        }

    }

    public void PromptAdvance()
    {
        DialogueSystem.instance.OnUserPrompt_Next();
        //Debug.Log("Success");
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        Debug.Log($"Player Controller Move Input: {vertical}, {horizontal}");
    }

}
