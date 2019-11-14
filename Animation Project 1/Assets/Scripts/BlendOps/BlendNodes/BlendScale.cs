using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendScale : BlendNode
{
    public float parameter;
    public override void setType()
    {
        nodeType = blendType.BLEND_SCALE;
    }


    public override blendPoseData blendOperation(BlendingTree parentTree, int currentFrameID)
    {
        blendPoseData firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);

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
