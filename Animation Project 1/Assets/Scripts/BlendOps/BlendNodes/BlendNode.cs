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
    public BlendNode nodePrev;

    public BlendNode nodeOne;

    public BlendNode nodeTwo;

    public blendType nodeType;

    public BlendNode()
    {
        setType();
    }

    public virtual void setType()
    {
        nodeType = blendType.BLEND_INVALID;
    }

    public virtual blendPoseData blendOperation(int currentFrameID)
    {
        return new blendPoseData();
    }
}
