using UnityEngine;
using System.Collections;

public class WarpTunnel : MonoBehaviour {

	Vector2 textureOffset;
	float speed = 4f;

	// Use this for initialization
	void Start () {
		textureOffset = Random.insideUnitCircle.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.mainTextureOffset += textureOffset * Time.deltaTime * speed;

	}
}
