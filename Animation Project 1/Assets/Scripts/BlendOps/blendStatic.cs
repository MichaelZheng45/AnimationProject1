using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class identity
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public identity()
    {
        position = Vector3.zero;
        scale = Vector3.zero;
        rotation = Quaternion.identity;
    }
}

public struct transformData
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;

    public transformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        localPosition = position;
        localRotation = rotation;
        localScale = scale;
    }

    public Vector3 localEulerAngles()
    {
        return localRotation.eulerAngles;
    }

    public void localEulerAnglesSet(Vector3 v)
    {
        localRotation = Quaternion.Euler(v);
    }

    public void changeTransform(Transform thisTransform)
    {
        thisTransform.localPosition = localPosition;
        thisTransform.localRotation = localRotation;
        thisTransform.localScale = localScale;
    }

    public void setTransform(Transform thisTransform)
    {
        localPosition = thisTransform.localPosition;
        localRotation = thisTransform.localRotation;
        localScale = thisTransform.localScale;
    }

    public void setTransformIndividual(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        localPosition = position;
        localRotation = rotation;
        localScale = scale;
    }
}


public static class blendStatic
{

    public static transformData lerp(transformData pose_0, transformData pose_1, float parameter, bool usingQuaternion = false)
    {
        transformData poseresult = new transformData();
        //translation: literal linear interpolation
        poseresult.localPosition = Vector3.Lerp(pose_0.localPosition, pose_1.localPosition, parameter);

        //scale: ditto
        poseresult.localScale = Vector3.Lerp(pose_0.localScale, pose_1.localScale, parameter);
        //rotation: slerp if quaternion (or NLERP) otherwise euler lerp
        if (usingQuaternion)
            poseresult.localRotation = Quaternion.Slerp(pose_0.localRotation, pose_1.localRotation, parameter);
        else
            poseresult.localEulerAnglesSet(Vector3.Lerp(pose_0.localEulerAngles(), pose_1.localEulerAngles(), parameter));

        return poseresult;
    }

    public static transformData add(transformData pose0, transformData pose1, bool usingQuaternion)
    {
        transformData poseresult = new transformData();
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
            poseresult.localEulerAnglesSet(pose0.localEulerAngles() + pose1.localEulerAngles());
        }

        return poseresult;
    }

    public static transformData scale(identity nIdentity, transformData pose_1, float parameter, bool usingQuaternion)
    {
        transformData poseresult = new transformData();
        //translation: literal linear interpolation
        poseresult.localPosition = Vector3.Lerp(nIdentity.position, pose_1.localPosition, parameter);

        //scale: ditto
        poseresult.localScale = Vector3.Lerp(nIdentity.scale, pose_1.localScale, parameter);
        //rotation: slerp if quaternion (or NLERP) otherwise euler lerp
        if (usingQuaternion)
            poseresult.localRotation = Quaternion.Slerp(nIdentity.rotation, pose_1.localRotation, parameter);
        else
            poseresult.localEulerAnglesSet(Vector3.Lerp(nIdentity.rotation.eulerAngles, pose_1.localEulerAngles(), parameter));
        return poseresult;
    }

    public static transformData average(identity nIdentity, transformData pose_0, transformData pose_1, float parameter0, float parameter1, bool usingQuaternion)
    {
        transformData newPose_0 = scale( new identity(), pose_0, parameter0, usingQuaternion);
        transformData newPose_1 = scale( new identity(), pose_1, parameter1, usingQuaternion);
        return add(newPose_0, newPose_1, usingQuaternion);
    }
}
