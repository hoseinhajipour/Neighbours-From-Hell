using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    public bool freeze = false;

    public Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (freeze == false)
        {
            float moveInput = Input.GetAxis("Horizontal");
            if (moveInput != 0)
            {
                animator.SetBool("isWalking", true);

                if (moveInput < 0)
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (moveInput > 0)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                moveDirection = new Vector3(moveInput, 0f, 0f);
                moveDirection *= speed;
            }
            else
            {
                animator.SetBool("isWalking", false);
                moveDirection = Vector3.zero;
            }

            moveDirection.y -= gravity;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    public void freezePlayer()
    {
        animator.SetBool("isWalking", false);
        freeze = true;
        
    }
}