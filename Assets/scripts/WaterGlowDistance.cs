using UnityEngine;
using System.Collections;

public class WaterGlowDistance : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        Color c = renderer.material.GetColor( "_TintColor" );
        renderer.material.SetColor( "_TintColor", new Color( c.r, c.g, c.b, Mathf.Pow(distance / 200f, 2f) ) );
	}
}
