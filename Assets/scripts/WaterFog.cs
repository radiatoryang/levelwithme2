using UnityEngine;
using System.Collections;

public class WaterFog : MonoBehaviour {
    public Color newFog;
    float fogDensityBase;

	// Use this for initialization
	void Start () {
        fogDensityBase = RenderSettings.fogDensity;
	}
	
	// Update is called once per frame
	void Update () {
        if ( Camera.main.transform.position.y < transform.position.y ) {
            RenderSettings.fogDensity = 0.07f;
            RenderSettings.fogColor = newFog;
        } else {
            ResetFog();
        }
	}

    void ResetFog() {
        RenderSettings.fogDensity = fogDensityBase;
        RenderSettings.fogColor = Color.black;
    }

    void OnApplicationQuit() {
        ResetFog();
    }
}
