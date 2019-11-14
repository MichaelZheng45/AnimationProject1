using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct blendPoseData
{
    public blendTransformData[] poseData;
    public int size;
    public blendTransformData getPoseData(int index)
    {
        return poseData[index];
    }

    public void setPoseData(blendTransformData data,int index)
    {
        poseData[index] = data;
    }

    public void setData(AnimationDataHierarchal animdata, int keyIndex)
    {
        size = animdata.poseBase.Length;
        poseData = new blendTransformData[size];
        for (int i = 0; i < size; i++)
        {
            KeyFrame key = animdata.poseBase[i].keyFrames[keyIndex];
            blendTransformData newdata = new blendTransformData(key.keyPosition, Quaternion.Euler(key.keyRotation), key.scale);
            poseData[i] = newdata;
        }
    }

    public void setData(AnimationClip clip, int keyIndex)
    {
        size = clip.animData.poseBase.Length;
        poseData = new blendTransformData[size];
        for(int i = 0; i <size;i++)
        {
            KeyFrame key = clip.getcurrentFrame(keyIndex, i);
            blendTransformData newdata = new blendTransformData(key.keyPosition, Quaternion.Euler(key.keyRotation), key.scale);
            poseData[i] = newdata;
        }
    }
}
