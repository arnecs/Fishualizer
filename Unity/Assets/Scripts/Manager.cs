using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using InfinityCode;


public class Manager : MonoBehaviour {

	ArrayList lokaliteter;

	public OnlineMaps onlineMaps;
	public Button playPauseButton;
	public Slider slider;

	public InputField searchField;

	public Camera _camera;

	bool animating;

	// Use this for initialization
	void Start () {
		lokaliteter = new ArrayList ();

		lokaliteter.Add (new Lokalitet (12394, "Ørnøya", 63.759167, 8.449133, null));
		lokaliteter.Add (new Lokalitet (31959, "Rataren", 63.782383, 8.526367, null));

		for (int i = 0; i < lokaliteter.Count; i++) {
			Lokalitet l = lokaliteter[i] as Lokalitet;

			OnlineMapsMarker marker = new OnlineMapsMarker();

			marker.position = new Vector2((float)l.getLengdegrad(), (float)l.getBreddegrad());
			marker.label = l.getLokalitetsnavn();

			onlineMaps.AddMarker(marker);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Update");
	}

	private bool startAnimation() {
		// Prøv å start animasjon av data
		animating = true;

		return true;
	}

	private bool stopAnimation() {
		// Stop animasjon av data
		animating = false;

		return true;
	}
	

	public void startPauseAnimation() {
		Text t = playPauseButton.GetComponentInChildren<Text> ();
		if (animating) {
			if ( stopAnimation()) {
				// Bytt bilde på playPauseButton?
				t.text = "►";
			}
		} else {
			if (startAnimation()){
				// Bytt bilde på playPauseButton?
				t.text = "❙❙";
			}
		}
	}



	public void searchValueChanged() {

		if (Input.GetKey (KeyCode.Backspace) || searchField.text.Equals ("")) {
			return;
		}

		string query = searchField.text.Substring (0, searchField.caretPosition);

		Lokalitet l = null;
		for (int i = 0; i < lokaliteter.Count; i++) {
			if (((Lokalitet)lokaliteter[i]).getLokalitetsnavn().ToUpper().StartsWith(query.ToUpper())) {
				l = (Lokalitet)lokaliteter[i];
				break;
			}
		}

		if (l != null) {
			searchField.text = l.getLokalitetsnavn();
			searchField.caretPosition = query.Length;
			searchField.selectionAnchorPosition = searchField.caretPosition;
			searchField.selectionFocusPosition = searchField.text.Length;
		}
	}


	public void searchEnd() {
		if (Input.GetKey (KeyCode.Return)) {
			search();
		}
	}

	public void search() {
		string query = searchField.text;

		if (!query.Equals ("")) {
			Lokalitet l = null;
			for (int i = 0; i < lokaliteter.Count; i++) {
				if (((Lokalitet)lokaliteter [i]).getLokalitetsnavn ().ToUpper ().StartsWith (query.ToUpper ())) {
					l = (Lokalitet)lokaliteter [i];
					break;
				}
			}
		
			if (l != null) {
				onlineMaps.SetPosition (l.getLengdegrad (), l.getBreddegrad ());
			}
		}
	}
}