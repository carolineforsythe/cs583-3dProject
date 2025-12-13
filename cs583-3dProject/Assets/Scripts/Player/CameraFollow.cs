using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // MonkeyPlayer
    public float distance = 6f;       // how far behind the monkey
    public float height = 3f;         // how high above the monkey
    public float rotationDamping = 5f;
    public float heightDamping = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // --- Desired rotation & height based on the monkey ---
        float desiredRotationAngle = target.eulerAngles.y;
        float desiredHeight = target.position.y + height;

        // Current rotation & height of the camera
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Smoothly interpolate rotation and height
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, desiredRotationAngle,
                                               rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, desiredHeight,
                                   heightDamping * Time.deltaTime);

        // Convert angle to a rotation
        Quaternion currentRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);

        // Position the camera behind the monkey
        Vector3 pos = target.position - currentRotation * Vector3.forward * distance;
        pos.y = currentHeight;

        transform.position = pos;

        // Always look at the monkey
        transform.LookAt(target);
    }
}


