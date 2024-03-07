using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private Transform player;
    private Transform npc;

    private bool isNearNPC => PlayerController.isNearNPC;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Use()
    {
        if (isNearNPC)
        {
            Destroy(gameObject);
        }
    }
}
