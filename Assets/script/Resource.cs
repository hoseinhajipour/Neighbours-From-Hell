using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone
}

public class Resource : MonoBehaviour
{
    public ResourceType resourceType; // the type of resource this GameObject represents
    public string resourceName;
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        // Add any logic for when the resource is destroyed
        Destroy(gameObject);
    }
}
