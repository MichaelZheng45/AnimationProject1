using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendAvg : BlendNode
{
    float parameter1, parameter2;

    public override blendPoseData blendOperation(int currentFrameID)
    {
        blendPoseData firstPose = nodeOne.blendOperation(currentFrameID);
        blendPoseData secondPose = nodeTwo.blendOperation(currentFrameID);
        int length = firstPose.size;

        for (int i = 0; i < length; i++)
        {
            blendTransformData transformData;
            identity nIdentity = new identity();

            transformData = blendStatic.average(nIdentity, firstPose.getPoseData(i), secondPose.getPoseData(i), parameter1, parameter2, true);
            firstPose.setPoseData(transformData, i);
        }
        return firstPose;
    }
}
