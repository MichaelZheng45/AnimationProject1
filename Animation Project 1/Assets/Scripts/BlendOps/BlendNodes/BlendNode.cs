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

    public BlendNode()
    {
        nextID1 = -1;
        nextID2 = -1;
        setType();
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
        return new blendPoseData();
    }
}
