using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimationData : ScriptableObject
{
    public int totalFrameDuration;
    public int keyFrameCount;
    public float framePerSecond;

    //set how many seconds before next frame
    public virtual void setFramePerSecond(float count)
    {
        framePerSecond = 1.0f / count;
    }
}


//functions
//insert new keyframe in the order

//find current animation position/rotation using lerp and current duration

//find current animation position/rotation using lerp and number 0-1


