using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct animationTransformData
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;

	public animationTransformData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		localPosition = position;
		localRotation = rotation;
		localScale = scale;
	}
	public animationTransformData(int value = 0)
    {
		localPosition = Vector3.zero;
		localRotation = Quaternion.identity;
		localScale = new Vector3(1, 1, 1);
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
