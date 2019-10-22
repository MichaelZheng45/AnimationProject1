using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBlendLerp : TestBlend
{
    public Transform pose_0 = null, pose_1 = null;

    [Range(0.0f, 1.0f)]
    public float parameter = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //translation: literal linear interpolation
        poseresult.localPosition = Vector3.Lerp(pose_0.localPosition, pose_1.localPosition, parameter);

        //scale: ditto
        poseresult.localScale = Vector3.Lerp(pose_0.localScale, pose_1.localScale, parameter);
        //rotation: slerp if quaternion (or NLERP) otherwise euler lerp
        if (usingQuaternion)
            poseresult.localRotation = Quaternion.Slerp(pose_0.localRotation, pose_1.localRotation, parameter);
        else
            poseresult.localEulerAngles = Vector3.Lerp(pose_0.localEulerAngles, pose_1.localEulerAngles, parameter);
    }
}
