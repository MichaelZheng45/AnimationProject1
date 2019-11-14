using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendLerp : BlendNode
{
    public float parameter;

    public override void setType()
    {
        nodeType = blendType.BLEND_LERP;
    }


    public override blendPoseData blendOperation(BlendingTree parentTree, int currentFrameID)
    {
        blendPoseData firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);
        blendPoseData secondPose = parentTree.getIndexedNode(nextID2).blendOperation(parentTree, currentFrameID);

        int length = firstPose.size;

        for (int i = 0; i < length; i++)
        {
            blendTransformData transformData;

            transformData = blendStatic.lerp(firstPose.getPoseData(i),secondPose.getPoseData(i), parameter, true);
            firstPose.setPoseData(transformData, i);
        }
        return firstPose;
    }
}
