using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationDataHierarchal", menuName = "ScriptableObjects/AnimationDataHierarchal", order = 2)]
[System.Serializable]
public class AnimationDataHierarchal : AnimationData
{
    //eulerRotationOrder
    //calibrationUnits: mm is .001, cm is .01, dm = .1, m = 1
    public float calibrationUnit;
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
        //htr has same amount of keyframes as there is the total duration
        keyFrameCount = count;
        totalFrameDuration = count;

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
            calibrationUnit = 1f;
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

    public Matrix4x4 localBaseTransform;
    public Matrix4x4 globalBaseTransform;

    public Matrix4x4 currentTransform;

    public List<KeyFrame> keyFrames; //list of keyframes for this object

    public Vector3 getLocalPosition()
    {
        return localBaseTransform.GetColumn(3);
    }

    public Vector3 getLocalRotationEuler()
    {
        Quaternion rotation = Quaternion.LookRotation(localBaseTransform.GetColumn(2), localBaseTransform.GetColumn(1));
        return rotation.eulerAngles;
    }

    public Vector3 getCurrentPosition()
    {
        return currentTransform.GetColumn(3);
    }

    public Vector3 getCurrentRotationEuler()
    {
        Quaternion rotation = Quaternion.LookRotation(currentTransform.GetColumn(2), currentTransform.GetColumn(1));
        return rotation.eulerAngles;
    }

    public Quaternion getCurrentRotationQ()
    {
        Quaternion rotation = Quaternion.LookRotation(currentTransform.GetColumn(2), currentTransform.GetColumn(1));
        return rotation;
    }

    public void updateNewPosition(GameObject joint)
    {
        joint.transform.position = getCurrentPosition();
        joint.transform.rotation = getCurrentRotationQ();
    }
}
