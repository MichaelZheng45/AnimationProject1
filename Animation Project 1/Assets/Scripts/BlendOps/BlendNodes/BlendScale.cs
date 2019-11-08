using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendScale : BlendNode
{
    public float parameter;
    public override blendPoseData blendOperation(int currentFrameID)
    {
        blendPoseData firstPose = nodeOne.blendOperation(currentFrameID);
        int length = firstPose.size;

        for(int i =0; i < length; i++)
        {
            blendTransformData transformData = firstPose.getPoseData(i);
            identity newIdentity = new identity();
            transformData = blendStatic.scale(newIdentity, transformData, parameter, true);
            firstPose.setPoseData(transformData, i);
        }
        return firstPose;
    }
}
