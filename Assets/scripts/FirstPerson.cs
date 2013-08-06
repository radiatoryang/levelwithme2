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
    Camera cam;
    public AudioClip underwaterSound;

    PullMove pullMove;

    [HideInInspector]
    public bool isSwimming = false;

	// Use this for initialization
	void Awake () {
        cam = GetComponentInChildren<Camera>();
        pullMove = GetComponent<PullMove>();
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            Screen.lockCursor = true;
        }

        if ( Input.GetKeyDown( KeyCode.Escape ) ) {
            Screen.lockCursor = false;
        }

        float v = Input.GetAxis( "Vertical" );
        float h = Input.GetAxis( "Horizontal" );

        // hack to make player always walk forward while pulling
        if ( Mathf.Abs( v ) < 0.25f && pullMove.currentGrip != null && pullMove.currentGrip.tag != "Lever" )
            v = 1f;

        if ( isSwimming ) {
            moveVector = v * cam.transform.forward + h * transform.right;
            if ( !audio.isPlaying ) {
                audio.Play();
            }
        } else {
            moveVector = v * transform.forward + h * transform.right;
            if ( audio.isPlaying ) {
                audio.Stop();
            }
        }
            

        float realSensitivity = GetComponent<PullMove>().currentGrip != null ? sensitivity * 0.01f : sensitivity;

        rotY += Input.GetAxis( "Mouse Y" ) * Time.deltaTime * realSensitivity * (invertY ? 1f : -1f);
        rotY = Mathf.Clamp( rotY, minY, maxY );
        cam.transform.localRotation = Quaternion.Euler( rotY, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z );
        transform.Rotate( 0f, Input.GetAxis( "Mouse X" ) * Time.deltaTime * realSensitivity, 0f );
	}

    void FixedUpdate() {
        float realMoveSpeed = isSwimming ? moveSpeed * 4f : moveSpeed;

        if ( moveVector == Vector3.zero && !isSwimming) {
            rigidbody.AddForce( -rigidbody.velocity * 1.5f, ForceMode.Impulse );
        } else {
            rigidbody.AddForce( moveVector.normalized * realMoveSpeed, ForceMode.VelocityChange );
        }
    }
}
