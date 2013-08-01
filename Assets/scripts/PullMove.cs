using UnityEngine;
using System.Collections;

public class PullMove : MonoBehaviour {

    FirstPerson fPerson;
    Vector3 gripPoint;
    Vector3 gripDirection;
    public Collider currentGrip;
    Vector3 pullStrength;

    float surfaceGripRange = 6.5f;
    float underwaterGripRange = 14f;
    public LayerMask gripMask;
    public GUIText gripCursor;

	// Use this for initialization
	void Start () {
        fPerson = GetComponent<FirstPerson>();
	}
	
	// Update is called once per frame
	void Update () {
        float gripRange = surfaceGripRange;
        if ( fPerson.isSwimming ) {
            gripRange = underwaterGripRange;
        }

        if ( Input.GetMouseButtonUp( 0 ) || (currentGrip != null && Vector3.Distance(gripPoint, transform.position) > gripRange) ) {
            currentGrip = null;
            gripPoint = Vector3.zero;
            gripDirection = Vector3.zero;
            gripCursor.text = "X";
        } else {
            gripCursor.text = ".";
        }

        if ( currentGrip != null ) {
            pullStrength = -gripDirection * Input.GetAxis( "Mouse Y" ) * ( 1f - (Vector3.Distance(gripPoint, transform.position) / gripRange) );
            gripCursor.text = "[O]";
        } else {
            RaycastHit rayHit = new RaycastHit();
            // Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            Ray ray = new Ray( Camera.main.transform.position, Camera.main.transform.forward );

            if ( Physics.Raycast( ray, out rayHit, gripRange, gripMask ) ) {
                gripCursor.text = "O";
                if ( Input.GetMouseButtonDown( 0 ) ) {
                    currentGrip = rayHit.collider;
                    gripPoint = rayHit.point;
                    // gripDirection = ( ( rayHit.point - transform.position ).normalized - rayHit.normal) / 2f;
                    if ( fPerson.isSwimming ) {
                        gripDirection = (rayHit.point - transform.position).normalized;
                    } else {
                        gripDirection = ( ( rayHit.point + rayHit.normal ) - transform.position ).normalized;
                    }
                }
            }
        }
	}

    void FixedUpdate() {
        if ( currentGrip != null) {
            if ( currentGrip.tag != "Lever" ) {
                if ( currentGrip.tag == "Swing" ) {
                    rigidbody.AddForce( pullStrength * 400f, ForceMode.Acceleration );
                } else {
                    rigidbody.AddForce( pullStrength * 100f, ForceMode.Acceleration );
                }
            } else { // LEVER
                currentGrip.GetComponent<PullLever>().Pull( pullStrength );
            }
        }
    }
}
