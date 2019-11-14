using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class blendEnd : BlendNode
{
    [SerializeField]
    public AnimationClip clip;
    public int stuff;
    public override void setType()
    {
        nodeType = blendType.BLEND_END;
    }

    public override blendPoseData blendOperation(BlendingTree parentTree, int currentFrameID)
    {
        blendPoseData newPoseData = new blendPoseData();
        newPoseData.setData(clip, currentFrameID);
        return newPoseData;
    }
}
