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
        identity nIdentity = new identity();

        //translation: literal linear interpolation
        poseresult.localPosition = Vector3.Lerp(nIdentity.position, pose_1.localPosition, parameter);

        //scale: ditto
        poseresult.localScale = Vector3.Lerp(nIdentity.scale, pose_1.localScale, parameter);
        //rotation: slerp if quaternion (or NLERP) otherwise euler lerp
        if (usingQuaternion)
            poseresult.localRotation = Quaternion.Slerp(nIdentity.rotation, pose_1.localRotation, parameter);
        else
            poseresult.localEulerAngles = Vector3.Lerp(nIdentity.rotation.eulerAngles, pose_1.localEulerAngles, parameter);

    }
}
