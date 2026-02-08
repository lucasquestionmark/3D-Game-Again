using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Settings")]
    public float attackRange = 2.0f;
    public int damage = 1;
    public float attackDelay = 0.5f;

    [Header("Visuals (Wiggle)")]
    public Transform swordPivot; // The sword object itself
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    void Update()
    {
        // Check for Left Mouse Click
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            StartCoroutine(PerformSwing());
            nextAttackTime = Time.time + attackDelay;
        }
    }

    System.Collections.IEnumerator PerformSwing()
    {
        isAttacking = true;
        
        // 1. Simple Visual Swing (Rotate down, then back up)
        Quaternion startRot = swordPivot.localRotation;
        Quaternion endRot = Quaternion.Euler(45, 0, 0); // Tilts the sword forward

        // Swing Down
        float elapsed = 0;
        while (elapsed < 0.1f) {
            swordPivot.localRotation = Quaternion.Slerp(startRot, endRot, elapsed / 0.1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2. Check for Hit (The "Damage" Part)
        CheckHit();

        // Swing Back
        elapsed = 0;
        while (elapsed < 0.1f) {
            swordPivot.localRotation = Quaternion.Slerp(endRot, startRot, elapsed / 0.1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        swordPivot.localRotation = startRot;
        isAttacking = false;
    }

    void CheckHit()
    {
        // Shoot an invisible beam forward from the camera
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit Enemy!");
                // Here we will eventually call a 'TakeDamage' function on the enemy
                Destroy(hit.collider.gameObject); 
            }
        }
    }
}