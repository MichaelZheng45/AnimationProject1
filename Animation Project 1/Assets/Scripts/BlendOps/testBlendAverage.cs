using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBlendAverage : MonoBehaviour
{
    public Transform pose_0 = null, pose_1 = null;

    [Range(0.0f, 1.0f)]
    public float parameter1 = 1.0f;

    [Range(0.0f, 1.0f)]
    public float parameter2 = 1.0f;
    // Update is called once per frame
    void Update()
    {
        identity nIdentity = new identity();

    }
}
