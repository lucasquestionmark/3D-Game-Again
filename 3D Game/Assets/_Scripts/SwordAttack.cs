using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour
{
    [Header("Settings")]
    public float attackRange = 3.0f;
    public int damage = 1;
    public float attackDelay = 0.5f;

    [Header("Visuals")]
    public Transform swordPivot; // Drag your sword model here

    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    void Update()
    {
        // Left Click to Attack
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && !isAttacking)
        {
            StartCoroutine(PerformSwing());
            nextAttackTime = Time.time + attackDelay;
        }
    }

    IEnumerator PerformSwing()
    {
        isAttacking = true;

        // Store original rotation
        Quaternion startRot = swordPivot.localRotation;
        // Calculate target rotation (swing down 45 degrees)
        Quaternion endRot = startRot * Quaternion.Euler(45, 0, 0);

        // 1. Swing Down
        float elapsed = 0;
        float speed = 0.1f;
        while (elapsed < speed)
        {
            swordPivot.localRotation = Quaternion.Slerp(startRot, endRot, elapsed / speed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2. CHECK FOR HIT (At the bottom of the swing)
        CheckHit();

        // 3. Swing Back Up
        elapsed = 0;
        while (elapsed < speed)
        {
            swordPivot.localRotation = Quaternion.Slerp(endRot, startRot, elapsed / speed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to exact start position
        swordPivot.localRotation = startRot;
        isAttacking = false;
    }

    void CheckHit()
    {
        RaycastHit hit;

        // VISUAL DEBUG: Draws a red line in the Scene view for 2 seconds when you attack
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * attackRange, Color.red, 2.0f);

        // Cast a ray from the camera forward
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange))
        {
            // CONSOLE DEBUG: Prints exactly what object the ray hit
            Debug.Log("I hit: " + hit.collider.name);

            // Check if the object is tagged "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // Try to get the script on the object OR its parent
                EnemyAI enemyScript = hit.collider.GetComponentInParent<EnemyAI>();

                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                    Debug.Log("Dealt Damage to Enemy!");
                }
                else
                {
                    Debug.LogWarning("Hit an Enemy, but could not find the 'EnemyAI' script on it!");
                }
            }
        }
        else
        {
            Debug.Log("I missed (didn't hit anything in range).");
        }
    }
}