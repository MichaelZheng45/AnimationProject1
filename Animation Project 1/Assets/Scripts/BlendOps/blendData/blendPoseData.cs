using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct blendPoseData
{
    public animationTransformData[] poseData;
    public int size;
    public animationTransformData getPoseData(int index)
    {
        return poseData[index];
    }

    public void setPoseData(animationTransformData data,int index)
    {
        poseData[index] = data;
    }

    public void setData(AnimationDataHierarchal animdata, int keyIndex)
    {
        size = animdata.poseBase.Length;
        poseData = new animationTransformData[size];
        for (int i = 0; i < size; i++)
        {
            KeyFrame key = animdata.poseBase[i].keyFrames[keyIndex];
            animationTransformData newdata = new animationTransformData(key.keyPosition, Quaternion.Euler(key.keyRotation), key.scale);
            poseData[i] = newdata;
        }
    }

    public void setData(AnimationClip clip, int keyIndex)
    {
        size = clip.animData.poseBase.Length;
        poseData = new animationTransformData[size];
        for(int i = 0; i <size;i++)
        {
            KeyFrame key = clip.getcurrentFrame(keyIndex, i);
            animationTransformData newdata = new animationTransformData(key.keyPosition, Quaternion.Euler(key.keyRotation), key.scale);
            poseData[i] = newdata;
        }
    }
}
