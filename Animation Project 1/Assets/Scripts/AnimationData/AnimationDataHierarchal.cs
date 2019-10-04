using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDataHierarchal : MonoBehaviour
{
    string name;
    int segmentNumber;
    //eulerRotationOrder
    //calibrationUnits: mm is .001, cm is .01, dm = .1, m = 1
    //RotationUnits
    //globalAxisofGravity
    //Bone lengthAxis: default y
    int scaleFactor;

    //poseData
    //basePose -> contains poseNode[] each poseNode has-> string name, parentPoseNode index, bone length,
    //keyframePoses is a list of class keyFramePose -> contains: keyframe list, name, 
}
