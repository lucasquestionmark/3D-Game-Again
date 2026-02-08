using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    public float stopDistance = 1.5f; // Stop before hugging the player
    
    [Header("Combat Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    private Transform playerTarget;

    void Start()
    {
        currentHealth = maxHealth;

        // Automatically find the player so you don't have to drag them in every time
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            playerTarget = playerObj.transform;
        }
    }

    void Update()
    {
        // Simple Chase Logic
        if (playerTarget != null)
        {
            // 1. Look at the player
            // We lock the 'y' position so they don't look up/down, just left/right
            Vector3 lookPos = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
            transform.LookAt(lookPos);

            // 2. Move towards player if we are too far away
            float distance = Vector3.Distance(transform.position, playerTarget.position);
            
            if (distance > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    // This function will be called by your Sword script
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optional: Play a death sound or particle effect here
        Destroy(gameObject);
    }
}