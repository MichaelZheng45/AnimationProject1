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

	bool[] updatePrioKeySet; //this is to keep check if there are anychanges to the actual priokey array
	int frameDuration = -1;

    int lineBasePosY = 100;
    int lineMaxPosY = 200;
    int lineMaxPosX = 400;
    int xShift = 35;

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
		gameObjectHierarchy = EditorGUILayout.ObjectField("GameObjectHierarchy", gameObjectHierarchy, typeof(gameObjectMain), true) as gameObjectMain;
		if (!animData)
		{
			GUILayout.Label("Need AnimationData");

		}
		else
		{
			createNewHierarchy = EditorGUILayout.Toggle("Create New Hierarchy", createNewHierarchy);
			if (createNewHierarchy)
			{
				newHierarchy();
			}

			modifyAnimationData();
			drawPrimaryKeys();
		}
	}

	void modifyAnimationData()
	{
		//update editor data for hierarchy
		if (updatePrioKeySet == null || updatePrioKeySet.Length != animData.prioFrameKey.Length)
			updatePrioKeySet = animData.prioFrameKey;

		if(frameDuration < 0)
		{
			frameDuration = animData.totalFrameDuration;
		}

		animData.framePerSecond = EditorGUILayout.FloatField("FrameRate(in seconds)", animData.framePerSecond);

        frameDuration = EditorGUILayout.IntField("Frame Duration", frameDuration);
        if(frameDuration != animData.keyFrameCount && GUILayout.Button("Set New Frame Count"))
        {
            //update by adding/removing all keyframes and then update
            int oldCount = animData.keyFrameCount;
            int newCount = frameDuration;
			animData.keyFrameCount = newCount; //update frameCount
			animData.totalFrameDuration = newCount;

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

			updatePrioKeySet = newPrioKeyList;
			animData.prioFrameKey = newPrioKeyList;

			//add or remove keyframes up to
			updateKeyList(newCount, oldCount);

			//if the recent primary key is the last one
			if (recentPrimaryKey == newCount - 1)
			{
				//do nothing because the frames are already set up
			}
			else
			{
				//lerp between the recent frame and last one
				lerpBetweenFrames(recentPrimaryKey, newCount - 1);
			}  
        }

        GUILayout.Space(120);
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
		
		if (currentKeyFrame < animData.totalFrameDuration - 1 && GUILayout.Button("Next Primary Frame"))
		{
			currentKeyFrame = findPrimaryKeyFrame(animData.totalFrameDuration-1, currentKeyFrame);
		}

		if (currentKeyFrame > 0 && GUILayout.Button("Previous Primary Frame"))
		{
			currentKeyFrame = findPrimaryKeyFrame(0, currentKeyFrame);
		}


		GUILayout.Space(20);
        GUILayout.Label("Pose options", EditorStyles.boldLabel);
        
        if(GUILayout.Button("Set to current pose"))
        {
			//function set pose, there will be a bool asking bind to current pose
			setPose(true);
        }

        if(GUILayout.Button("Set to base pose"))
        {
			//same function as above, there will be a bool asking bind to current pose
			setPose(false);
        }

        GUILayout.Space(20);
        GUILayout.Label("Current Key Frame", EditorStyles.boldLabel);

        //current key cannot be toggled if it is the first or last
        if(currentKeyFrame != 0 && currentKeyFrame < animData.totalFrameDuration-1)
        {
            animData.prioFrameKey[currentKeyFrame] = EditorGUILayout.Toggle("Primary Key", animData.prioFrameKey[currentKeyFrame]);
			//check on if it had been toggled if so update
			//find the two neighbor primaries
			//if toggled off lerp between the two, if toggled true lerp from to this, then lerp this to to

			//if the prioKey is now updated by checking if there is a change

			if(animData.prioFrameKey[currentKeyFrame] != updatePrioKeySet[currentKeyFrame])
			{
				bool currentCheck = animData.prioFrameKey[currentKeyFrame];
				updatePrioKeySet[currentKeyFrame] = currentCheck;
				//find the next and previous keyframes
				int previous = findPrimaryKeyFrame(0, currentKeyFrame);
				int next = findPrimaryKeyFrame(updatePrioKeySet.Length-1, currentKeyFrame);

				Debug.Log(currentCheck);
				//if true then it is a primary key, update previous to this and this to next
				if(currentCheck)
				{
					lerpBetweenFrames(previous, currentKeyFrame);
					lerpBetweenFrames(currentKeyFrame, next);
				}
				else
				{
					Debug.Log(previous);
					Debug.Log(next);
					//false then it is not a primary key anymore, update previous to next
					lerpBetweenFrames(previous, next);
				}
			}

		}
		else
        {
            GUILayout.Label("Primary Key: True");
			GUILayout.Label("Note: Cannot be toggled because it is either first or last frame");
		}

        //if it is primary key
        if(animData.prioFrameKey[currentKeyFrame])
        {
            if (GUILayout.Button("Set New Pose to Frame"))
            {
				for (int jointID = 0; jointID < animData.poseBase.Length; jointID++)
				{
					copyDownDataAndSet(jointID, currentKeyFrame);
				}

				int previous = findPrimaryKeyFrame(0, currentKeyFrame);
				int next = findPrimaryKeyFrame(updatePrioKeySet.Length-1, currentKeyFrame);

				lerpBetweenFrames(previous, currentKeyFrame);
				lerpBetweenFrames(currentKeyFrame, next);
			}
        }
        else
        {
			//it is a secondary key
			//just show data
			GUILayout.Label("Note: Cannot be edited");
		}

	}

	/*
	* Functions: Keyframes and priority keys
	* Purpose: These functions are to work on keyframe data and priority keys
	*	Names: findPrimaryKeyFrame, lerpBetweenFrames, copyDownDataAndSet, setPose
	*/

    //from is the starting frame (should be the first or last frame), to is the current key frame
    //find the closest primary keyframe, if from is smaller than to it will go up and find the one before if reversed it will search backwards and find the one after
    int findPrimaryKeyFrame(int from, int to)
    {
		int recentKey = from; //recently found key, starting from the first or last
        int updater = 1;
        if(from > to)
        {
            //from is greater than to meaning it needs to go backwards
            updater = -1;
        }


        //will stop once i is at to but if i is ever less than 0 or greater than the duration, stop
        for (int i = from; i != to && (i >= 0 && i < animData.totalFrameDuration); i += updater)
        {
			if(animData.prioFrameKey[i])
			{
				recentKey = i;
			}
        }

		return recentKey;
    }

	//take two primary pose frames and create lerped frames between them 
    void lerpBetweenFrames(int from, int to)
    {
        for (int i = from + 1; i < to; i++)
        {
			//find lerp parameter -> i /(to-from)
			float param = (float)(i-from) / (float)(to - from);

			for(int jointID = 0; jointID < animData.poseBase.Length; jointID++)
			{
				//set keyframe with from and to and lerp using parameter 
				animData.poseBase[jointID].keyFrames[i] = blendStatic.lerpKey(animData.poseBase[jointID].keyFrames[from], animData.poseBase[jointID].keyFrames[to], param, true);
			}
        }
    }

	//copies the transforms of the object and set it to a key frame number
	void copyDownDataAndSet(int jointID, int frameNumber)
	{
		//create new keyframe
		Transform poseObj = gameObjectHierarchy.getObject(jointID).transform;
		Vector3 localPosition = animData.poseBase[jointID].getLocalPosition();
		Vector3 localRotation = animData.poseBase[jointID].getLocalRotationEuler();

		KeyFrame newKey = new KeyFrame(poseObj.localPosition - localPosition, poseObj.localRotation, poseObj.localScale, animData.keyFrameCount - 1);

		animData.poseBase[jointID].keyFrames[frameNumber] = newKey;
	}
	
	//binds the objects in the object hierarchy to a pose, either current or base
	void setPose(bool boundToCurrent)
	{
		for(int jointID = 0; jointID < animData.poseBase.Length; jointID++)
		{
			animationTransformData tData = new animationTransformData(0);
			if (boundToCurrent)
			{
				KeyFrame currentKey = animData.poseBase[jointID].keyFrames[currentKeyFrame];
				tData.setTransformIndividual(currentKey.keyPosition, currentKey.keyQRotation,currentKey.scale);
				
			}

			ForwardKinematics.setData(gameObjectHierarchy, animData, tData, jointID);
		}
	}

	/*
	* Functions: Hierarchy and Object Structures
	* Purpose: These functions are to work on the hierarchy and its class other uses include modifying keyframe class but not the data entirely
	*	Names: updateKeyList, copydownDataAndAdd, checkChildren, newHierarchy
	*/

	//remove or add keys until oldsize is now currentsize
	void updateKeyList(int currentSize, int oldSize)
	{
		int diff = Mathf.Abs(currentSize - oldSize);
		
		//Do this diff amount, based on new size, add or remove frames on each joint 
		for(int i = 0; i < diff; i++)
		{
			for (int jointID = 0; jointID < animData.poseBase.Length; jointID++)
			{
				if (currentSize > oldSize)
				{
					animData.poseBase[jointID].keyFrames.Add(new KeyFrame());
				}
				else
				{
					animData.poseBase[jointID].keyFrames.RemoveAt(oldSize - 1 - i);
				}
			}
		}
	}
	
	//writes down data of the current transform of the gameobjects
	void copyDownDataAndAdd(int jointID)
    {
        //create new keyframe
        Transform poseObj = gameObjectHierarchy.getObject(jointID).transform;
        Vector3 localPosition = animData.poseBase[jointID].getLocalPosition();
        Vector3 localRotation = animData.poseBase[jointID].getLocalRotationEuler();

        KeyFrame newKey = new KeyFrame(poseObj.localPosition - localPosition, poseObj.rotation, poseObj.localScale, animData.keyFrameCount - 1);
       

        animData.poseBase[jointID].keyFrames.Add(newKey);

        // animData.poseBase[jointID].keyFrames[frameNumber] = newKey;
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

                //remove old poses and start updating heirarchy
                animData.deletePoses();
				gameObjectHierarchy.newList();
				createNewHierarchy = false;

				int rootIndex = gameObjectHierarchy.addObjectWithIndex(rootObject, -1);
				animData.addNewPose(rootObject, rootObject, -1);
				checkChildren(rootObject, rootIndex);

				frameDuration = animData.totalFrameDuration;

				//update all old frames and update new ones
				animData.prioFrameKey = new bool[animData.totalFrameDuration];
                for (int i = 0; i < animData.totalFrameDuration; i++)
                {
                    animData.prioFrameKey[i] = false;

                    //reason to go throught each pose is because we want to fill the frames with dummy data
                    for (int j = 0; j < animData.poseBase.Length; j++)
                    {
                        copyDownDataAndAdd(j);
                    }

                }
                //the last and first keys are primary keys by default and always will be
                animData.prioFrameKey[0] = true;
                animData.prioFrameKey[animData.totalFrameDuration - 1] = true;

				updatePrioKeySet = animData.prioFrameKey;
            }
		}
		else
		{
			GUILayout.Label("Need GameObjectHierarchy");
		}
	}

	/*
	* Functions: Editor GUI
	* Purpose: These functions are to work on the editor and any gui related stuff
	*	Names: drawPrimaryKeys
	*/

	//for every primary keys draw lines in box 
	void drawPrimaryKeys()
	{
		int totalDuration = updatePrioKeySet.Length;
		float xPos;
		Vector3 startPosition;
		Vector3 endPosition;

		for (int i = 0; i < totalDuration; i++)
		{
			if (updatePrioKeySet[i])
			{
				xPos = (lineMaxPosX) * ((float)i / (float)totalDuration) + xShift;
				startPosition = new Vector3(xPos, lineBasePosY);
				endPosition = new Vector3(xPos, lineMaxPosY);

				Handles.BeginGUI();
				Handles.color = Color.red;
				Handles.DrawLine(startPosition, endPosition);
				Handles.EndGUI();
			}
		}

		xPos = (lineMaxPosX) * ((float)currentKeyFrame / (float)totalDuration) + xShift;
		startPosition = new Vector3(xPos, lineBasePosY);
		endPosition = new Vector3(xPos, lineMaxPosY);

		Handles.BeginGUI();
		Handles.color = Color.green;
		Handles.DrawLine(startPosition, endPosition);
		Handles.EndGUI();
	}



}
