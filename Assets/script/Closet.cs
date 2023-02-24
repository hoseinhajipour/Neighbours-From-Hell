using UnityEngine;

public class Closet : MonoBehaviour
{
    public bool isOccupied = false;

    public void Hide(GameObject player)
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
            Debug.Log("Closet is already occupied.");
        }
    }

    public void Leave(GameObject player)
    {
        if (isOccupied)
        {
            // Make the player leave the closet
            player.gameObject.SetActive(true);
            isOccupied = false;
            Debug.Log("Player has left the closet.");
        }
        else
        {
            Debug.Log("Closet is already empty.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isOccupied)
        {
            // If the enemy enters the closet while the player is hiding, game over
            Debug.Log("Game over!");
            // Call game over function here
        }
    }
}