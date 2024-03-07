using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject item;
    private Transform player;
    private Transform world;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        world = GameObject.FindGameObjectWithTag("World").transform;
    }
    public void SpawnDroppedItem()
    {
        Vector2 playerpos = new Vector2(player.position.x, player.position.y + 1.5f);
        item = Instantiate(item, playerpos, Quaternion.identity);
        item.transform.SetParent(world);
    }
}
