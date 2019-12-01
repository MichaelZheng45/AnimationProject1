  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationHierarchyEditor : EditorWindow
{
	public AnimationDataHierarchal animData;
	public gameObjectMain gameObjectHierarchy;
	int currentKeyFrame;

	[MenuItem("Window/AnimationHierarchyEditor")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(AnimationHierarchyEditor));
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnGUI()
	{
		
	}
}
