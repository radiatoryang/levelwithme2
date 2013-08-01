using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WIPNote : MonoBehaviour {

    [Multiline]
    public string note;

	// Use this for initialization
	void Start () {
        TextIt();
	}

    void OnEnable() {
        TextIt();
    }

    void OnDisable() {
        TextIt();
    }

    void TextIt() {
        GetComponent<TextMesh>().text = note;
    }
	
}
