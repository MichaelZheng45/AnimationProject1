using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendingTreePlayer : MonoBehaviour
{
    public gameObjectMain animationObjectHierData;
	public AnimationDataHierarchal animData; //just need to contain the hierarchy data (base pose)
    public BlendingTree blendTreeData;
    public bool play;

    int currentKeyFrame = 0;
    int maxFrame = 20;
    // Update is called once per frame
    void Update()
    {
        int jointCount = animationObjectHierData.ObjectHierarchy.Count;
        currentKeyFrame++;
        if(currentKeyFrame > maxFrame)
        {
            currentKeyFrame = 0;
        }

        blendPoseData poseDataResult = blendTreeData.useBlendTree(blendTreeData,currentKeyFrame);  

        for(int i = 0; i < jointCount; i++)
        {
            ForwardKinematics.setData(animationObjectHierData, animData ,poseDataResult.getPoseData(i), i);
        }
    }
}
