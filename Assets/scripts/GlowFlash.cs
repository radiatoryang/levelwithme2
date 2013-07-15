using UnityEngine;
using System.Collections;

public class GlowFlash : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Random.value > 0.5f)
            renderer.enabled = false;
        StartCoroutine( Flash() );
	}
	
	// Update is called once per frame
    IEnumerator Flash() {
        float duration = Random.Range( 0.5f, 2f );
        while ( true ) {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds( duration );
        }
    }
}
