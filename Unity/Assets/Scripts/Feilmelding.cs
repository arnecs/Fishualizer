using UnityEngine;
using System.Collections;

public class Feilmelding : MonoBehaviour {

	public float width;
	public float height;

	bool show;
	string text = "";


	// Use this for initialization
	void Start () {
		Show("Lorem ispum dolor sit amet");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGUI() {


		if (show) {
			var bounds = new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
			GUI.Box (bounds, "Feil");

			/* Text */
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
		
			GUI.Label (new Rect ((Screen.width - width) / 2 + 20, (Screen.height - height) / 2 + 50, width - 40, height - 100), text);


			/* Ok Button */
			var buttonWidth = 100;
			var buttonHeight = 30;

			var okButtonRect = new Rect ((Screen.width - buttonWidth) / 2, (Screen.height + height) / 2 - buttonHeight - 20, buttonWidth, buttonHeight);
			if (GUI.Button (okButtonRect, "Ok")) {
				show = false;
				var mapControl = GameObject.Find ("Tileset map").GetComponent<OnlineMapsControlBase3D> ();
				mapControl.allowUserControl = true;
			}
		}

	}


	public void Show(string melding) {
		show = true;
		text = melding;

		var mapControl = GameObject.Find ("Tileset map").GetComponent<OnlineMapsControlBase3D> ();
		mapControl.allowUserControl = false;

	}
}
