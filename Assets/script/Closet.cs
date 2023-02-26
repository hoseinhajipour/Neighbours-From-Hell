using UnityEngine;

public class Closet : MonoBehaviour
{
    public bool isOccupied = false;
    public GameObject textInfo;
    private Collider player;

    private void Update()
    {
        if (player)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("press E");
                if (player.CompareTag("Player"))
                {
                    if (!isOccupied)
                    {
                        // Hide the player in the closet
                        player.gameObject.SetActive(false);
                        isOccupied = true;
                        Debug.Log("Player is now hiding in the closet.");
                    }
                    else
                    {
                        player.gameObject.SetActive(true);
                        isOccupied = false;
                        Debug.Log("Closet is already occupied.");
                    }
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

            this.player = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textInfo.SetActive(false);
            this.player = null;
        }
    }
}