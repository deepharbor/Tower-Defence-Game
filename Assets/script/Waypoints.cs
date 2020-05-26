using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] positions;

    void Awake()
    {
        positions = new Transform[transform.childCount];
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i);
        }
    }
}
