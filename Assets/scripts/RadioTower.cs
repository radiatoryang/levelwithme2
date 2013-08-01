using UnityEngine;
using System.Collections;

public class RadioTower : MonoBehaviour {
    public Renderer pinkFlash;
    bool flashed = false;
    bool playerIsHere = false;
	
	// Update is called once per frame
	void Update () {
        if ( !flashed && playerIsHere && IsLooking() ) {
            flashed = true;
            StartCoroutine( Flash() );
        }
	}

    IEnumerator Flash() {
        yield return new WaitForSeconds( 2f );
        pinkFlash.enabled = true;
        yield return new WaitForSeconds( 0.5f );
        pinkFlash.enabled = false;
    }

    bool IsLooking() {
        if ( Vector3.Angle( Camera.main.transform.forward, pinkFlash.transform.position - transform.position ) < 15f )
            return true;
        else
            return false;
    }

    void OnTriggerEnter() {
        playerIsHere = true;
    }

    void OnTriggerExit() {
        playerIsHere = false;
    }
}
