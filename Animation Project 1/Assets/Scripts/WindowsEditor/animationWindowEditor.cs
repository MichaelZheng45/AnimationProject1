using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class animationWindowEditor : EditorWindow
{
	//line variables
	int lineBasePosY = 200;
	int lineMaxPosY =300;
	int lineMaxPosX = 400;
    int xShift = 50;
    public AnimationDataSingle animData;
    public GameObject animObject;
	int currentKeyframe;

    [MenuItem("Window/AnimationHandler")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(animationWindowEditor));
    }

    private void Update()
	{
		//if there is list, sort it 

	}

    private void OnGUI()
    {
        Rect rectangle = new Rect(new Vector2(xShift, lineBasePosY), new Vector2(lineMaxPosX, lineMaxPosY-lineBasePosY));
		EditorGUI.DrawRect(rectangle, Color.gray);

        animData = EditorGUILayout.ObjectField("AnimationDataSingle", animData, typeof(AnimationDataSingle), true) as AnimationDataSingle;
        animObject = EditorGUILayout.ObjectField("AnimationObject", animObject, typeof(GameObject), true) as GameObject;

        if (animData != null && animObject != null)
        {
            //set data  
            if (animData.keyFrames == null||animData.keyFrames.Count == 0)
            {
                animData.totalFrameDuration = 1;
                animData.keyFrames = new List<KeyFrame>();
                animData.keyFrames.Add(new KeyFrame(animObject.transform.position));
                currentKeyframe = 0;
            }
            int keyCount = animData.keyFrames.Count;

            GUILayout.Label("Animation Data Base Setting", EditorStyles.boldLabel);
            GUILayout.Label("Key Frame Count: " +  keyCount, EditorStyles.miniLabel);
			animData.totalFrameDuration = EditorGUILayout.IntField("Frame Duration: ",animData.totalFrameDuration);
            int tempCurrentFrame = EditorGUILayout.IntField("Current Key: ", currentKeyframe + 1);
            currentKeyframe = tempCurrentFrame - 1;
            if(currentKeyframe > keyCount)
            {
                currentKeyframe = keyCount;
            }
            else if(currentKeyframe < 0)
            {
                currentKeyframe = 0;
            }

            //create new key frame button and set current keyframe to it
            //draw out a visual representation
            //for loop for each keyframe, then draw another line for current key frame but with different color
            foreach (KeyFrame pKeyF in animData.keyFrames)
			{
                drawLine(pKeyF, animData.totalFrameDuration, Color.red);
			}
            //draw current keyframe line
            drawLine(animData.keyFrames[currentKeyframe], animData.totalFrameDuration, Color.green);

            GUILayout.Space(200);

            //display current keyframe stats, duration cannot be more than max "TO DO"
            GUILayout.Label("Current Key Settings", EditorStyles.boldLabel);
            KeyFrame keyF = animData.keyFrames[currentKeyframe];
            int atFrame = keyF.atFrame;
            atFrame = EditorGUILayout.IntField("At Frame: ", atFrame);
            if(atFrame > animData.totalFrameDuration)
            {
                atFrame = animData.totalFrameDuration;
            }
            else if(atFrame < 0)
            {
                atFrame = 0;
            }
            keyF.atFrame = atFrame;

            GUILayout.Label("Key Data", EditorStyles.miniLabel);
            keyF.keyPosition = EditorGUILayout.Vector3Field("Position: ", keyF.keyPosition);
            
            if(GUILayout.Button("Set new transform"))
            {
                keyF.keyPosition = animObject.transform.position;
            }

            //sort the list
            sortList();

            GUILayout.Space(50);
            GUILayout.Label("Add/Delete Keys", EditorStyles.boldLabel);
            if (GUILayout.Button("Add new key"))
            {
                //add new key
                //add it to the begninning
                //set current key to that
                animData.keyFrames.Insert(0, new KeyFrame(animObject.transform.position));
                currentKeyframe = 0;
            }

            //delete currentKey
            if(GUILayout.Button("Delete Current Key"))
            {
                //delete current key
                //set currentindex to 0
                animData.keyFrames.RemoveAt(currentKeyframe);
                currentKeyframe = 0;
            }
        }

    }

    //sort list
    void sortList()
    {
        List<KeyFrame> frameList = animData.keyFrames;
        KeyFrame currentKey = animData.keyFrames[currentKeyframe];


        for (int i = 1; i < frameList.Count; i++)
        {
            KeyFrame tempKey = frameList[i];
            bool placed = false;
            int index = i;
            while (!placed && index >= 0)
            {
                if(frameList[index].atFrame > frameList[index - 1 ].atFrame)
                {
                    frameList.RemoveAt(i);
                    frameList.Insert(index, tempKey);
                    placed = true;
                }

                index--;
                if(index == 0 && !placed)
                {
                    frameList.RemoveAt(i);
                    frameList.Insert(0, tempKey);
                    placed = true;
                }
            }
            animData.keyFrames = frameList;
            currentKeyframe = animData.keyFrames.IndexOf(currentKey);
        }
    }

    void drawLine(KeyFrame keyF, int totalDuration, Color colorL)
    {
        //line pos = maxPos * (keyduration/maxDuration) 
        float xPos = (lineMaxPosX) * ((float)keyF.atFrame / (float)totalDuration) + xShift;
        Vector3 startPosition = new Vector3(xPos, lineBasePosY);
        Vector3 endPosition = new Vector3(xPos,lineMaxPosY);

        Handles.BeginGUI();
        Handles.color = colorL;
        Handles.DrawLine(startPosition, endPosition);
        Handles.EndGUI();
    }
}

