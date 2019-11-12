using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayerHierarchal : MonoBehaviour
{
    public AnimationDataHierarchal animData;

    public bool play = false;

    public int frameCount = 0;
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            //update to next frame
            timer += Time.deltaTime;
            if (timer >= animData.framePerSecond)
            {
                frameCount++;
                timer = 0;

                if (frameCount > animData.totalFrameDuration)
                {
                    frameCount = 0;
                }

                //update keyframes
                updateJoints();
            }

        }
    }

    void updateJoints()
    {
        //for each joint...
        for (int i = 0; i < animData.poseBase.Length; i++)
        {
            KeyFrame toKey;
            if (animData.keyFrameCount > frameCount)
            {
                //get next key
                toKey = animData.poseBase[i].keyFrames[frameCount];

                int parentIndex = animData.poseBase[i].parentNodeIndex;
                Vector3 localPosition = animData.poseBase[i].getLocalPosition();
                Vector3 localRotation = animData.poseBase[i].getLocalRotationEuler();

                //find delta change from localpose
                Matrix4x4 deltaMatrix = Matrix4x4.TRS(localPosition + toKey.keyPosition, Quaternion.Euler(localRotation + toKey.keyRotation), new Vector4(1, 1, 1, 1));

                if (parentIndex == -1)
                {
                    //is root
                    animData.poseBase[i].currentTransform = deltaMatrix;
                }
                else
                {
                    //current transform = take the parent index current transform and multiply with delta matrix
                    animData.poseBase[i].currentTransform = animData.poseBase[parentIndex].currentTransform * deltaMatrix;
                }

             //   animData.poseBase[i].updateNewPosition();
            }
        }
    }

}
