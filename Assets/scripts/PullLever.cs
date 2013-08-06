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
    bool teleported = false;

	// Use this for initialization
	void Start () {
        if (planetPivot)
            positionZero = planetPivot.position;
        baseRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        // HACK HACK HACK OH MY GOD THIS IS SO BAD
        if ( pullTeleportBasis ) {
            if ( Vector3.Angle(transform.parent.forward, (marsTransform.position - marsTransform.parent.position)) < 45f )
                collider.enabled = true;
            else
                collider.enabled = false;
        }

        if ( teleportDestination && !teleported && Mathf.Abs( position ) > 0.001f ) {
            ScreenFade.Fade( Color.red, 1, 0, .5f, 0, true );
            Camera.main.transform.parent.position = teleportDestination.position;
            Camera.main.transform.parent.rotation = teleportDestination.rotation;
            teleported = true;
        }

        if (!pullTeleportBasis)
            transform.localRotation = baseRot * Quaternion.Euler( new Vector3( position * 80f, 0f, 0f ) );
        if (planetPivot)
            planetPivot.position = positionZero + position * transform.forward * 4000f;
	}

    public void Pull( Vector3 pullStrength ) {
        if ( Vector3.Dot( pullStrength, transform.forward ) < 0f ) {
            position += pullStrength.magnitude * .01f;
        } else {
            position -= pullStrength.magnitude * .01f;
        }
        position = Mathf.Clamp( position, -1f, 1f );
    }
}
