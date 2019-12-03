using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
	enum MovementStance
	{
		IDLE,
		WALKING,
		RUNNING
	}
    // Start is called before the first frame update

    public gameObjectMain objectHierarchy;
	public AnimationDataHierarchal walkingAnim;
	public AnimationDataHierarchal idleAnim;
	public AnimationDataHierarchal runningAnim;

	bool idle = false;
	bool sprint = false;

	float timer = 0;
	int currentFrame = 0;
	MovementStance currentStance;
	float transitionParameter;
	bool lerpin = false;
	AnimationDataHierarchal prevAnim;
	AnimationDataHierarchal currentAnim;
	void Start()
    {
		currentStance = 0;
		currentAnim = idleAnim;
		prevAnim = idleAnim;
    }

    // Update is called once per frame
    void Update()
    {
		sprint = Input.GetKey(KeyCode.LeftShift);
		idle = Input.GetKey(KeyCode.W);

		//check if different input,
		MovementStance updatedStance;
		if(!idle)
		{
			if(sprint)
			{
				updatedStance = MovementStance.RUNNING;
			}
			else
			{
				updatedStance = MovementStance.WALKING;
			}
		}
		else
		{
			updatedStance = MovementStance.IDLE;
		}

		if(updatedStance != currentStance)
		{
			transitionParameter = 0;
			lerpin = true;
			prevAnim = setAnimData(currentStance);
			currentAnim = setAnimData(updatedStance);
			currentStance = updatedStance;
		}

		//update framecount;
		//update to next frame
		timer += Time.deltaTime;
		if (timer >= currentAnim.framePerSecond)
		{
			currentFrame++;
			timer = 0;

			if (currentFrame > currentAnim.totalFrameDuration)
			{
				currentFrame = 0;
			}

		}

		if (lerpin)
		{
			transitionParameter += .1f;
			if (transitionParameter >= 1)
			{
				transitionParameter = 1;
				lerpin = false;
			}
		}
		else
		{
			transitionParameter = 1;
		}

		blendAnimation();
	}

	void blendAnimation()
	{
		bool usingQuaternion = true;
		for (int i = 0; i < currentAnim.poseBase.Length; i++)
		{
			blendTransformData poseresult = new blendTransformData();
			blendTransformData dataPose0 = new blendTransformData();
			blendTransformData dataPose1 = new blendTransformData();
			KeyFrame key0 = prevAnim.poseBase[i].keyFrames[currentFrame];
			KeyFrame key1 = currentAnim.poseBase[i].keyFrames[currentFrame];
			dataPose0.setTransformIndividual(key0.keyPosition, Quaternion.Euler(key0.keyRotation), new Vector3(1, 1, 1));
			dataPose1.setTransformIndividual(key1.keyPosition, Quaternion.Euler(key1.keyRotation), new Vector3(1, 1, 1));

			poseresult = blendStatic.lerp(dataPose0, dataPose1, transitionParameter, usingQuaternion);

			int parentIndex = currentAnim.poseBase[i].parentNodeIndex;
			Vector3 localPosition = currentAnim.poseBase[i].getLocalPosition();
			Vector3 localRotation = currentAnim.poseBase[i].getLocalRotationEuler();

			//find delta change from localpose
			Matrix4x4 deltaMatrix = Matrix4x4.TRS(localPosition + poseresult.localPosition, Quaternion.Euler(localRotation + poseresult.localRotation.eulerAngles), new Vector4(1, 1, 1, 1));

			if (parentIndex == -1)
			{
				//is root
				currentAnim.poseBase[i].currentTransform = deltaMatrix;
			}
			else
			{
				//current transform = take the parent index current transform and multiply with delta matrix
				currentAnim.poseBase[i].currentTransform = currentAnim.poseBase[parentIndex].currentTransform * deltaMatrix;
			}

			currentAnim.poseBase[i].updateNewPosition(objectHierarchy.getObject(i));

		}
	}

	AnimationDataHierarchal setAnimData(MovementStance state)
	{
		switch (state)
		{
			case MovementStance.IDLE:
				return idleAnim;
			case MovementStance.WALKING:
				return walkingAnim;
			case MovementStance.RUNNING:
				return runningAnim;
			default:
				return idleAnim;
		}
	}
}
