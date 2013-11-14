using UnityEngine;
using System.Collections;

public class MenuBegin : MonoBehaviour {

    public GUITexture fadeBG;
    public Texture2D newTexture;
    float timeStart;
    public Color endColor;
    float timeHold;
    Vector3 basePos;

	// Use this for initialization
	void Start () {
        timeStart = Time.time;
        basePos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // we lie to players and say the game is loading so that they'll actually play with the menu screen for a bit?
        if ( Time.time - timeStart > 22f ) {
            if (renderer.material.mainTexture != newTexture)
                renderer.material.mainTexture = newTexture;
            renderer.material.color = Color.Lerp( Color.clear, endColor, Mathf.Abs( Mathf.Sin( Time.time * 0.25f ) ) );
        }

        if ( Input.GetKey( KeyCode.W ) ) {
            timeHold += Time.deltaTime * 0.25f;
        } else {
            timeHold -= Time.deltaTime;
        }

        timeHold = Mathf.Clamp( timeHold, 0f, 2f );
        transform.position = basePos + timeHold * transform.forward * 2f;
        audio.volume = (1f - (timeHold * 1.6f) ) * 0.3f;

        fadeBG.color = new Color( 0f, 0f, 0f, timeHold );

        if ( timeHold > 0.8f )
            Application.LoadLevel( 1 );
	}
}
