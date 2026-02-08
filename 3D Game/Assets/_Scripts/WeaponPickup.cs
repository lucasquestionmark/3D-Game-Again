using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Drag the actual weapon object (the one attached to your Player's camera/hand) here.")]
    public GameObject weaponInHand;

    [Tooltip("Optional: Drag a sound effect here.")]
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the Player
        if (other.CompareTag("Player"))
        {
            // 1. Enable the weapon in the player's hand
            if (weaponInHand != null)
            {
                weaponInHand.SetActive(true);
                Debug.Log("Weapon Equipped!");
            }
            else
            {
                Debug.LogError("You forgot to assign the 'Weapon In Hand' variable in the Inspector!");
            }

            // 2. Play sound (PlayClipAtPoint creates a temporary audio source so the sound finishes even after this object is destroyed)
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // 3. Destroy this object (the one floating on the ground)
            Destroy(gameObject);
        }
    }
}