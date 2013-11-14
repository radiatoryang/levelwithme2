using UnityEngine;
using System.Collections;

public class MenuURL : MonoBehaviour {

    public string url;
    public bool isArticle = true;

    public static MenuURL currentURL;
    public static Texture2D texture;

	// Use this for initialization
	void Start () {
        texture = new Texture2D( 1, 1, TextureFormat.ARGB32, false );

        // set the pixel values
        texture.SetPixel( 0, 0, new Color( 1.0f, 1.0f, 1.0f, 1f ) );

        // Apply all SetPixel calls
        texture.Apply();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        const float width = 256f;
        const float height = 72f;
        const float padding = 16f;
        if ( currentURL != null ) {
            Vector2 screenPos = Event.current.mousePosition;
            Vector2 guiPos = GUIUtility.ScreenToGUIPoint(screenPos) + new Vector2( 16f, 4f);
            if ( guiPos.x > Screen.width - width )
                guiPos.x -= width;
            GUI.color = new Color( 1f, 1f, 1f, 0.1f );
            GUI.DrawTexture( new Rect( guiPos.x - padding, guiPos.y - padding * 0.5f, width + padding * 2f, height ), texture );
            GUI.color = new Color( 0.5f, 0.2f, 0.2f, 1f );
            string label = currentURL.isArticle ? "click to read this interview:" : "click to open in browser:";
            GUI.Label( new Rect( guiPos.x, guiPos.y, width, height ), "<b><size=16>" + label + "</size></b>\n<size=12>" + currentURL.url + "</size>" );
        }
    }

    void OnMouseEnter() {
        currentURL = this;
    }

    void OnMouseExit() {
        if ( currentURL == this )
            currentURL = null;
    }

    void OnMouseDown() {
        if ( Application.isWebPlayer ) {
            Application.ExternalEval( "window.open('" + url + "','" + url + "')" );
        } else {
            Application.OpenURL( url );
        }
    }
}
