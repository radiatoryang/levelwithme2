using UnityEngine;
using System.Collections;

public class CameraBreathe : MonoBehaviour {

    Quaternion baseRot;

	// Use this for initialization
	void Start () {
        baseRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	    // transform.rotation = baseRot * Quaternion.Euler( 
	}
}
