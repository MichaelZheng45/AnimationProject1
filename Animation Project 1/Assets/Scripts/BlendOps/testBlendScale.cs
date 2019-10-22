using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBlendScale : TestBlend
{
    public Transform pose_1 = null;

    [Range(0.0f, 1.0f)]
    public float parameter = 1.0f;

    // Update is called once per frame
    void Update()
    {
        //Logic:same as lerp operation as if pose_0 is pose_identity
    }
}
