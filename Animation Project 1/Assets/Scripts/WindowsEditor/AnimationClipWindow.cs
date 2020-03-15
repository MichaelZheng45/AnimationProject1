using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationClipWindow : EditorWindow
{
    AnimationClip currentClip;

    [MenuItem("Window/CreateAnimationClip")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationClipWindow));
    }

    private void OnGUI()
    {
        currentClip = EditorGUILayout.ObjectField("AnimationClip", currentClip, typeof(AnimationClip), true) as AnimationClip;

        if(currentClip != null)
        {
            currentClip.animData = EditorGUILayout.ObjectField("AnimationData", currentClip.animData, typeof(AnimationDataHierarchal), true) as AnimationDataHierarchal;
            currentClip.startFrame = EditorGUILayout.IntField("Start Frame", currentClip.startFrame);
            currentClip.endFrame = EditorGUILayout.IntField("End Frame", currentClip.endFrame);
        }
    }
}
