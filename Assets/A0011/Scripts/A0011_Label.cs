using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0011_Label : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(new Vector3(0, 0.35f, 1));
        transform.Rotate(Vector3.up, 180);
	}
}
