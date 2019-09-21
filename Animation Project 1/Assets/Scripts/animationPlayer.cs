using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationPlayer : MonoBehaviour
{

    public AnimationData animData;

    public bool play = false;

    int currentKeyFrame = 0;
    float frameCount = 0;
    // Update is called once per frame
    void Update()
    {
        if(play)
        {
            //play current frame data
            KeyFrame fromKey = animData.keyFrames[currentKeyFrame];
            KeyFrame toKey;
            if (animData.keyFrames.Count > currentKeyFrame+1)
            {
               toKey = animData.keyFrames[currentKeyFrame + 1];
            }
            else
            {
                toKey = fromKey;
            }
            float t = ((frameCount - (float)fromKey.atFrame) / ((float)toKey.atFrame - (float)fromKey.atFrame));
            //do lerp
            if(toKey != fromKey)
            {
                transform.position = Vector3.Lerp(fromKey.keyPosition, toKey.keyPosition, t);
            }
            else
            {
                transform.position = toKey.keyPosition;
            }

            //update to next frame
            frameCount++;
            if (frameCount >= toKey.atFrame)
            {
                currentKeyFrame++;
            }
            if (frameCount > animData.totalFrameDuration)
            {
                currentKeyFrame = 0;
                frameCount = 0;
            }

        }
    }
}
