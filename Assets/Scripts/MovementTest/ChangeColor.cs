using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color newColor; // ������ ����

    void Start()
    {
        GetComponent<Renderer>().material.color = newColor;
    }
}
