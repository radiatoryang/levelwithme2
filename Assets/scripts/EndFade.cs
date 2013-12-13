using UnityEngine;
using System.Collections;

public class EndFade : MonoBehaviour {

    bool faded = false;
    public Transform warpTunnel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // ending stuff


    void OnTriggerEnter( Collider c ) {
        if ( c.GetComponent<FirstPerson>() && !faded) {
            faded = true;
            StartCoroutine( WarpTimer( c.GetComponent<FirstPerson>() ) );
        }
    }

    IEnumerator WarpTimer( FirstPerson player ) {
        player.canMove = false;
        ScreenFade.Fade( Color.white, 1f, 0f, 0.5f, 0f, true );
        player.rigidbody.velocity = Vector3.zero;
        player.rigidbody.useGravity = false;
        player.transform.position = warpTunnel.position;
        yield return new WaitForSeconds( 3f );
        ScreenFade.Fade( Color.black, 0f, 1f, 0.5f, 0f, true );
        yield return new WaitForSeconds( 0.5f );
        Application.LoadLevel( 2 );
    }
}
