using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class CreateBlendTree : EditorWindow
{
    [SerializeField]
    BlendingTree currentTree;

    blendType currentNodeType;
    BlendNode currentNode;

    bool changeType = false;

    [MenuItem("Window/AnimationBlendTree")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateBlendTree));
    }

    private void Update()
    {
        
    }

    private void OnGUI()
    {
        //making sure the everything is in the right spot based on the blending tree, so data is not lost
        currentTree = EditorGUILayout.ObjectField("Blend Tree", currentTree, typeof(BlendingTree), true) as BlendingTree;
        if (currentTree == null)
        {
            GUILayout.Label("You need a new tree, create a new scriptable object in assets", EditorStyles.boldLabel);
        }
        if(currentTree != null)
        {
            if(currentTree.getRoot() == null)
            {
                currentTree.SetRoot(new BlendNode());
                currentNodeType = blendType.BLEND_INVALID;
            }
            else
            {
                if(currentNode == null)
                {
                    currentNode = currentTree.getRoot();
                }
                //if root is an invalid node-> create a new node
                if (currentTree.getRoot().nodeType == blendType.BLEND_INVALID)
                {
                    currentNodeType = (blendType)EditorGUILayout.EnumPopup("root blend node type", currentNodeType);
                    if (currentNodeType != blendType.BLEND_INVALID && currentNodeType != currentTree.getRoot().nodeType)
                    {
                        currentTree.SetRoot(changeNewNode(currentNodeType));
                        currentNode = currentTree.getRoot();
                    }
                }
                else
                {
                    nodeWorkings();
                }
            }
   
        }    
    }

    BlendNode changeNewNode(blendType newType)
    {
        BlendNode newNode = new BlendNode();

        switch (newType)
        {
            case blendType.BLEND_LERP:
                newNode = new BlendLerp();
                break;
            case blendType.BLEND_ADD:
                newNode = new BlendAdd();
                break;
            case blendType.BLEND_SCALE:
                newNode = new BlendScale();
                break;
            case blendType.BLEND_AVG:
                newNode = new BlendAvg();
                break;
            case blendType.BLEND_END:
                newNode = new blendEnd();
                break;
            default:
                return null;
        }
        changeType = false;
        return newNode;
    }

    void nodeWorkings()
    {
        //navigate back to the root of tree
        if (currentNode != currentTree.getRoot() && GUILayout.Button("Go to root"))
        {
            currentNode = currentTree.getRoot();
            currentNodeType = currentNode.nodeType;
        }

        //navigate to connections
        if(currentNode != currentTree.getRoot())
        {
            if (currentNode.nodePrev != null)
            {
                if (GUILayout.Button("Previous Node"))
                {
                    currentNode = currentNode.nodePrev;
                    currentNodeType = currentNode.nodeType;
                }
            }
        }
        else
        {
            GUILayout.Label("IS ROOT", EditorStyles.boldLabel);
        }

        if(currentNode.nodeOne != null)
        {
            if (GUILayout.Button("Next Node branch 1"))
            {
                currentNode = currentNode.nodeOne;
                currentNodeType = currentNode.nodeType;
            }
        }
        if (currentNode.nodeTwo != null)
        {
            if (GUILayout.Button("Next Node branch 2"))
            {
                currentNode = currentNode.nodeTwo;
                currentNodeType = currentNode.nodeType;
            }
        }

        GUILayout.Label("Current: " + currentNode.nodeType, EditorStyles.miniLabel);
        changeType = EditorGUILayout.Toggle("change node type", changeType);

        if(changeType)
        {
            //change type
            BlendNode newNode = null;
            GUILayout.Label("Generate new Node", EditorStyles.miniLabel);
            if (GUILayout.Button("New Lerp"))
            {
                newNode = new BlendLerp();
            }
            if (GUILayout.Button("New Scale"))
            {
                newNode = new BlendScale();
            }
            if (GUILayout.Button("New Average"))
            {
                newNode = new BlendAvg();
            }
            if (GUILayout.Button("New Add"))
            {
                newNode = new BlendAdd();
            }
            if (GUILayout.Button("New End"))
            {
                newNode = new blendEnd();
            }

            if(newNode != null)
            {
                if (currentTree.getRoot() == currentNode)
                {
                    currentTree.SetRoot(newNode);
                    currentNode = currentTree.getRoot();
                }
                else
                {
                    BlendNode prevNode = currentNode.nodePrev;
                    BlendNode oldNode = currentNode;

                    currentNode = newNode;
                    currentNode.nodePrev = prevNode;

                    if(prevNode.nodeOne == oldNode)
                    {
                        prevNode.nodeOne = currentNode;
                    }
                    else
                    {
                        prevNode.nodeTwo = currentNode;
                    }
                    
                }
                changeType = false;
            }

          
        }
        else
        {
            GUILayout.Label("Node Data", EditorStyles.boldLabel);
            //do node
            switch (currentNode.nodeType)
            {
                case blendType.BLEND_INVALID:
                    GUILayout.Label("This Node is invalid, please change to a valid type", EditorStyles.miniLabel);
                    break;
                case blendType.BLEND_LERP:
                    BlendLerp lerpNode =  (BlendLerp)currentNode;
                    lerpNode.parameter = EditorGUILayout.Slider("Lerp Parameter", lerpNode.parameter, 0, 1);
                    lerpNode = (BlendLerp)checkBranchOne(lerpNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    lerpNode = (BlendLerp)checkBranchTwo(lerpNode);

                    currentNode = lerpNode;
                    break;
                case blendType.BLEND_ADD:
                    BlendAdd addNode = (BlendAdd)currentNode;
                    addNode = (BlendAdd)checkBranchOne(addNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    addNode = (BlendAdd)checkBranchTwo(addNode);

                    currentNode = addNode;
                    break;
                case blendType.BLEND_SCALE:
                    BlendScale scaleNode = (BlendScale)currentNode;
                    scaleNode.parameter = EditorGUILayout.Slider("Scale Parameter", scaleNode.parameter, 0, 1);
                    scaleNode = (BlendScale)checkBranchOne(scaleNode);

                    currentNode = scaleNode;
                    break;
                case blendType.BLEND_AVG:
                    BlendAvg avgNode = (BlendAvg)currentNode;
                    avgNode.parameter1 = EditorGUILayout.Slider("Average Parameter One", avgNode.parameter1, 0, 1);
                    avgNode = (BlendAvg)checkBranchOne(avgNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    avgNode.parameter2 = EditorGUILayout.Slider("Average Parameter Two", avgNode.parameter2, 0, 1);
                    avgNode = (BlendAvg)checkBranchTwo(avgNode);

                    currentNode = avgNode;
                    break;
                case blendType.BLEND_END:
                    blendEnd endNode = (blendEnd)currentNode;
                    endNode.clip = EditorGUILayout.ObjectField("AnimationClip", endNode.clip, typeof(AnimationClip), true) as AnimationClip;

                    currentNode = endNode;
                    break;
                default:
                    break;
            }
        }


        //based on type, show parameters
        //show next nodes-also id, if empty create new node and add index
        //show next nodes-also id, if empty create new ndoe and add index
        //note: when creating new node, it is the default end node
    }

    BlendNode checkBranchOne(BlendNode node)
    {
        if (node.nodeOne == null)
        {
            GUILayout.Label("Generate new Node", EditorStyles.miniLabel);
            if(GUILayout.Button("New Lerp"))
            {
                node.nodeOne = new BlendLerp();
            }
            if (GUILayout.Button("New Scale"))
            {
                node.nodeOne = new BlendScale();
            }
            if (GUILayout.Button("New Average"))
            {
                node.nodeOne = new BlendAvg();
            }
            if (GUILayout.Button("New Add"))
            {
                node.nodeOne = new BlendAdd();
            }
            if (GUILayout.Button("New End"))
            {
                node.nodeOne = new blendEnd();
            }

            if(node.nodeOne != null)
            {
                node.nodeOne.nodePrev = node;
            }
         
        }
        else
        {
            GUILayout.Label("Node one, type: " + node.nodeOne.nodeType, EditorStyles.miniLabel);
            if (GUILayout.Button("Delete Node?"))
            {
                node.nodeOne = null;
            }
        }

        return node;
    }

    BlendNode checkBranchTwo(BlendNode node)
    {
    
        if (node.nodeTwo == null)
        {
            GUILayout.Label("Generate new Node", EditorStyles.miniLabel);
            if (GUILayout.Button("New Lerp"))
            {
                node.nodeTwo = new BlendLerp();
            }
            if (GUILayout.Button("New Scale"))
            {
                node.nodeTwo = new BlendScale();
            }
            if (GUILayout.Button("New Average"))
            {
                node.nodeTwo = new BlendAvg();
            }
            if (GUILayout.Button("New Add"))
            {
                node.nodeTwo = new BlendAdd();
            }
            if (GUILayout.Button("New End"))
            {
                node.nodeTwo = new blendEnd();
            }

            if (node.nodeTwo != null)
            {
                node.nodeTwo.nodePrev = node;
            }
        }
        else
        {
            GUILayout.Label("Node two, type: " + node.nodeTwo.nodeType, EditorStyles.miniLabel);
            if (GUILayout.Button("Delete Node?"))
            {
                node.nodeTwo = null;
            }
        }

        return node;
    }
}
