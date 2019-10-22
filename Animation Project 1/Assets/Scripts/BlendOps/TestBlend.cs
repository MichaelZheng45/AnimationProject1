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
