using UnityEngine;
using System;
using System.Collections;

public class InspiserLokalitet : MonoBehaviour {
	
	bool showTooltip;
	Lokalitet l;
	GameObject g;
	GUIStyle s;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Starter");
		l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void toggleTooltip(){
		if(showTooltip){
			showTooltip = false;
		}else{
			showTooltip = true;
		}
	}
	
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			toggleTooltip ();
		}
	}

	void OnGUI(){
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.MiddleCenter;

		var point = Camera.main.WorldToScreenPoint(transform.position);
		if (showTooltip) {
			GUI.Box (new Rect (point.x + Screen.width/20, Screen.height-point.y - Screen.height/5, 100, 20), l.getLokalitetsnavn());
		}
		GUI.Label (new Rect (point.x, Screen.height - point.y + Screen.height / 20, 100, 20), l.getLokalitetsnavn (), style);
	}
}
