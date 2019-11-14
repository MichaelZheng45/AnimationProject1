using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendAdd : BlendNode
{
    public override void setType()
    {
        nodeType = blendType.BLEND_ADD;
    }

    public override blendPoseData blendOperation(BlendingTree parentTree, int currentFrameID)
    {
        blendPoseData firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);
        blendPoseData secondPose = parentTree.getIndexedNode(nextID2).blendOperation(parentTree, currentFrameID);

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
