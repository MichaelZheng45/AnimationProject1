using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyFrame
{
    public Vector3 keyPosition;
    public Vector3 keyRotation;
    public Vector3 scale;
	public int atFrame;

    public KeyFrame()
    {
        atFrame = 0;
        keyPosition = Vector3.zero;
    }

	public KeyFrame(Vector3 newPosition, Vector3 newRotation, Vector3 newScale, int frame)
	{
		keyPosition = newPosition;
		keyRotation = newRotation;
		scale = newScale;
		atFrame = frame;
	}

    public KeyFrame(Vector3 newPosition)
    {
        atFrame = 0;
        keyPosition = newPosition;
    }

    public KeyFrame(int frame)
    {
        atFrame = frame;
    }
}
