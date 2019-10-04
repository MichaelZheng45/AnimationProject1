using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "ScriptableObjects/AnimationData", order = 1)]
[System.Serializable]
public abstract class AnimationData : ScriptableObject
{
    public List<KeyFrame> keyFrames; //will be keyframePoses->single has one, hierarchy has multiple
    public int totalFrameDuration;
    public int keyFrameCount;
    public int framePerSecond;
}

//functions
//insert new keyframe in the order

//find current animation position/rotation using lerp and current duration

//find current animation position/rotation using lerp and number 0-1


