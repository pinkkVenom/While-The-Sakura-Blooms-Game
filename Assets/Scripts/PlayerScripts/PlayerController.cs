using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

//keep rigidbody 2D rotation frozen

public class PlayerController : MonoBehaviour
{
    private Animator anim;


    
    private void Awake()
    {

    }
    
   private void OnTriggerEnter2D(Collider2D collider)
    {
        
        
    }
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public float moveSpeed = 5.0f;
    float horizontal;
    float vertical;

    public void Update()
    {
        Vector3 moveDirection = Vector3.up * vertical + Vector3.right * horizontal;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        //Debug.Log($"Player Controller Move Input: {vertical}, {horizontal}");
    }


}
    


