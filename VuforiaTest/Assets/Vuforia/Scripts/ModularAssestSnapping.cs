using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularAssestSnapping : MonoBehaviour {

    public float xStepSize = 1;
    public float zStepSize = 1;
    public float rotationStepSize = 90;

	// Use this for initialization
	void Start () {
        Transform[] transforms = GetComponentsInChildren<Transform>();

        foreach (Transform tf in transforms)
        {
            if (tf == this.transform)
                continue;

            // Set Position to grid
            float posX = determinePosition(tf.position.x, xStepSize);
            float posY = 0f;
            float posZ = determinePosition(tf.position.z, zStepSize);

            Vector3 newPos = new Vector3(posX, posY, posZ);

            tf.position = newPos;

            // Set Rotation to grid
            float rotX = 0f;
            float rotY = determineYRotation(tf.localEulerAngles.y);
            float rotZ = 0f;

            Debug.Log(tf.localEulerAngles);

            Vector3 newRot = new Vector3(rotX, rotY, rotZ);

            tf.eulerAngles = newRot;
        }
    }

    /* Positions start at 0 and have snapping points every stepSize units */
    float determinePosition(float pos, float stepSize)
    {
        if(pos % stepSize < stepSize / 2)
        {
            return pos - (pos % stepSize);
        }
        else
        {
            return pos - (pos % stepSize) + stepSize;
        }
    }

    /* Set the Y rotation to the nearest multiple of rotationStepSize between 0 and 359 */
    float determineYRotation(float rot)
    {
        // Work with positive rotations only
        while (rot < 0f)
            rot += 360;

        // Work with whole numbers only
        rot = Mathf.Round(rot);

        // Get 0ffset
        float offset = rot % rotationStepSize;

        // Round down to nearest multiple of rotationStepSize
        rot = rot - (rot % rotationStepSize);

        // an offset larger than half the rotationStepSize neans it should have been rounded up instead
        if (offset > rotationStepSize / 2)
            rot += rotationStepSize;

        // Make rotation between 0 and 359
        while (rot >= 360)
            rot -= 360;

        return rot;
    }
}
