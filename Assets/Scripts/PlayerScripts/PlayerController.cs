using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keep rigidbody 2D rotation frozen

public class PlayerController : MonoBehaviour
{
    
    private Animator anim;

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

        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isRunning", true); 
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        Debug.Log($"Player Controller Move Input: {vertical}, {horizontal}");
    }
}
