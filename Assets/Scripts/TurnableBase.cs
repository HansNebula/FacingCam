using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnableBase : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public void Rotate(float angle_){
        // Vector3 direction = target - transform.position;

        // // Calculate the angle around the Z axis to look at the target
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // // Create a rotation around the Z axis
        // Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // // Apply the rotation to the object
        // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        transform.Rotate(Vector3.forward * angle_);
    }
}
