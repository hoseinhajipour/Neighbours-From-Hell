using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destination;
    public GameObject textInfo;
    private Collider other;

    private void Update()
    {
        if (other)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("press E");
                if (other.CompareTag("Player"))
                {
                    moveto(other);
                    this.other = null;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInfo.SetActive(true);

            
            Vector3 direction = Camera.main.transform.position - textInfo.transform.position;

            // Calculate the rotation that points in the opposite direction
            Quaternion oppositeRotation = Quaternion.LookRotation(-direction, Vector3.up);
            // Rotate the textInfo to face away from the camera and towards the player
            textInfo.transform.rotation = oppositeRotation;
            
            this.other = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInfo.SetActive(false);
            this.other = null;
        }
    }


    void moveto(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        controller.enabled = false;
        other.transform.position = destination.position;
        controller.enabled = true;
        textInfo.SetActive(false);
    }
}