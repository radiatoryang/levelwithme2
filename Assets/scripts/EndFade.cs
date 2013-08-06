using UnityEngine;
using System.Collections;

public class EndFade : MonoBehaviour {

    bool faded = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter( Collider c ) {
        if ( c.GetComponent<FirstPerson>() && !faded) {
            faded = true;
            ScreenFade.Fade( Color.white, 0f, 1f, 2f, 0f, false );
        }
    }
}
