using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blendEnd : BlendNode
{
    AnimationClip clip;

    public override blendPoseData blendOperation(int currentFrameID)
    {
        blendPoseData newPoseData = new blendPoseData();
        newPoseData.setData(clip, currentFrameID);
        return newPoseData;
    }
}
