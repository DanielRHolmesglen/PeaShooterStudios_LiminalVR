using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public Vector3 angularVelocity;
    public Space space = Space.Self;

    void Update()
    {
        transform.Rotate(angularVelocity * Time.deltaTime, space);
    }
}
