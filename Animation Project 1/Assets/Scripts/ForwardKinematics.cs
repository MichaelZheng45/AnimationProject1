using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematics
{

	//used for blending, given a joint with a transform data and the hierarchies do forward kinematics
	public static void setData(gameObjectMain objHierarchy, AnimationDataHierarchal animData, animationTransformData transformData, int jointID)
	{
		int parentIndex = animData.poseBase[jointID].parentNodeIndex;
		Vector3 localPosition = animData.poseBase[jointID].getLocalPosition();
		Vector3 localRotation = animData.poseBase[jointID].getLocalRotationEuler();

		//find delta change from localpose
		Matrix4x4 deltaMatrix = Matrix4x4.TRS(localPosition + transformData.localPosition, Quaternion.Euler(localRotation + transformData.localRotation.eulerAngles), new Vector4(1, 1, 1, 1));

		if (parentIndex == -1)
		{
			//is root
			animData.poseBase[jointID].currentTransform = deltaMatrix;
		}
		else
		{
			//current transform = take the parent index current transform and multiply with delta matrix
			animData.poseBase[jointID].currentTransform = animData.poseBase[parentIndex].currentTransform * deltaMatrix;
		}

		animData.poseBase[jointID].updateNewPosition(objHierarchy.ObjectHierarchy[jointID]);
	}

}
