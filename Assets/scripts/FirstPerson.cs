using UnityEngine;
using System.Collections;

public class FirstPerson : MonoBehaviour {

    public float sensitivity = 5f;
    public float minY = -70f;
    public float maxY = 90f;
    public bool invertY = false;
    public float moveSpeed = 2f;
    Vector3 moveVector;
    float rotY;
	[HideInInspector]
    public Camera cam;
    // public AudioClip underwaterSound;

    PullMove pullMove;

    [HideInInspector]
    public bool isSwimming = false;

	[HideInInspector]
	public bool canMove = true;

	bool grounded = false;

	// Use this for initialization
	void Awake () {
        cam = GetComponentInChildren<Camera>();
        pullMove = GetComponent<PullMove>();
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics.Raycast ( transform.position, -transform.up, 1.4f);

        if ( Input.GetMouseButtonDown( 0 ) ) {
            Screen.lockCursor = true;
        }

        if ( Input.GetKeyDown( KeyCode.Escape ) || Input.GetKeyDown( KeyCode.Tab) ) {
            Screen.lockCursor = false;
        }

        float v = Input.GetAxis( "Vertical" );
        float h = Input.GetAxis( "Horizontal" );

		// HACK HACK HACK
		v = 0f;
		h = 0f;

		if (grounded || isSwimming) {
			const float crawlSpeed = 3f;
			if( Input.GetKey(KeyCode.W) && Mathf.Sin (Time.time * crawlSpeed) < 0f) {
				//moveVector = transform.forward;
				v = 1f;
				audio.loop = false;
				cam.transform.localPosition = new Vector3( 0f, 0.25f, 0f) + Vector3.up * 0.12f * Mathf.Sin (Time.time * crawlSpeed);
				if ( (!audio.isPlaying || audio.volume < 0.2f) && Mathf.Sin (Time.time * crawlSpeed) < -0.8f) {
					audio.volume = 0.2f;
					audio.Play ();
				}
			} else if (pullMove.currentGrip != null && pullMove.currentGrip.tag != "Lever") {
				cam.transform.localPosition = Vector3.Lerp ( cam.transform.localPosition, new Vector3( 0f, 0.25f, 0f) + Vector3.up * -0.5f * pullMove.gripRangeNormalized, Time.deltaTime * 3f);
				if ( Mathf.Abs (Input.GetAxis("Mouse Y")) > 0.1f ) {
					v = 1f;
					if (!audio.loop) {
						audio.loop = true;
						audio.Play ();
					}
					audio.volume = Mathf.Lerp ( audio.volume, 0.4f * Mathf.Abs (Input.GetAxis ("Mouse Y")), Time.deltaTime * 3f);
				}
			} else {
				audio.volume = Mathf.Lerp ( audio.volume, 0f, Time.deltaTime * 3f);
				if ( audio.volume < 0.01f ) {
					audio.Stop ();
					audio.loop = false;
				}

				cam.transform.localPosition = Vector3.Lerp ( cam.transform.localPosition, new Vector3( 0f, 0.25f, 0f), Time.deltaTime * 3f);
			}
		} else {
			audio.Stop ();
			audio.loop = false;
			cam.transform.localPosition = Vector3.Lerp ( cam.transform.localPosition, new Vector3( 0f, 0.25f, 0f), Time.deltaTime * 3f);
		}

		
		if ( isSwimming ) {
            moveVector = v * cam.transform.forward + h * transform.right;
//            if ( !audio.isPlaying ) {
//                audio.Play();
//            }
        } else {
            moveVector = v * transform.forward + h * transform.right;
//            if ( audio.isPlaying ) {
//                audio.Stop();
//            }
        }
		
        float realSensitivity = GetComponent<PullMove>().currentGrip != null ? sensitivity * 0.01f : sensitivity;

        rotY += Input.GetAxis( "Mouse Y" ) * Time.deltaTime * realSensitivity * (invertY ? 1f : -1f);
        rotY = Mathf.Clamp( rotY, minY, maxY );
        cam.transform.localRotation = Quaternion.Euler( rotY, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z );
        transform.Rotate( 0f, Input.GetAxis( "Mouse X" ) * Time.deltaTime * realSensitivity, 0f );
	}

    void FixedUpdate() {
        float realMoveSpeed = isSwimming ? moveSpeed * 2f : moveSpeed;

		if (canMove) {
	        if ( moveVector == Vector3.zero && !isSwimming) {
	            rigidbody.AddForce( -rigidbody.velocity * 1.5f + Physics.gravity, ForceMode.Impulse );
	        } else {
	            rigidbody.AddForce( moveVector.normalized * realMoveSpeed, ForceMode.VelocityChange );
	        }
		}
    }
}
