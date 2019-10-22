using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBlendAdd : TestBlend
{
    public Transform pose0 = null, pose1 = null;

    // Update is called once per frame
    void Update()
    {
        //translation literal addition
        poseresult.localPosition = pose0.localPosition + pose1.localPosition;

        //scale component-wise multiplication
        poseresult.localScale = pose0.localScale;
        poseresult.localScale.Scale(pose1.localScale);

        //rotation: quaternioni concatenation or Euler addition
        if (usingQuaternion)
        {
            poseresult.localRotation = pose0.localRotation * pose1.localRotation;
        }
        else
        {
            poseresult.localEulerAngles = pose0.localEulerAngles + pose1.localEulerAngles;
        }
    }
}
