using UnityEngine;
using System.Collections;

public class EndControl : MonoBehaviour {

	float speed = 200f;
	float mouseSpeed = 120f;
	bool ended = false;

	// Use this for initialization
	void Start () {
		ScreenFade.Fade (Color.black, 1f, 0f, 1f, 0f, true);
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0))
			Screen.lockCursor = true;

		transform.position -= Vector3.forward * speed * Time.deltaTime;

		transform.Rotate ( -Input.GetAxis ("Mouse Y") * mouseSpeed * Time.deltaTime, Input.GetAxis ( "Mouse X") * mouseSpeed * Time.deltaTime, 0f);

		if (transform.position.z < -8000f && !ended) {
			ended = true;
			StartCoroutine( Ending() );
		}
	}

	IEnumerator Ending () {
		ScreenFade.Fade(Color.black, 0f, 1f, 3f, 0f, true);
		yield return new WaitForSeconds(3f);
		Application.LoadLevel(0);
	}
}
