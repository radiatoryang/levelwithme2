using UnityEngine;
using System.Collections;

public class MenuPortrait : MonoBehaviour {

    public TextMesh text;
    public Renderer portrait;

    [Multiline]
    public string portraitText;
    public Texture2D portraitTexture;
    public Texture2D bgTexture;

    public static MenuPortrait current;
    const float lerpSpeed = 2f;

	// Use this for initialization
	void Start () {
        text.text = portraitText;
        portrait.material.mainTexture = portraitTexture;
	}
	
	// Update is called once per frame
	void Update () {
        if ( MenuPortrait.current == this ) {
            text.color = Color.Lerp( text.color, new Color(0f, 0f, 0f, 1f), Time.deltaTime * lerpSpeed);
            portrait.material.color = Color.Lerp( portrait.material.color, new Color( 1f, 1f, 1f, 1f ), Time.deltaTime * lerpSpeed );
            portrait.transform.parent.localScale = Vector3.Lerp( portrait.transform.parent.localScale, new Vector3( 1.6f, 1.6f, 1.6f ), Time.deltaTime * lerpSpeed );
        } else {
            text.color = Color.Lerp( text.color, new Color( 0.5f, 0.5f, 0.5f, 0.6f ), Time.deltaTime * lerpSpeed );
            portrait.material.color = Color.Lerp( portrait.material.color, new Color( 1f, 1f, 1f, 0.3f ), Time.deltaTime * lerpSpeed );
            portrait.transform.parent.localScale = Vector3.Lerp( portrait.transform.parent.localScale, new Vector3( 0.75f, 0.75f, 0.75f ), Time.deltaTime * lerpSpeed );
        }
	}

    void OnMouseEnter() {
        current = this;
    }

    void OnMouseExit() {
        if ( current == this )
            current = null;
    }
}
