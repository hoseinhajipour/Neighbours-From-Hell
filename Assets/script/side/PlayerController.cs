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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (freeze == false)
        {
            float moveInput = Input.GetAxis("Horizontal");

            moveDirection = new Vector3(moveInput, 0f, 0f);
            moveDirection *= speed;

            moveDirection.y -= gravity;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}