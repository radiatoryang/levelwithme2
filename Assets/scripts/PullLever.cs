using UnityEngine;
using System.Collections;

public class PullLever : MonoBehaviour {

    [HideInInspector]
    public float position = 0f;
    public Transform planetPivot;
    Vector3 positionZero;
    Quaternion baseRot;
    public Transform marsTransform;
    public PullLever pullTeleportBasis; // if this PullLever is > 0.9f, then enable this collider; 
    public Transform teleportDestination;
	public Transform warpTunnel;
    bool teleported = false;
	public GameObject periscopeHUD;
	public AudioClip hoverSound;

	public Rigidbody turnMode; // if specified, it'll just apply torque

	// Use this for initialization
	void Start () {
        if (planetPivot)
            positionZero = planetPivot.position;
        baseRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (!turnMode) {
        // HACK HACK HACK OH MY GOD THIS IS SO BAD
        if ( pullTeleportBasis ) {
            if ( Vector3.Angle(transform.parent.forward, (marsTransform.position - marsTransform.parent.position)) < 30f ) {
                if (!collider.enabled)
					AudioSource.PlayClipAtPoint (hoverSound, transform.position);
				collider.enabled = true;
				periscopeHUD.SetActive (true);
			} else {
                collider.enabled = false;
				periscopeHUD.SetActive (false);
			}
        }

        if ( teleportDestination && !teleported && Mathf.Abs( position ) > 0.001f ) {
			StartCoroutine( WarpTimer(Camera.main.transform.parent.GetComponent<FirstPerson>() ) );
            teleported = true;
        }

        if (!pullTeleportBasis)
            transform.localRotation = baseRot * Quaternion.Euler( new Vector3( position * 80f, 0f, 0f ) );
        if (planetPivot)
            planetPivot.position = positionZero + position * transform.forward * 4000f;
		}
	}

	IEnumerator WarpTimer (FirstPerson player ) {
		player.canMove = false;
		ScreenFade.Fade (Color.magenta, 1f, 0f, 0.5f, 0f, true);
		player.rigidbody.velocity = Vector3.zero;
		player.rigidbody.useGravity = false;
		player.transform.position = warpTunnel.position;
		yield return new WaitForSeconds( 2.5f);
		ScreenFade.Fade (Color.red, 1f, 0f, 1f, 0f, true);
		yield return new WaitForSeconds( 0.5f);
		player.canMove = true;
		player.rigidbody.useGravity = true;
		player.transform.position = teleportDestination.position;
		player.transform.rotation = teleportDestination.rotation;
	}

    public void Pull( Vector3 pullStrength ) {
		if (!turnMode) {
	        if ( Vector3.Dot( pullStrength, transform.forward ) < 0f ) {
	            position += pullStrength.magnitude * .01f;
	        } else {
	            position -= pullStrength.magnitude * .01f;
	        }
	        position = Mathf.Clamp( position, -1f, 1f );
		} else {
			turnMode.AddTorque ( 0f, Vector3.Dot( pullStrength, transform.forward ), 0f);
		}
    }
}
