using UnityEngine;
using System.Collections;

public class MenuBG : MonoBehaviour {

    Texture2D currentBG;
    Renderer layerBG;
    float blend = 0f;
    public TextMesh pullQuote;

	// Use this for initialization
	void Start () {
        layerBG = renderer;
	}
	
	// Update is called once per frame
	void Update () {
        if ( MenuPortrait.current != null ) {
            if ( currentBG != MenuPortrait.current.bgTexture ) {
                blend = 0f;
                currentBG = MenuPortrait.current.bgTexture;
                layerBG.material.mainTexture = currentBG;
                audio.Play();
                pullQuote.text = MenuPortrait.current.pullQuote;
            }

            blend = Mathf.Clamp( blend + Time.deltaTime, 0f, 1f );
            layerBG.material.color = new Color( 1f, 1f, 1f, blend );
        } else {
            currentBG = null;
            blend = Mathf.Clamp( blend - Time.deltaTime, 0f, 1f );
            layerBG.material.color = new Color( 1f, 1f, 1f, blend );
            pullQuote.text = "an interview series with game developers\nabout their design philosophy and work;\nthen they collaborated on a short game...";
        }
	}
}
