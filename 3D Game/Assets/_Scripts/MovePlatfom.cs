using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    public Vector3 moveDirection = Vector3.right;

    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Vector3 platformVelocity;
    
    // List to keep track of multiple objects (Players, Crates, etc.) on the platform
    private List<Transform> passengers = new List<Transform>();

    void Start()
    {
        startPosition = transform.position;
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        // 1. Move the platform
        float movement = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPosition + (moveDirection.normalized * movement);

        // 2. Calculate exactly how much we moved since the last physics frame
        platformVelocity = transform.position - previousPosition;
        previousPosition = transform.position;

        // 3. Manually push every passenger by that same amount
        foreach (Transform passenger in passengers)
        {
            if (passenger != null)
            {
                // This moves the player exactly as much as the platform moved
                passenger.position += platformVelocity;
            }
        }
    }

    // Detect when someone stands on the platform
    private void OnTriggerEnter(Collider other)
    {
        if (!passengers.Contains(other.transform) && !other.isTrigger)
        {
            passengers.Add(other.transform);
        }
    }

    // Detect when they leave
    private void OnTriggerExit(Collider other)
    {
        if (passengers.Contains(other.transform))
        {
            passengers.Remove(other.transform);
        }
    }
}