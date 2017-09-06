using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
}
