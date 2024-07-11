using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnableBase : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public void Rotate(float angle_){
        
        transform.Rotate(Vector3.forward * angle_);
    }
}
