using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private bool isNearNPC => PlayerController.isNearNPC;
    string itemname;
    private const string CLONE_NAME = "(Clone)";

    private void Start()
    {
        itemname = this.name;
    }

    //do cases for each item type and each NPC
    public void Use()
    {
        if (isNearNPC == true && itemname == "BluebellButton"+CLONE_NAME)
        {
            Debug.Log($"its the bluebell {itemname}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"ewwww");
        }
    }
}
