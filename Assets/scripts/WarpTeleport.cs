using UnityEngine;
using System.Collections;

public class WarpTeleport : MonoBehaviour {

	public Transform warpTunnel;
	public Transform destination;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if ( c.GetComponent<FirstPerson>() ) {
			StartCoroutine ( WarpTimer ( c.GetComponent<FirstPerson>() ));
		}
	}

	IEnumerator WarpTimer (FirstPerson player ) {
		player.canMove = false;
		ScreenFade.Fade (Color.cyan, 1f, 0f, 1f, 0f, true);
		player.rigidbody.velocity = Vector3.zero;
		player.rigidbody.useGravity = false;
		player.transform.position = warpTunnel.position;
		yield return new WaitForSeconds( 2f);
		ScreenFade.Fade (Color.blue, 1f, 0f, 1f, 0f, true);
		yield return new WaitForSeconds( 1f);
		player.canMove = true;
		player.rigidbody.useGravity = true;
		player.transform.position = destination.position;
	}
}
