﻿using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
    }
}