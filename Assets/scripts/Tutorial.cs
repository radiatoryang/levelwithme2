using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	float holdTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W))
			holdTime += Time.deltaTime;

		if (holdTime > 5f) {
			guiText.enabled = false;
			this.enabled = false;
		}
	}
}
