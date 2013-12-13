using UnityEngine;
using System.Collections;

public class PullMove : MonoBehaviour {

    FirstPerson fPerson;
    Vector3 gripPoint;
    Vector3 gripDirection;
    public Collider currentGrip;
    Vector3 pullStrength;

	public float gripRangeNormalized { get { 
			if (currentGrip != null) {
				return Mathf.Clamp (1f - (Vector3.Distance(gripPoint, transform.position) / surfaceGripRange), 0f, 1f);
			} else {
				return 0f;
			}
		}
	}
	float surfaceGripRange = 6.5f;
    float underwaterGripRange = 14f;
    public LayerMask gripMask;
    public GUIText gripCursor;
	public GUITexture gripCrosshair;
	public Texture2D[] gripTex;

	float time = 0f;
	float realTime { get { return Time.time - time;} }

	// Use this for initialization
	void Start () {
		time = Time.time;
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
            gripCursor.text = "";
			gripCrosshair.texture = gripTex[0];
        } else {
            gripCursor.text = "";
			gripCrosshair.texture = gripTex[0];
        }

        if ( currentGrip != null ) {
            pullStrength = -gripDirection * Input.GetAxis( "Mouse Y" ) * ( 1f - (Vector3.Distance(gripPoint, transform.position) / gripRange) );
            CursorText ();
			gripCrosshair.texture = gripTex[2];
			gripCrosshair.color = Color.white;
			gripCrosshair.transform.localScale = Vector3.zero;

			if (Vector3.Angle ( Camera.main.transform.forward, gripPoint - Camera.main.transform.position) > Camera.main.fieldOfView * 0.5f) {
				gripCrosshair.transform.position = -Vector3.one;
			} else {
				gripCrosshair.transform.position = Camera.main.WorldToViewportPoint ( gripPoint);
			}
        } else {
			gripCrosshair.transform.position = new Vector3( 0.5f, 0.5f, 1f);
			gripCrosshair.transform.localScale = Vector3.zero;
            RaycastHit rayHit = new RaycastHit();
            // Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            Ray ray = new Ray( Camera.main.transform.position, Camera.main.transform.forward );

            if ( Physics.Raycast( ray, out rayHit, gripRange, gripMask ) ) {
				CursorText ();
				gripCrosshair.texture = gripTex[1];
				// gripCrosshair.color = Color.white * ( 1f + Mathf.Sin (Time.time * 16f) ) * 0.5f;
				gripCrosshair.transform.localScale = Vector3.one * Mathf.Abs (Mathf.Sin (Time.time * 5f) * 0.005f );
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
            } else {
				gripCrosshair.color = Color.white;
			}
        }
	}

	void CursorText () {
		if ( realTime < 90f) {
			gripCursor.text = "click + drag down";
		} else {
			gripCursor.text = "";
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
                currentGrip.GetComponent<PullLever>().Pull( pullStrength + Vector3.one * .01f );
            }
        }
    }
}
