using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClip : ScriptableObject
{
    public AnimationDataHierarchal animData;
    public int startFrame, endFrame;

    AnimationClip(int start, int end)
    {
        startFrame = start;
        endFrame = end;
    }

    public KeyFrame getcurrentFrame(int frameCount, int jointIndex)
    {
        //framecount is the current duration of a player, cannot be less than 0
        int currentFrame;
        if(frameCount > endFrame-startFrame)
        {
            currentFrame = endFrame;
        }
        else
        {
            currentFrame = startFrame + frameCount;
        }

        return animData.poseBase[jointIndex].keyFrames[currentFrame];
    }
}


