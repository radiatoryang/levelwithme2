using UnityEngine;
using System.Collections;

public class AlexPeriscope : MonoBehaviour {

    public Transform camMirror;
	
	// Update is called once per frame
	void Update () {
        camMirror.rotation = transform.rotation;
		rigidbody.AddTorque ( 0f, 0.009f * Mathf.Sin (Time.time), 0f);
	}
}
