using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationDataHierarchal", menuName = "ScriptableObjects/AnimationDataHierarchal", order = 2)]
[System.Serializable]
public class AnimationDataHierarchal : AnimationData
{
    //eulerRotationOrder
    //calibrationUnits: mm is .001, cm is .01, dm = .1, m = 1
    float calibrationUnit;
    //RotationUnits
    //globalAxisofGravity
    //Bone lengthAxis: default y
    public float scaleFactor;

    //poseData
    //basePose -> contains poseNode[] each poseNode has-> string name, parentPoseNode index, bone length,
    public poseNode[] poseBase;

    public void createBase(int count)
    {
        poseBase = new poseNode[count];
        for(int i = 0; i < count; i++)
        {
            poseBase[i] = new poseNode();
        }
    }

    public void generateFrames(int count)
    {
        for(int i = 0; i < poseBase.Length; i++)
        {
            poseBase[i].keyFrames = new List<KeyFrame>();
            for(int j = 0; j < count; j++)
            {
                poseBase[i].keyFrames.Add(new KeyFrame(j));
            }
        }
    }

    public void setCalibrationUnit(string unit)
    {
        if (unit == "mm")
        {
            calibrationUnit = .001f;
        }
        else if (unit == "cm")
        {
            calibrationUnit = .01f;
        }
        else if (unit == "dm")
        {
            calibrationUnit = .1f;
        }
        else if (unit == "m")
        {
            calibrationUnit = 1f;
        }
    }
}

[System.Serializable]
public class poseNode
{
    public string name;
    public int parentNodeIndex;
    public float boneLength;

    public Vector3 basePosition;
    public Vector3 baseRotation;
    public float scale;

    public List<KeyFrame> keyFrames; //list of keyframes for this object
}
