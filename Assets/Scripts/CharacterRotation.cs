using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public float rotationSpeed = 5f;

    void Update()
    {
        // Horizontal input for rotation
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Calculate rotation amount based on input
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        // Create a quaternion for the rotation around the character's up axis
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);

        // Apply the rotation to the character's current rotation
        transform.rotation *= deltaRotation;
    }
}
