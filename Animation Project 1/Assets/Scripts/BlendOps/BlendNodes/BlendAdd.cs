using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendAdd : BlendNode
{
    public override blendPoseData blendOperation(int currentFrameID)
    {
        blendPoseData firstPose = nodeOne.blendOperation(currentFrameID);
        blendPoseData secondPose = nodeTwo.blendOperation(currentFrameID);
        int length = firstPose.size;

        for (int i = 0; i < length; i++)
        {
            blendTransformData transformData;

            transformData = blendStatic.add(firstPose.getPoseData(i), secondPose.getPoseData(i), true);
            firstPose.setPoseData(transformData, i);
        }
        return firstPose;
    }
}
