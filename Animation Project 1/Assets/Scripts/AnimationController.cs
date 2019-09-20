using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "ScriptableObjects/AnimationData", order = 1)]
public class AnimationData : ScriptableObject
{
    public List<KeyFrame> keyFrames;
    public int totalFrame;

    public int currentKeyFrame;
    public float currentDuration;
}

//functions
//insert new keyframe in the order

//find current animation position/rotation using lerp and current duration

//find current animation position/rotation using lerp and number 0-1


