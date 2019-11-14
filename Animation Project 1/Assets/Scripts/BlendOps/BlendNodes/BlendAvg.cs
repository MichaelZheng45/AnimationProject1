using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendAvg : BlendNode
{
    public float parameter1, parameter2;

    public override void setType()
    {
        nodeType = blendType.BLEND_AVG;
    }


    public override blendPoseData blendOperation(BlendingTree parentTree, int currentFrameID)
    {
        blendPoseData firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree,currentFrameID);
        blendPoseData secondPose = parentTree.getIndexedNode(nextID2).blendOperation(parentTree, currentFrameID);
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
