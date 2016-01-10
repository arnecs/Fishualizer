using UnityEngine;
using System;
using System.Collections;

public class InspiserLokalitet : MonoBehaviour {
	
	bool showTooltip;
	Lokalitet l;
	GUIStyle s;
	string labelText;
	public GUISkin mySkin;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Starter");
		l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
		labelText = l.getLokalitetsnavn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (0)) {
			toggleTooltip ();
		}
	}

	void OnGUI(){
		GUI.skin = mySkin;
		var point = Camera.main.WorldToScreenPoint(transform.position);


		if (showTooltip) {
			GUI.Box (new Rect (point.x + Screen.width/20, Screen.height-point.y - Screen.height/5, 100, 20), labelText);
		}

		mySkin.label.normal.textColor = Color.black;
		GUI.Label (new Rect (point.x -51, Screen.height - point.y + Screen.height / 20, 100, 20), labelText);
		GUI.Label (new Rect (point.x -49, Screen.height - point.y + Screen.height / 20, 100, 20), labelText);
		GUI.Label (new Rect (point.x -50, Screen.height - point.y + 1 + Screen.height / 20, 100, 20), labelText);
		GUI.Label (new Rect (point.x -50, Screen.height - point.y - 1 + Screen.height / 20, 100, 20), labelText);

		mySkin.label.normal.textColor = Color.white;
		GUI.Label (new Rect (point.x -50, Screen.height - point.y + Screen.height / 20, 100, 20), labelText);
	}


	void toggleTooltip(){
		if(showTooltip){
			showTooltip = false;
		}else{
			showTooltip = true;
		}
	}

	public void setValueText(double d){
		labelText = d.ToString("0.000");
	}
}
