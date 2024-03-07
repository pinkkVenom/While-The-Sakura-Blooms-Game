using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Portal : MonoBehaviour
{
    private Transform destination;

    public bool isRed;
    public float distance = 0.2f;

    [SerializeField] Collider2D confinerRed;
    [SerializeField] Collider2D confinerBlue;

    bool artCamActive = true;
    bool hospitalCamActive = false;

    [SerializeField] public CinemachineVirtualCamera camera;
    [SerializeField] public CinemachineConfiner confiner;


    // Start is called before the first frame update
    void Start()
    {
        if(isRed == false)
        {
            destination = GameObject.FindGameObjectWithTag("Red Portal").GetComponent<Transform>();
        }
        else
        {
            destination = GameObject.FindGameObjectWithTag("Blue Portal").GetComponent<Transform>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Vector2.Distance(transform.position, collision.transform.position) > distance)
        {
            if (confiner.m_BoundingShape2D == confinerRed)
            {
                confiner.m_BoundingShape2D = confinerBlue;
                collision.transform.position = new Vector2(destination.position.x, destination.position.y);
                return;
            }
            else if (confiner.m_BoundingShape2D = confinerBlue)
            {
                confiner.m_BoundingShape2D = confinerRed;
                collision.transform.position = new Vector2(destination.position.x, destination.position.y);
                return;
            }
            
            
        }
    }
}
