using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlendHierarchal : MonoBehaviour
{
    public AnimationDataHierarchal animData;
    public bool usingQuaternion = true;
    public testBlendMode blendMode;

    [Range(0, 1)]
    public float parameter0, parameter1;
    public int keyframe0, keyframe1;

    public gameObjectMain gameObjectData;

    private void Update()
    {
        for(int i = 0; i < animData.poseBase.Length; i++)
        {
            blendTransformData poseresult = new blendTransformData();
            blendTransformData dataPose0 = new blendTransformData();
            blendTransformData dataPose1 = new blendTransformData();
            KeyFrame key0 = animData.poseBase[i].keyFrames[keyframe0];
            KeyFrame key1 = animData.poseBase[i].keyFrames[keyframe1];
            dataPose0.setTransformIndividual(key0.keyPosition, Quaternion.Euler(key0.keyRotation), new Vector3(1, 1, 1));
            dataPose1.setTransformIndividual(key1.keyPosition, Quaternion.Euler(key1.keyRotation), new Vector3(1, 1, 1));
            switch (blendMode)
            {
                case testBlendMode.LERP:
                    poseresult = blendStatic.lerp(dataPose0, dataPose1, parameter0, usingQuaternion);
                    break;
                case testBlendMode.ADD:
                    poseresult = blendStatic.add(dataPose0, dataPose1, usingQuaternion);
                    break;
                case testBlendMode.SCALE:
                    poseresult = blendStatic.scale(new identity(), dataPose0, parameter0, usingQuaternion);
                    break;
                case testBlendMode.AVERAGE:
                    poseresult = blendStatic.average(new identity(), dataPose0, dataPose1, parameter0, parameter1, usingQuaternion);
                    break;
            }

            int parentIndex = animData.poseBase[i].parentNodeIndex;
            Vector3 localPosition = animData.poseBase[i].getLocalPosition();
            Vector3 localRotation = animData.poseBase[i].getLocalRotationEuler();

            //find delta change from localpose
            Matrix4x4 deltaMatrix = Matrix4x4.TRS(localPosition + poseresult.localPosition, Quaternion.Euler(localRotation + poseresult.localRotation.eulerAngles), new Vector4(1, 1, 1, 1));

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

            animData.poseBase[i].updateNewPosition(gameObjectData.getObject(i));
        
        }


    }
}
