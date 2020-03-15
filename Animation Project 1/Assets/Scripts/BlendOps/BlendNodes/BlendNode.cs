using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blendType
{
    BLEND_INVALID = -1,
    BLEND_LERP,
    BLEND_ADD,
    BLEND_SCALE,
    BLEND_AVG,
    BLEND_END
}

[System.Serializable]
public class BlendNode
{
    public int prevIndex;
    public int currentIndex;
    public int nextID1;
    public int nextID2;

    public blendType nodeType;
    public float parameter1;
    public float parameter2;
    public AnimationClip clip;

    public BlendNode(blendType newtype)
    {
        nextID1 = -1;
        nextID2 = -1;
        nodeType = newtype;
    }

    public void replaceConnections(BlendNode node)
    {
        prevIndex = node.prevIndex;
        currentIndex = node.currentIndex;
        nextID1 = node.nextID1;
        nextID2 = node.nextID2;
    }

    public virtual void setType()
    {
        nodeType = blendType.BLEND_INVALID;
    }

    public virtual blendPoseData blendOperation(BlendingTree parentTree, int currentFrameID)
    {
        blendPoseData firstPose = new blendPoseData();
        blendPoseData secondPose = new blendPoseData();
        int length=0;
        switch (nodeType)
        {
            case blendType.BLEND_LERP:
                firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);
                secondPose= parentTree.getIndexedNode(nextID2).blendOperation(parentTree, currentFrameID);

                 length = firstPose.size;

                for (int i = 0; i < length; i++)
                {
                    animationTransformData transformData;

                    transformData = blendStatic.lerp(firstPose.getPoseData(i), secondPose.getPoseData(i), parameter1, true);
                    firstPose.setPoseData(transformData, i);
                }
                return firstPose;

            case blendType.BLEND_ADD:
                firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);
                secondPose = parentTree.getIndexedNode(nextID2).blendOperation(parentTree, currentFrameID);

                length = firstPose.size;

                for (int i = 0; i < length; i++)
                {
                    animationTransformData transformData;

                    transformData = blendStatic.add(firstPose.getPoseData(i), secondPose.getPoseData(i), true);
                    firstPose.setPoseData(transformData, i);
                }
                return firstPose;

            case blendType.BLEND_SCALE:
                firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);

                length = firstPose.size;

                for (int i = 0; i < length; i++)
                {
                    animationTransformData transformData = firstPose.getPoseData(i);
                    identity newIdentity = new identity();
                    transformData = blendStatic.scale(newIdentity, transformData, parameter1, true);
                    firstPose.setPoseData(transformData, i);
                }
                return firstPose;
                
            case blendType.BLEND_AVG:
                firstPose = parentTree.getIndexedNode(nextID1).blendOperation(parentTree, currentFrameID);
                secondPose = parentTree.getIndexedNode(nextID2).blendOperation(parentTree, currentFrameID);
                length = firstPose.size;

                for (int i = 0; i < length; i++)
                {
                    animationTransformData transformData;
                    identity nIdentity = new identity();

                    transformData = blendStatic.average(nIdentity, firstPose.getPoseData(i), secondPose.getPoseData(i), parameter1, parameter2, true);
                    firstPose.setPoseData(transformData, i);
                }
                return firstPose;

            case blendType.BLEND_END:
                blendPoseData newPoseData = new blendPoseData();
                newPoseData.setData(clip, currentFrameID);
                return newPoseData;
              
            default:
                return firstPose;
        }
    }
}
