using UnityEngine;
using System.Collections;

public class WaterPhysics : MonoBehaviour {

    FirstPerson player;
    PullMove pullMove;

    void OnTriggerEnter(Collider c) {
        if ( c.GetComponent<FirstPerson>() ) {
            player = c.GetComponent<FirstPerson>();
            pullMove = player.GetComponent<PullMove>();
            player.isSwimming = true;
        }
    }

    void OnTriggerExit( Collider c ) {
        if ( player != null && c.collider == player.collider ) {
            player.isSwimming = false;
            player = null;
            pullMove = null;
        }
    }

    void FixedUpdate() {
        if ( player != null && pullMove.currentGrip == null) {
            player.rigidbody.AddForce( Physics.gravity * Mathf.Clamp(player.transform.position.y / 50f, -1.81f, 0f), ForceMode.Acceleration );
        }
    }
}
