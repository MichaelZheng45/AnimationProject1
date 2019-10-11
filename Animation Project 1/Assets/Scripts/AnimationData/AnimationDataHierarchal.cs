using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationDataHierarchal", menuName = "ScriptableObjects/AnimationDataHierarchal", order = 2)]
[System.Serializable]
public class AnimationDataHierarchal : AnimationData
{
    public int segmentNumber;
    //eulerRotationOrder
    //calibrationUnits: mm is .001, cm is .01, dm = .1, m = 1
    //RotationUnits
    //globalAxisofGravity
    //Bone lengthAxis: default y
    public int scaleFactor;

    //poseData
    //basePose -> contains poseNode[] each poseNode has-> string name, parentPoseNode index, bone length,
    public poseNode[] poseBase;

    public void createBase(int count)
    {
        poseBase = new poseNode[count];
        keyFrames = new List<KeyFrame>();
        for(int i = 0; i < count; i++)
        {
            keyFrames.Add(new KeyFrame());
        }
    }
}

public class poseNode
{
    public string name;
    public int parentNodeIndex;
    public float boneLength;

    public Vector3 basePosition;
    public Vector3 keyRotation;
    public float scale;
}
