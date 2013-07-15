using UnityEngine;
using System.Collections;

public class WaterBuoyant : MonoBehaviour {

    Vector3 basePos;

    Vector3 rotateVector;
    Vector3 rotateVector2;
    float rotateSpeed;

	// Use this for initialization
	void Start () {
        basePos = transform.position;
        rotateVector = Random.onUnitSphere;
        rotateSpeed = Random.Range( 0.2f, 0.4f );
        rotateVector2 = Random.onUnitSphere;
        transform.LookAt( rotateVector2 );
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = basePos + Vector3.up * Mathf.Sin( (rotateVector.y + rotateSpeed + Time.time) * 0.25f ) * 0.8f;
        transform.RotateAround( rotateVector, rotateSpeed * Time.deltaTime );
        transform.RotateAround( rotateVector2, rotateSpeed * 0.618f * Time.deltaTime );
	}
}
