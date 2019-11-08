using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendingTree : ScriptableObject
{
    BlendNode rootNode;

    public blendPoseData useBlendTree(int keyFrameIndex)
    {

        return rootNode.blendOperation(keyFrameIndex);
    }
}
