using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {

	// Use this for initialization
	void Awake() {
        foreach (Transform child in transform)
        {
            if(child.tag == "Target")
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
