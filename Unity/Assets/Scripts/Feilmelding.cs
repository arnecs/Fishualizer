using UnityEngine;
using System.Collections;

public class Feilmelding : MonoBehaviour {

	public float width;
	public float height;

	bool show = true;
	string text = "Kunne ikke lese fil";


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGUI() {


		if (show) {
			var bounds = new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);

			GUI.Box (bounds, "Feil");



			// Ok Bottun
			var buttonWidth = 100;
			var buttonHeight = 30;

			var okButtonRect = new Rect ((Screen.width - buttonWidth) / 2, (Screen.height + height) / 2 - buttonHeight - 20, buttonWidth, buttonHeight);
			if (GUI.Button (okButtonRect, "Ok")) {
				show = false;
			}
		}

	}


	public void Show(string melding) {

	}
}
