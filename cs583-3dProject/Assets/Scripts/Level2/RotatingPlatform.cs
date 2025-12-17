using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 45f; // degrees per second

    // Exposed so PlayerController can read it
    [HideInInspector]
    public float deltaRotation;

    void Update()
    {
        // Calculate rotation for this frame
        deltaRotation = rotationSpeed * Time.deltaTime;

        // Rotate platform
        transform.Rotate(Vector3.up, deltaRotation, Space.World);
    }
}
