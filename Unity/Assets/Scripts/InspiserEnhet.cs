﻿using UnityEngine;
using System;
using System.Collections;

public class InspiserEnhet : MonoBehaviour {
	
	bool showTooltip;
	bool showText;
	Enhet e;
	Måling m;
	GUIStyle s;
	string labelText;
	public GUISkin mySkin;
	public Texture xBtn;
	public Texture2D guiDark;
	public Texture2D guiLight;
	Manager manager;
	string info;

	// Move window
	Vector2 point;
	Vector2 lastMousePosition;
	bool moving;


	OnlineMaps api;
	// Use this for initialization
	void Start () {

		api = GameObject.Find ("Tileset map").GetComponent<OnlineMaps>();

		e = (Enhet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
		m = (Måling)e.getSenesteMålingGittDato(Manager.currentDate);
		labelText = e.getEnhetsId ();
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
		var pointText = Camera.main.WorldToScreenPoint (transform.position);
		GUI.skin = mySkin;
		GUI.depth = 0;
		if (showText && api.zoom > 12) {
			mySkin.label.normal.textColor = Color.black;
			GUI.Label (new Rect (pointText.x - 51, Screen.height - pointText.y + 10, 100, 20), labelText);
			GUI.Label (new Rect (pointText.x - 49, Screen.height - pointText.y + 10, 100, 20), labelText);
			GUI.Label (new Rect (pointText.x - 50, Screen.height - pointText.y + 1 + 10, 100, 20), labelText);
			GUI.Label (new Rect (pointText.x - 50, Screen.height - pointText.y - 1 + 10, 100, 20), labelText);
			
			mySkin.label.normal.textColor = Color.white;
			GUI.Label (new Rect (pointText.x - 50, Screen.height - pointText.y + 10, 100, 20), labelText);
		}


		GUI.depth = 1;
		if (showTooltip) {
			m = (Måling)e.getSenesteMålingGittDato(Manager.currentDate);

			mySkin.box.alignment = TextAnchor.UpperCenter;
			mySkin.box.normal.textColor = Color.white;
			mySkin.box.normal.background = guiDark;



			var barRect = new Rect (point.x,Screen.height - point.y, 380, 20);
			//var barRect = new Rect (0, 0, 380, 20);

			Debug.Log (barRect);
			if (barRect.yMin < 0) {
				point.y = Screen.height;
				barRect = new Rect (point.x + Screen.width / 20, point.y, 380, 20);
			}

			GUI.Box (barRect, e.getEnhetsId ());
			var mp = Input.mousePosition;

			if (barRect.Contains (new Vector2 (mp.x, Screen.height - mp.y)) && Input.GetMouseButton (0)) {
				moving = true;	
			}
			if (moving && !Input.GetMouseButton (0)) {
				moving = false;
			}
			if (moving) {
			
				if (lastMousePosition != Vector2.zero) {
					var dmp = new Vector2 (mp.x - lastMousePosition.x, mp.y - lastMousePosition.y);

					point = new Vector2(point.x + dmp.x, point.y  + dmp.y);
				}
				lastMousePosition = mp;
			} else {
					lastMousePosition = Vector2.zero;
			}


			if(GUI.Button(new Rect (point.x + 380, Screen.height - point.y, 20, 20), xBtn)){
				toggleTooltip();
			}

			mySkin.box.alignment = TextAnchor.UpperLeft;
			mySkin.box.normal.background = guiLight;
			mySkin.box.normal.textColor = Color.black;
			if(m != null){
				int numLines = m.ToString().Split('\n').Length;
				GUI.Box (new Rect (point.x, Screen.height - point.y + 20, 400, 11*numLines), m.ToString());
			}else{
				GUI.Box (new Rect (point.x, Screen.height - point.y + 20, 400, 100), "Ingen data tilgjengelig før denne datoen.");
			}
		}
	}
	
	
	void toggleTooltip(){
		point = Camera.main.WorldToScreenPoint (transform.position);
		point.y += 50;
		point.x += 10;
		if(showTooltip){
			showTooltip = false;
		}else{
			showTooltip = true;
		}
	}
	


	public void setValueText(double d){
		labelText = d.ToString("0.000");
	}

	public void ToggleText(bool b){
		showText = b;
	}
}
