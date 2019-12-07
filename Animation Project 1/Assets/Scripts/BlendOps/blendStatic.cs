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

public static class blendStatic
{

	public static KeyFrame lerpKey(KeyFrame from, KeyFrame to, float parameter, bool usingQuaternion = false)
	{
		KeyFrame newKey = new KeyFrame();
		//translation: literal linear interpolation
		newKey.keyPosition = Vector3.Lerp(from.keyPosition, to.keyPosition, parameter);

		//scale: ditto
		newKey.scale = Vector3.Lerp(from.scale, to.scale, parameter);
		//rotation: slerp if quaternion (or NLERP) otherwise euler lerp
		if (usingQuaternion)
			newKey.keyQRotation = Quaternion.Slerp(from.keyQRotation, to.keyQRotation, parameter);
		else
			newKey.keyRotation = (Vector3.Lerp(from.keyRotation, to.keyRotation, parameter));

		return newKey;
	}

	public static animationTransformData lerp(animationTransformData pose_0, animationTransformData pose_1, float parameter, bool usingQuaternion = false)
    {
        animationTransformData poseresult = new animationTransformData();
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

    public static animationTransformData add(animationTransformData pose0, animationTransformData pose1, bool usingQuaternion)
    {
        animationTransformData poseresult = new animationTransformData();
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

    public static animationTransformData scale(identity nIdentity, animationTransformData pose_1, float parameter, bool usingQuaternion)
    {
        animationTransformData poseresult = new animationTransformData();
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

    public static animationTransformData average(identity nIdentity, animationTransformData pose_0, animationTransformData pose_1, float parameter0, float parameter1, bool usingQuaternion)
    {
        animationTransformData newPose_0 = scale( new identity(), pose_0, parameter0, usingQuaternion);
        animationTransformData newPose_1 = scale( new identity(), pose_1, parameter1, usingQuaternion);
        return add(newPose_0, newPose_1, usingQuaternion);
    }
}
