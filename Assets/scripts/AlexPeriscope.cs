using UnityEngine;
using System.Collections;

public class AlexPeriscope : MonoBehaviour {

    public Transform camMirror;
	
	// Update is called once per frame
	void Update () {
        camMirror.rotation = transform.rotation;
	}
}
