using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PortalController : MonoBehaviour
{
    public Transform destination;
    GameObject player;
    public CinemachineConfiner confinerCam;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Vector2.Distance(player.transform.position, transform.position) > 0.2f) {
                player.transform.position = destination.transform.position;
                confinerCam.m_BoundingShape2D = destination.GetComponentInParent<PolygonCollider2D>();
            }
        }
    }
}
