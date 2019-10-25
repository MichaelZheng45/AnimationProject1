using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlend : MonoBehaviour
{
    protected Transform poseresult;
    public bool usingQuaternion = true;

    // Start is called before the first frame update
    void Start()
    {
        poseresult = this.gameObject.transform;
    }
}

public class identity
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public identity()
    {
        position = Vector3.zero;
        scale = Vector3.zero;
        rotation = Quaternion.identity;
    }
}
