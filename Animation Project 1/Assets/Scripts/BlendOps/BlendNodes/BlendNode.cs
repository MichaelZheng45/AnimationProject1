using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct blendPoseData
{
   transformData[] poseData;

   void setData(AnimationDataHierarchal animdata, int keyIndex)
    {
        int size = animdata.poseBase.Length;
        poseData = new transformData[size];
        for(int i = 0; i < size; i++)
        {
            KeyFrame key = animdata.poseBase[i].keyFrames[keyIndex];
            transformData newdata = new transformData(key.keyPosition, Quaternion.Euler(key.keyRotation), key.scale);
            poseData[i] = newdata;
        }
    }
}
public class BlendNode
{
    public BlendNode nodeOne;
    public BlendNode nodeTwo;

    public virtual blendPoseData blendOperation()
    {
        return new blendPoseData();
    }
}
