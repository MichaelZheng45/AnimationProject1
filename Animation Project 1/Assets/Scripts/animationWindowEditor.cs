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
    public AnimationController animController;
	KeyFrame currentKeyframe;
    [MenuItem("Window/AnimationHandler")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(animationWindowEditor));
    }

    private void Update()
	{
		//if there is list, sort it 
		if (animController.keyFrames == null)
		{
			animController.keyFrames = new List<KeyFrame>();
		}

	}

    private void OnGUI()
    {
		Rect rectangle = new Rect(new Vector2(0, lineBasePosY), new Vector2(lineMaxPosX, lineMaxPosY-lineBasePosY));
		EditorGUI.DrawRect(rectangle, Color.gray);
		animController = EditorGUILayout.ObjectField("AnimationController", animController, typeof(AnimationController), true) as AnimationController;

        if(animController != null)
        {
            GUILayout.Label("Key Frame Count: " +  animController.keyFrames.Count, EditorStyles.miniLabel);
			animController.totalFrame = EditorGUILayout.IntField("Frame Duration: ",animController.totalFrame);

			//create new key frame button and set current keyframe to it
			//draw out a visual representation
			//for loop for each keyframe, then draw another line for current key frame but with different color
			foreach(KeyFrame keyF in animController.keyFrames)
			{
				Handles.BeginGUI();
				Handles.color = Color.red;
				Handles.DrawLine(new Vector3(0, 0), new Vector3(300, 300));
				Handles.EndGUI();
			}

			GUILayout.Space(200);
			if(animController.keyFrames.Count > 0 && currentKeyframe != null)
			{
				//display current keyframe stats
			}
			else
			{
				currentKeyframe = animController.keyFrames[0];
			}

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
