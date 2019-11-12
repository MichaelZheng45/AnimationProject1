using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blendEnd : BlendNode
{
    public AnimationClip clip;
    public override void setType()
    {
        nodeType = blendType.BLEND_END;
    }

    public override blendPoseData blendOperation(int currentFrameID)
    {
        blendPoseData newPoseData = new blendPoseData();
        newPoseData.setData(clip, currentFrameID);
        return newPoseData;
    }
}
