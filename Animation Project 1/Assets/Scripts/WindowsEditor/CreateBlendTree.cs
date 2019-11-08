using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateBlendTree : EditorWindow
{
    public BlendingTree mainTree;
    int nodeIndex = 0;
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
        mainTree = EditorGUILayout.ObjectField("Blend Tree", mainTree, typeof(BlendingTree), true) as BlendingTree;
        if(mainTree == null)
        {
            if(GUILayout.Button("Create new Tree"))
            {
                mainTree = new BlendingTree();
            }
        }

        //at node and current tree idnex
        //show what level
        //change node option
        
        //based on type, show parameters
        //show next nodes-also id, if empty create new node and add index
        //show next nodes-also id, if empty create new ndoe and add index
        //note: when creating new node, it is the default end node
    }
}
