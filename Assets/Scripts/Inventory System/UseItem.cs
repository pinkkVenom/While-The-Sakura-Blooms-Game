using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private Transform player;
    private Transform npc;

    private bool isNearNPC = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        npc = GameObject.FindGameObjectWithTag("NPC").transform;
    }

    public void Use()
    {
        if (isNearNPC)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            isNearNPC = true;
            Use();
        }
        isNearNPC = false;
    }
}
