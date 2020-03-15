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
        EditorWindow window = EditorWindow.GetWindow(typeof(CreateBlendTree));
        window.maxSize = new Vector2(500f, 700f);
        window.minSize = window.maxSize;
    }

    private void Update()
    {
        //some value test later
     
    }

    Texture blueTexture;
    private void OnGUI()
    {

       // blueTexture = EditorGUILayout.ObjectField("Texture", blueTexture, typeof(Texture), true) as Texture;
      //  if(GUILayout.Button(blueTexture))
      //  {

     //   }
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
                currentTree.SetRoot(new BlendNode(blendType.BLEND_INVALID));
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
                    //currentNodeType = (blendType)EditorGUILayout.EnumPopup("root blend node type", currentNodeType);
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
        BlendNode newNode = new BlendNode(newType);

        changeType = false;
        return newNode;
    }

    void nodeWorkings()
    {
        //navigate back to the root of tree
        /*
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.blue;
        GUILayout.Button("Label", style);
        */

        //button color
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.blue;
        
        if (currentNode != 0 && GUILayout.Button("Go to root", style))
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
        blendType newType = blendType.BLEND_INVALID;
        newType = (blendType)EditorGUILayout.EnumPopup("Type: ", newType);
        if(changeType)
        {
            //change type
            BlendNode newNode = null;
            GUILayout.Label("Generate new Node", EditorStyles.miniLabel);
            if (GUILayout.Button("New Lerp"))
            {
                newNode = new BlendNode(blendType.BLEND_LERP);
            }
            if (GUILayout.Button("New Scale"))
            {
                newNode = new BlendNode(blendType.BLEND_SCALE);
            }
            if (GUILayout.Button("New Average"))
            {
                newNode = new BlendNode(blendType.BLEND_AVG);
            }
            if (GUILayout.Button("New Add"))
            {
                newNode = new BlendNode(blendType.BLEND_ADD);
            }
            if (GUILayout.Button("New End"))
            {
                newNode = new BlendNode(blendType.BLEND_END);
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
                    BlendNode lerpNode = currentTree.getIndexedNode(currentNode);
                    lerpNode.parameter1 = EditorGUILayout.Slider("Lerp Parameter", lerpNode.parameter1, 0, 1);
                    lerpNode = checkBranchOne(lerpNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    lerpNode = checkBranchTwo(lerpNode);

                    break;
                case blendType.BLEND_ADD:
                    BlendNode addNode = currentTree.getIndexedNode(currentNode);
                    addNode = checkBranchOne(addNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    addNode = checkBranchTwo(addNode);

                    break;
                case blendType.BLEND_SCALE:
                    BlendNode scaleNode = currentTree.getIndexedNode(currentNode);
                    scaleNode.parameter1 = EditorGUILayout.Slider("Scale Parameter", scaleNode.parameter1, 0, 1);
                    scaleNode = checkBranchOne(scaleNode);

                    break;
                case blendType.BLEND_AVG:
                    BlendNode avgNode = currentTree.getIndexedNode(currentNode);
                    avgNode.parameter1 = EditorGUILayout.Slider("Average Parameter One", avgNode.parameter1, 0, 1);
                    avgNode = checkBranchOne(avgNode);
                    GUILayout.Label("[======================]", EditorStyles.miniLabel);
                    avgNode.parameter2 = EditorGUILayout.Slider("Average Parameter Two", avgNode.parameter2, 0, 1);
                    avgNode = checkBranchTwo(avgNode);

                    break;
                case blendType.BLEND_END:
                    BlendNode endNode = currentTree.getIndexedNode(currentNode);
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
            if (GUILayout.Button("New Lerp"))
            {
                newNode = new BlendNode(blendType.BLEND_LERP);
            }
            if (GUILayout.Button("New Scale"))
            {
                newNode = new BlendNode(blendType.BLEND_SCALE);
            }
            if (GUILayout.Button("New Average"))
            {
                newNode = new BlendNode(blendType.BLEND_AVG);
            }
            if (GUILayout.Button("New Add"))
            {
                newNode = new BlendNode(blendType.BLEND_ADD);
            }
            if (GUILayout.Button("New End"))
            {
                newNode = new BlendNode(blendType.BLEND_END);
            }

            if (newNode != null)
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
                newNode = new BlendNode(blendType.BLEND_LERP);
            }
            if (GUILayout.Button("New Scale"))
            {
                newNode = new BlendNode(blendType.BLEND_SCALE);
            }
            if (GUILayout.Button("New Average"))
            {
                newNode = new BlendNode(blendType.BLEND_AVG);
            }
            if (GUILayout.Button("New Add"))
            {
                newNode = new BlendNode(blendType.BLEND_ADD);
            }
            if (GUILayout.Button("New End"))
            {
                newNode = new BlendNode(blendType.BLEND_END);
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
