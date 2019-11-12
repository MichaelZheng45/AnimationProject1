using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlendingTree", menuName = "ScriptableObjects/BlendingTree", order = 3)]
[System.Serializable]
public class BlendingTree : ScriptableObject
{
    public BlendNode rootNode;

    public BlendingTree()
    {
        rootNode = new BlendNode();
    }

    public BlendNode getRoot()
    {
        return rootNode;
    }

    public void SetRoot(BlendNode newRoot)
    {
        rootNode = newRoot;
    }
    public blendPoseData useBlendTree(int keyFrameIndex)
    {
        return rootNode.blendOperation(keyFrameIndex);
    }
}
