using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendNode
{
    public BlendNode nodeOne;
    public BlendNode nodeTwo;

    public virtual blendPoseData blendOperation(int currentFrameID)
    {
        return new blendPoseData();
    }
}
