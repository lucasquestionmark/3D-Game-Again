using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Drag an empty GameObject here to set where the player goes.")]
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if it is the Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player fell! Respawning...");

            // 2. Handle CharacterController (Standard FPS Controller)
            // We must disable it briefly, or it will override our teleportation.
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // 3. Teleport the Player
            other.transform.position = respawnPoint.position;
            other.transform.rotation = respawnPoint.rotation;

            // 4. Handle Rigidbody (Physics-based Controller)
            // Stop them from keeping their falling momentum.
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // 5. Re-enable CharacterController
            if (cc != null) cc.enabled = true;
        }
        else
        {
            // Optional: Destroy enemies or items that fall off
            Destroy(other.gameObject);
        }
    }
}