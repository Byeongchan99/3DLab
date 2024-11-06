using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            // 특성 검사
            if (aspect.aspectName == aspectName)
            {
                print("Enemy Touch Detected");
            }
        }
    }
}
