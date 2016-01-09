using UnityEngine;
using System.Collections;

public class Inspiser : MonoBehaviour {

	bool isActive;
	Lokalitet l;

	// Use this for initialization
	void Start () {
		l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void toggleIsActive(){
		if(isActive){
			isActive = false;
		}else{
			isActive = true;
		}
	}

	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			// Whatever you want it to do.
			//l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
			//Debug.Log (l.getEnheter().Count);
			toggleIsActive ();
		}


	}

	void OnGUI(){
		if (isActive) {
			var point = Camera.main.WorldToScreenPoint(transform.position);
			GUI.Box (new Rect (point.x + Screen.width/20, Screen.height-point.y - Screen.height/5, 100, 20), "Tittel");
		}
	}
}
