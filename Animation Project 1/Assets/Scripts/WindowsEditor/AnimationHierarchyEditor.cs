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
            int oldCount = animData.keyFrameCount;
            int newCount = animData.totalFrameDuration;

            bool[] newPrioKeyList = new bool[newCount];
            int recentPrimaryKey = 0;

            for(int i = 0; i < newCount; i++)
            {
                //if the new size is exceeding the old count
                if(i >= oldCount)
                {
                    newPrioKeyList[i] = false;

                }
                else
                {
                    newPrioKeyList[i] = animData.prioFrameKey[i];
                }

                if(newPrioKeyList[i])
                {
                    recentPrimaryKey = i;
                }
            }
            //set last key to true
            newPrioKeyList[newCount - 1] = true;

            //if the recent primary key is the last one
            if (recentPrimaryKey == newCount-1)
            {
                //do nothing because the frames are already set up
            }
            else
            {
                //lerp between the recent frame and last one
                lerpBetweenFrames(recentPrimaryKey,newCount-1);
            }
        
        }

        GUILayout.Space(20);
        GUILayout.Label("Key Frame Options", EditorStyles.boldLabel);
        currentKeyFrame = EditorGUILayout.IntField("Current Key Frame", currentKeyFrame);
        
        //bound the currentKeyframe within the limits
        if (currentKeyFrame >= animData.totalFrameDuration)
        {
            currentKeyFrame = animData.totalFrameDuration - 1;
        }
        else if(currentKeyFrame < 0 )
        {
            currentKeyFrame = 0;
        }

        GUILayout.Space(20);
        GUILayout.Label("Pose options", EditorStyles.boldLabel);
        
        if(GUILayout.Button("Set to current pose"))
        {

        }

        if(GUILayout.Button("Set to base pose"))
        {

        }

        GUILayout.Space(20);
        GUILayout.Label("Current Key Frame", EditorStyles.boldLabel);

        //current key cannot be toggled if it is the first or last
        if(currentKeyFrame != 0 && currentKeyFrame < animData.totalFrameDuration-1)
        {
            animData.prioFrameKey[currentKeyFrame] = EditorGUILayout.Toggle("Primary Key", animData.prioFrameKey[currentKeyFrame]);
        }
        else
        {
            GUILayout.Label("Primary Key: True");
        }
     
        
        //check on if it had been toggled if so update
        //find the two neighbor primaries
        //if toggled off lerp between the two, if toggled true lerp from to this, then lerp this to to

        //if it is primary key
        if(animData.prioFrameKey[currentKeyFrame])
        {
            if (GUILayout.Button("Set New Pose to Frame"))
            {

            }
        }
        else
        {
            //it is a secondary key
            //just show data
        }

	}

    //from is the starting frame (should be the first or last frame), to is the current key frame
    //find the closest primary keyframe, if from is smaller than to it will go up and find the one before if reversed it will search backwards and find the one after
    void findPrimaryKeyFrame(int from, int to)
    {
        int updater = 1;
        if(from > to)
        {
            //from is greater than to meaning it needs to go backwards
            updater = -1;
        }


        //will stop once i is at to but if i is ever less than 0 or greater than the duration, stop
        for (int i = from; i != to && (i >= 0 && i < animData.totalFrameDuration-1); i += updater)
        {

        }
    }

    void lerpBetweenFrames(int from, int to)
    {
        for (int i = from + 1; i < to; i++)
        {

        }
    }

    //writes down data of the current transform of the gameobjects
    void copydownData(int jointID, int frameNumber, bool add)
    {
        //create new keyframe
        Transform poseObj = gameObjectHierarchy.getObject(jointID).transform;
        Vector3 localPosition = animData.poseBase[jointID].getLocalPosition();
        Vector3 localRotation = animData.poseBase[jointID].getLocalRotationEuler();

        KeyFrame newKey = new KeyFrame(poseObj.localPosition - localPosition, poseObj.localEulerAngles - localRotation, poseObj.localScale, animData.keyFrameCount - 1);
        if(add)
        {
            animData.poseBase[jointID].keyFrames.Add(newKey);
        }
        else
        {
            animData.poseBase[jointID].keyFrames[frameNumber] = newKey;
        }

    }


    //recursive call to checkchild of the current game object used for generating a new hierarchy
	void checkChildren(GameObject currentObject, int currentIndex)
	{
		foreach(Transform child in currentObject.transform)
		{
			int childIndex = gameObjectHierarchy.addObjectWithIndex(child.gameObject, currentIndex);
			animData.addNewPose(child.gameObject,currentObject, currentIndex);
			checkChildren(child.gameObject, childIndex);
		}
	}

    //generate a new hierarchy data with a root heirarchy and object heirarchy
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

                    //reason to go throught each pose is because we want to fill the frames with dummy data
                    for (int j = 0; j < animData.poseBase.Length; j++)
                    {
                        copydownData(j,i, true);
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
