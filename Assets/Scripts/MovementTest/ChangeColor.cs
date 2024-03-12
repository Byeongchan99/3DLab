using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color newColor; // 변경할 색상

    void Start()
    {
        GetComponent<Renderer>().material.color = newColor;
    }
}
