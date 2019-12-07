using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum testBlendMode
{
    LERP,
    ADD,
    SCALE,
    AVERAGE
}
public class TestBlend : MonoBehaviour
{

    public bool usingQuaternion = true;
    public testBlendMode blendMode;

    [Range(0, 1)]
    public float parameter0, parameter1;
    public Transform pose0, pose1;
    animationTransformData dataPose0, dataPose1;
    animationTransformData poseresult;
    // Start is called before the first frame update
    void Start()
    {
        dataPose0 = new animationTransformData();
        dataPose1 = new animationTransformData();
        dataPose0.setTransform(pose0);
        dataPose1.setTransform(pose1);
    }

    private void Update()
    {
      switch(blendMode)
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
        poseresult.changeTransform(this.transform);
    }
}


