using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithAddVelocity : MonoBehaviour
{
    public float speed = 5f;

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
