using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithTranslate : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}
