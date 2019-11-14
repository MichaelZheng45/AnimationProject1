using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlendingTree", menuName = "ScriptableObjects/BlendingTree", order = 3)]
[System.Serializable]
public class BlendingTree : ScriptableObject
{
    public List<BlendNode> NodeTree;

    public BlendingTree()
    {
        NodeTree = new List<BlendNode>();
        NodeTree.Add(new BlendNode());
        NodeTree[0].prevIndex = -1;
        NodeTree[0].currentIndex = 0;
    }

    public BlendNode getRoot()
    {
        return NodeTree[0];
    }

    public void SetRoot(BlendNode newRoot)
    {
        newRoot.replaceConnections(NodeTree[0]);

        NodeTree[0] = newRoot;
    }
    public blendPoseData useBlendTree(BlendingTree tree,int keyFrameIndex)
    {
        return NodeTree[0].blendOperation(tree,keyFrameIndex);
    }

    public BlendNode getIndexedNode(int index)
    {
        if(index == -1)
        {
            return null;
        }
        return NodeTree[index];
    }

    public void setIndexedNode(int index,BlendNode newNode)
    {
        Debug.Log(index);
        newNode.replaceConnections(NodeTree[index]);

        NodeTree[index] = newNode;
    }

    public int addNewNode(BlendNode node)
    {
        NodeTree.Add(node);
        int newNodeId = NodeTree.IndexOf(node);
        node.currentIndex = newNodeId;
        return newNodeId;
    }
}
