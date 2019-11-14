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
    int currentNode = -1;

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
                if(currentNode == -1)
                {
                    currentNode = currentTree.getRoot().currentIndex;
                }
                //if root is an invalid node-> create a new node
                if (currentTree.getRoot().nodeType == blendType.BLEND_INVALID)
                {
                    currentNodeType = (blendType)EditorGUILayout.EnumPopup("root blend node type", currentNodeType);
                    if (currentNodeType != blendType.BLEND_INVALID && currentNodeType != currentTree.getRoot().nodeType)
                    {
                        currentTree.SetRoot(changeNewNode(currentNodeType));
                        currentNode = currentTree.getRoot().currentIndex;
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
        if (currentNode != 0 && GUILayout.Button("Go to root"))
        {
            currentNode = 0;
            currentNodeType = currentTree.getRoot().nodeType;
        }

        GUILayout.Label("CurrentIndex: " + currentNode, EditorStyles.boldLabel);

        //navigate to connections
        if (currentNode != 0)
        {
            BlendNode prevNode = currentTree.getIndexedNode(currentTree.getIndexedNode(currentNode).prevIndex);
            if (prevNode != null)
            {
                if (GUILayout.Button("Previous Node"))
                {
                    currentNode = prevNode.currentIndex;
                    currentNodeType = prevNode.nodeType;
                }
            }
        }
        else if(currentNode == 0)
        {
            GUILayout.Label("IS ROOT", EditorStyles.boldLabel);
        }

     
        BlendNode nextNode1 = currentTree.getIndexedNode(currentTree.getIndexedNode(currentNode).nextID1);
        if (nextNode1 != null)
        {
            if (GUILayout.Button("Next Node branch 1"))
            {
                currentNode = nextNode1.currentIndex;
                currentNodeType = nextNode1.nodeType;
            }
        }

        BlendNode nextNode2 = currentTree.getIndexedNode(currentTree.getIndexedNode(currentNode).nextID2);
        if (nextNode2 != null)
        {
            if (GUILayout.Button("Next Node branch 2"))
            {
                currentNode = nextNode2.currentIndex;
                currentNodeType = nextNode1.nodeType;
            }
        }

        GUILayout.Label("Current: " + currentTree.getIndexedNode(currentNode).nodeType, EditorStyles.miniLabel);
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
                if (0 == currentNode)
                {
                    currentTree.SetRoot(newNode);
                    currentNode = 0;
                }
                else
                {
                    int index = currentNode;
                
                    currentTree.setIndexedNode(index, newNode);
                    
                }
                changeType = false;
            }

          
        }
        else
        {
            GUILayout.Label("Node Data", EditorStyles.boldLabel);
            //do node
            switch (currentTree.getIndexedNode(currentNode).nodeType)
            {
                case blendType.BLEND_INVALID:
                    GUILayout.Label("This Node is invalid, please change to a valid type", EditorStyles.miniLabel);
                    break;
                case blendType.BLEND_LERP:
                    BlendLerp lerpNode = (BlendLerp)currentTree.getIndexedNode(currentNode);
                    lerpNode.parameter = EditorGUILayout.Slider("Lerp Parameter", lerpNode.parameter, 0, 1);
                    lerpNode = (BlendLerp)checkBranchOne(lerpNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    lerpNode = (BlendLerp)checkBranchTwo(lerpNode);

                    break;
                case blendType.BLEND_ADD:
                    BlendAdd addNode = (BlendAdd)currentTree.getIndexedNode(currentNode);
                    addNode = (BlendAdd)checkBranchOne(addNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    addNode = (BlendAdd)checkBranchTwo(addNode);

                    break;
                case blendType.BLEND_SCALE:
                    BlendScale scaleNode = (BlendScale)currentTree.getIndexedNode(currentNode);
                    scaleNode.parameter = EditorGUILayout.Slider("Scale Parameter", scaleNode.parameter, 0, 1);
                    scaleNode = (BlendScale)checkBranchOne(scaleNode);

                    break;
                case blendType.BLEND_AVG:
                    BlendAvg avgNode = (BlendAvg)currentTree.getIndexedNode(currentNode);
                    avgNode.parameter1 = EditorGUILayout.Slider("Average Parameter One", avgNode.parameter1, 0, 1);
                    avgNode = (BlendAvg)checkBranchOne(avgNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    avgNode.parameter2 = EditorGUILayout.Slider("Average Parameter Two", avgNode.parameter2, 0, 1);
                    avgNode = (BlendAvg)checkBranchTwo(avgNode);

                    break;
                case blendType.BLEND_END:
                    blendEnd endNode = (blendEnd)currentTree.getIndexedNode(currentNode);
                    endNode.clip = EditorGUILayout.ObjectField("AnimationClip", endNode.clip, typeof(AnimationClip), true) as AnimationClip;

                    break;
                default:
                    break;
            }

            //delete node option
        }


        //based on type, show parameters
        //show next nodes-also id, if empty create new node and add index
        //show next nodes-also id, if empty create new ndoe and add index
        //note: when creating new node, it is the default end node
    }

    BlendNode checkBranchOne(BlendNode node)
    {
        BlendNode newNode = null;
        BlendNode atNode = currentTree.getIndexedNode(node.nextID1);
        if (atNode == null)
        {
            GUILayout.Label("Generate new Node", EditorStyles.miniLabel);
            if(GUILayout.Button("New Lerp"))
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
                newNode.prevIndex = node.currentIndex;
                int currentIndex = currentTree.addNewNode(newNode);
                node.nextID1 = currentIndex;
            }
         
        }
        else
        {

            GUILayout.Label("Node one, type: " + atNode.GetType(), EditorStyles.miniLabel);
            /*
            if (GUILayout.Button("Delete Node?"))
            {
                node.nodeOne = null;
            } */
        }

        return node;
    }

    BlendNode checkBranchTwo(BlendNode node)
    {
        BlendNode newNode = null;
        BlendNode atNode = currentTree.getIndexedNode(node.nextID2);
        if (atNode == null)
        {
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

            if (newNode != null)
            {
                newNode.prevIndex = node.currentIndex;
                int currentIndex = currentTree.addNewNode(newNode);
                node.nextID2 = currentIndex;
            }

        }
        else
        {
            GUILayout.Label("Node rwo, type: " + atNode.GetType(), EditorStyles.miniLabel);
        }
            return node;
    }
}
