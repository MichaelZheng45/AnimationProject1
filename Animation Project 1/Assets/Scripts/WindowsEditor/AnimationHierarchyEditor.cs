  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationHierarchyEditor : EditorWindow
{
	public AnimationDataHierarchal animData;
	public gameObjectMain gameObjectHierarchy;
	int currentKeyFrame = 0;

	bool createNewHierarchy = false;

    int lineBasePosY = 200;
    int lineMaxPosY = 300;
    int lineMaxPosX = 400;
    int xShift = 50;

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
        Rect rectangle = new Rect(new Vector2(xShift, lineBasePosY), new Vector2(lineMaxPosX, lineMaxPosY - lineBasePosY));
        EditorGUI.DrawRect(rectangle, Color.gray);

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

        animData.totalFrameDuration = EditorGUILayout.IntField("Frame Duration", animData.totalFrameDuration);
        if(animData.totalFrameDuration != animData.keyFrameCount)
        {
            //update by adding/removing all keyframes and then update
        }


        //needs to be changed
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

    void copydownData(int jointID, int frameNumber)
    {
        //create new keyframe
        Transform poseObj = gameObjectHierarchy.getObject(jointID).transform;
        Vector3 localPosition = animData.poseBase[jointID].getLocalPosition();
        Vector3 localRotation = animData.poseBase[jointID].getLocalRotationEuler();

        KeyFrame newKey = new KeyFrame(poseObj.localPosition - localPosition, poseObj.localEulerAngles - localRotation, poseObj.localScale, animData.keyFrameCount - 1);
        animData.poseBase[jointID].keyFrames[frameNumber] = newKey;
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

                //remove old poses and start updating heirarchy
                animData.deletePoses();
				gameObjectHierarchy.newList();
				createNewHierarchy = false;

				int rootIndex = gameObjectHierarchy.addObjectWithIndex(rootObject, -1);
				animData.addNewPose(rootObject, rootObject, -1);
				checkChildren(rootObject, rootIndex);

                //update all old frames and update new ones
                animData.prioFrameKey = new bool[animData.totalFrameDuration];
                for (int i = 0; i < animData.totalFrameDuration; i++)
                {
                    animData.prioFrameKey[i] = false;

                    for (int j = 0; j < animData.poseBase.Length; j++)
                    {
                        //create new keyframe
                        Transform poseObj = gameObjectHierarchy.getObject(j).transform;
                        Vector3 localPosition = animData.poseBase[j].getLocalPosition();
                        Vector3 localRotation = animData.poseBase[j].getLocalRotationEuler();

                        KeyFrame newKey = new KeyFrame(poseObj.localPosition - localPosition, poseObj.localEulerAngles - localRotation, poseObj.localScale, animData.keyFrameCount - 1);
                        animData.poseBase[j].keyFrames.Add(newKey);
                    }

                }
                //the last and first keys are primary keys by default and always will be
                animData.prioFrameKey[0] = true;
                animData.prioFrameKey[animData.totalFrameDuration - 1] = true;
            }
		}
		else
		{
			GUILayout.Label("Need GameObjectHierarchy");
		}
	}
}
