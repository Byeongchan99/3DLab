using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithAddForce : MonoBehaviour
{
    public float force = 5f;

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }
}
