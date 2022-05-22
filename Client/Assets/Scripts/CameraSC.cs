using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSC : MonoBehaviour
{
    public static float width;

    void Start()
    {
        width = Camera.main.orthographicSize * Camera.main.aspect * 2;
    }
}
