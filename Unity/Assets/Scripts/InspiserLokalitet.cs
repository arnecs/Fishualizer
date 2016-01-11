using UnityEngine;
using System;
using System.Collections;

public class InspiserLokalitet : MonoBehaviour {
	
	bool showTooltip;
	bool showText;
	Lokalitet l;
	GUIStyle s;
	string labelText;
	public GUISkin mySkin;

	public InspiserLokalitet(){
	}
	
	// Use this for initialization
	void Start () {
		l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
		labelText = l.getLokalitetsnavn ();
		showText = true;

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
		if (showText) {
			GUI.skin = mySkin;
			var point = Camera.main.WorldToScreenPoint (transform.position);


			if (showTooltip) {
				GUI.Box (new Rect (point.x + Screen.width / 20, Screen.height - point.y - Screen.height / 5, 150, 40), labelText);
			}
			mySkin.label.normal.textColor = Color.black;
			GUI.Label (new Rect (point.x - 51, Screen.height - point.y + 10, 150, 40), labelText);
			GUI.Label (new Rect (point.x - 49, Screen.height - point.y + 10, 150, 40), labelText);
			GUI.Label (new Rect (point.x - 50, Screen.height - point.y + 1 + 10, 150, 40), labelText);
			GUI.Label (new Rect (point.x - 50, Screen.height - point.y - 1 + 10, 150, 40), labelText);

			mySkin.label.normal.textColor = Color.white;
			GUI.Label (new Rect (point.x - 50, Screen.height - point.y + 10, 150, 40), labelText);
		}
	}


	void toggleTooltip(){
		if(showTooltip){
			showTooltip = false;
		}else{
			showTooltip = true;
		}
	}

	public void setValueText(string l, double d, float t){
		labelText = l + " " + t.ToString() + "C\n" + d.ToString ("0.000");

	}

	public void ToggleText(bool b){
		showText = b;
	}
}
