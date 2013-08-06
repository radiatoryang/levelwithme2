using UnityEngine;
using System.Collections;

public class Wisp : MonoBehaviour {

    Transform owner;
    float speed = 5f;
    float stoppingDistance = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ( owner && Vector3.Distance(transform.position, owner.position) > stoppingDistance) {
            transform.position += ( owner.position - transform.position ).normalized * Time.deltaTime * speed;
        }
	}

    void OnTriggerEnter(Collider c) {
        if ( c.GetComponent<FirstPerson>() && owner == null)
            owner = c.transform;
    }
}
