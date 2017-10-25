using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularAssestSnapping : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform[] transforms = GetComponentsInChildren<Transform>();

        foreach (Transform tf in transforms)
        {
            if (tf == this.transform)
                continue;

            float posX = Mathf.Round(tf.position.x);
            float posY = 0f;
            float posZ = Mathf.Round(tf.position.z * 2) / 2;

            Vector3 newPos = new Vector3(posX, posY, posZ);

            tf.position = newPos;

            float rotX = -90f;
            float rotY = 0f;
            float rotZ = determineZRotation(tf.eulerAngles.z);

            Debug.Log(rotX + "." + rotY + "." + rotZ);

            tf.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            
            
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    float determineZRotation(float rot)
    {
        Debug.Log(rot);
        if (rot > 90f && rot < 270f)
        {
            return 180f;
        }
        else return 0f;
    }
}
