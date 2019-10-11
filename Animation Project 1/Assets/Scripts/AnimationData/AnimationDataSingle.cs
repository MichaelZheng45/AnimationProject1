using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationDataSingle", menuName = "ScriptableObjects/AnimationDataSingle", order = 1)]
[System.Serializable]
public class AnimationDataSingle : AnimationData
{
    //keyframePoses is a list of class keyFramePose -> contains: keyframe list, name,  but it will be a single so keyframePoses.count = 1

}
