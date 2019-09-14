using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class animationWindowEditor : EditorWindow
{
   
    public AnimationController animController;

    [MenuItem("Window/AnimationHandler")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(animationWindowEditor));
    }

    private void Update()
    {
        
    }

    private void OnGUI()
    {

        animController = EditorGUILayout.ObjectField("AnimationController", animController, typeof(AnimationController), true) as AnimationController;

        if(animController != null)
        {
            GUILayout.Label("Key Frame Count: " +  animController.keyFrames.Count, EditorStyles.miniLabel);
        }

        /*
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
        */
    }


}
