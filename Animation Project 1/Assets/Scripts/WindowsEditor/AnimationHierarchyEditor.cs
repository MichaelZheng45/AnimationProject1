  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationHierarchyEditor : EditorWindow
{
	public AnimationDataHierarchal animData;
	public gameObjectMain gameObjectHierarchy;
	int currentKeyFrame;

	bool createNewHierarchy = false;
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
		animData = EditorGUILayout.ObjectField("AnimationData", animData, typeof(AnimationDataHierarchal), true) as AnimationDataHierarchal;

		if(!animData)
		{
			GUILayout.Label("Need AnimationData");
		}
		else
		{
			gameObjectHierarchy = EditorGUILayout.ObjectField("GameObjectHierarchy", gameObjectHierarchy, typeof(gameObjectMain), true) as gameObjectMain;

			createNewHierarchy = EditorGUILayout.Toggle("Create New Hierarchy", createNewHierarchy);
			if (createNewHierarchy)
			{
				newHierarchy();
			}
			else
			{
				modifyAnimationData();
			}
		}
	}

	void modifyAnimationData()
	{
		animData.framePerSecond = EditorGUILayout.FloatField("FrameRate(in seconds)", animData.framePerSecond);
		if(GUILayout.Button("Set New Frame"))
		{
			animData.keyFrameCount++;
			animData.totalFrameDuration++;

			for(int i = 0; i < animData.poseBase.Length; i++)
			{
				//create new keyframe
				Transform poseObj = gameObjectHierarchy.getObject(i).transform;
				Vector3 localPosition = animData.poseBase[i].getLocalPosition();
				Vector3 localRotation = animData.poseBase[i].getLocalRotationEuler();

				KeyFrame newKey = new KeyFrame(poseObj.localPosition - localPosition, poseObj.localEulerAngles - localRotation, poseObj.localScale, animData.keyFrameCount - 1);
				animData.poseBase[i].keyFrames.Add(newKey);
			}
		}

		if(animData.keyFrameCount > 0 && GUILayout.Button("Remove Recent Key Frame"))
		{
			int recentKeyCount = animData.keyFrameCount;
			animData.keyFrameCount--;
			animData.totalFrameDuration--;
			for (int i = 0; i < animData.poseBase.Length; i++)
			{
				animData.poseBase[i].keyFrames.RemoveAt(recentKeyCount);
			}
		}
	}

	void checkChildren(GameObject currentObject, int currentIndex)
	{
		foreach(Transform child in currentObject.transform)
		{
			int childIndex = gameObjectHierarchy.addObjectWithIndex(child.gameObject, currentIndex);
			animData.addNewPose(child.gameObject,currentObject, currentIndex);
			checkChildren(child.gameObject, childIndex);
		}
	}

	void newHierarchy()
	{
		
		if(gameObjectHierarchy)
		{
			GameObject rootObject;
			rootObject = EditorGUILayout.ObjectField("Root", null, typeof(GameObject), true) as GameObject;
			if(rootObject)
			{
				gameObjectHierarchy.animData = animData;
				animData.deletePoses();
				gameObjectHierarchy.newList();
				createNewHierarchy = false;

				int rootIndex = gameObjectHierarchy.addObjectWithIndex(rootObject, -1);
				animData.addNewPose(rootObject, rootObject, -1);
				checkChildren(rootObject, rootIndex);
			}
		}
		else
		{
			GUILayout.Label("Need GameObjectHierarchy");
		}
	}
}
