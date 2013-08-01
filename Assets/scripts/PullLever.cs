using UnityEngine;
using System.Collections;

public class PullLever : MonoBehaviour {

    float position = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles = new Vector3( position * 80f, 0f, 0f );
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
