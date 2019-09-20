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

    public AnimationData animData;
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
		Rect rectangle = new Rect(new Vector2(0, lineBasePosY), new Vector2(lineMaxPosX, lineMaxPosY-lineBasePosY));
		EditorGUI.DrawRect(rectangle, Color.gray);
		animData = EditorGUILayout.ObjectField("AnimationData", animData, typeof(AnimationData), true) as AnimationData;
        animObject = EditorGUILayout.ObjectField("AnimationObject", animData, typeof(GameObject), true) as GameObject;

        if (animData != null && animObject != null)
        {
            if (animData.keyFrames == null && animObject != null)
            {
                animData.keyFrames = new List<KeyFrame>();
                animData.keyFrames.Add(new KeyFrame(animObject.transform.position));
                currentKeyframe = 0;
            }

            GUILayout.Label("Key Frame Count: " +  animData.keyFrames.Count, EditorStyles.miniLabel);
			animData.totalFrame = EditorGUILayout.IntField("Frame Duration: ",animData.totalFrame);

			//create new key frame button and set current keyframe to it
			//draw out a visual representation
			//for loop for each keyframe, then draw another line for current key frame but with different color
			foreach(KeyFrame keyF in animData.keyFrames)
			{
                drawLine(keyF, animData.totalFrame, Color.red);
			}

            //draw current keyframe line
            drawLine(animData.keyFrames[currentKeyframe], animData.totalFrame, Color.green);
			GUILayout.Space(200);

			//display current keyframe stats, duration cannot be more than max "TO DO"

            //sort the list
			

            //add new key
            //add it to the begninning
            //set current key to that
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

    //sort list
    void sortList()
    {

    }

    void drawLine(KeyFrame keyF, int totalDuration, Color colorL)
    {
        //line pos = maxPos * (keyduration/maxDuration) 
        float xPos = lineMaxPosX * (keyF.atFrame / totalDuration);

        Vector3 startPosition = new Vector3(xPos,lineBasePosY);
        Vector3 endPosition = new Vector3(xPos,lineMaxPosY);

        Handles.BeginGUI();
        Handles.color = colorL;
        Handles.DrawLine(startPosition, endPosition);
        Handles.EndGUI();
    }
}
