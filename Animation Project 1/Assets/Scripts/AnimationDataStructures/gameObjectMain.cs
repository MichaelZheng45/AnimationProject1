using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameObjectMain : MonoBehaviour
{
    public AnimationDataHierarchal animData;
    public List<GameObject> ObjectHierarchy;

    public void newList()
    {
        ObjectHierarchy = new List<GameObject>();
    }
    public void addObject(GameObject newObject)
    {
        ObjectHierarchy.Add(newObject);
    }

    public GameObject getObject(int index)
    {
        return ObjectHierarchy[index];
    }

    public void setNewData(blendTransformData poseresult, int i)
    {
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

        animData.poseBase[i].updateNewPosition(ObjectHierarchy[i]);
    }
}
