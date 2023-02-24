using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;

    public bool freeze = false;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }



    void Update()
    {
        if (freeze == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * moveSpeed;

            controller.Move(movement * Time.deltaTime);
        }

    }
}
