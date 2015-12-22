using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using InfinityCode;


public class Manager : MonoBehaviour {

	ArrayList lokaliteter;

	public OnlineMaps onlineMaps;
	public Button playPauseButton;
	public Slider slider;

	public Camera camera;

	bool animating;

	// Use this for initialization
	void Start () {



		Vector3 p1 = camera.ViewportToWorldPoint(new Vector3(0, 0, 1000));
		Vector3 p2 = camera.ViewportToWorldPoint(new Vector3(1, 1, 1000));

		Vector2 size = new Vector2 (p2.x - p1.x, p2.y - p1.y);


		Debug.Log (size);

		//onlineMaps.tilesetSize = size;
		//onlineMaps.width = (int)size.y;
		//onlineMaps.height = (int)size.x;

	
		lokaliteter = new ArrayList ();

		lokaliteter.Add (new Lokalitet (12394, "Ørnøya", 63.759167, 8.449133, null));
		lokaliteter.Add (new Lokalitet (31959, "Rataren", 63.782383, 8.526367, null));



		for (int i = 0; i < lokaliteter.Count; i++) {
			Lokalitet l = lokaliteter[i] as Lokalitet;

			OnlineMapsMarker marker = new OnlineMapsMarker();

			
			marker.position = new Vector2((float)l.getLengdegrad(), (float)l.getBreddegrad());
			marker.label = l.getLokalitetsnavn();

			marker.customData = l;
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

	void OnGUI(){

	//	Debug.Log ("OnGui");

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



}
