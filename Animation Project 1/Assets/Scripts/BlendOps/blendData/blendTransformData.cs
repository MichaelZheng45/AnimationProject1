using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct blendTransformData
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;

    public blendTransformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        localPosition = position;
        localRotation = rotation;
        localScale = scale;
    }

    public Vector3 localEulerAngles()
    {
        return localRotation.eulerAngles;
    }

    public void localEulerAnglesSet(Vector3 v)
    {
        localRotation = Quaternion.Euler(v);
    }

    public void changeTransform(Transform thisTransform)
    {
        thisTransform.localPosition = localPosition;
        thisTransform.localRotation = localRotation;
        thisTransform.localScale = localScale;
    }

    public void setTransform(Transform thisTransform)
    {
        localPosition = thisTransform.localPosition;
        localRotation = thisTransform.localRotation;
        localScale = thisTransform.localScale;
    }

    public void setTransformIndividual(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        localPosition = position;
        localRotation = rotation;
        localScale = scale;
    }
}
