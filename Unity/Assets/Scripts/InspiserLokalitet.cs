using UnityEngine;
using System.Collections;

public class InspiserLokalitet : MonoBehaviour {
	
	bool showTooltip;
	Lokalitet l;
	GameObject g;
	GUIStyle s;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Starter");
		//g = this.gameObject;
		//g.gameObject.GetComponent<InspiserLokalitet> ().enabled = true;
		l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void toggleTooltip(){
		if(showTooltip){
			showTooltip = false;
		}else{
			//l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
			//Debug.Log(l.getLokalitetsnavn());
			showTooltip = true;
		}
	}
	
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			// Whatever you want it to do.
			toggleTooltip ();
		}
		
		
	}

	void OnGUI(){
		var point = Camera.main.WorldToScreenPoint(transform.position);
		if (showTooltip) {
			GUI.Box (new Rect (point.x + Screen.width/20, Screen.height-point.y - Screen.height/5, 100, 20), l.getLokalitetsnavn());
		}

		GUI.TextField (new Rect (point.x, Screen.height - point.y + Screen.height / 20, 100, 20), l.getLokalitetsnavn (), GUIStyle.none);
	}
}
