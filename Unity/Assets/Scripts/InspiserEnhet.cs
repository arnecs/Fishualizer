using UnityEngine;
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
	
	// Use this for initialization
	void Start () {
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
		var point = Camera.main.WorldToScreenPoint (transform.position);
		GUI.skin = mySkin;
		GUI.depth = 0;
		if (showText) {
			mySkin.label.normal.textColor = Color.black;
			GUI.Label (new Rect (point.x - 51, Screen.height - point.y + Screen.height / 40, 100, 20), labelText);
			GUI.Label (new Rect (point.x - 49, Screen.height - point.y + Screen.height / 40, 100, 20), labelText);
			GUI.Label (new Rect (point.x - 50, Screen.height - point.y + 1 + Screen.height / 40, 100, 20), labelText);
			GUI.Label (new Rect (point.x - 50, Screen.height - point.y - 1 + Screen.height / 40, 100, 20), labelText);
		
			mySkin.label.normal.textColor = Color.white;
			GUI.Label (new Rect (point.x - 50, Screen.height - point.y + Screen.height / 40, 100, 20), labelText);
		}

		GUI.depth = 1;
		if (showTooltip) {
			m = (Måling)e.getSenesteMålingGittDato(Manager.currentDate);

			mySkin.box.alignment = TextAnchor.UpperCenter;
			mySkin.box.normal.textColor = Color.white;
			mySkin.box.normal.background = guiDark;
			GUI.Box (new Rect (point.x + Screen.width / 20, Screen.height - point.y - Screen.height / 5, 380, 20), e.getEnhetsId());
			if(GUI.Button(new Rect (point.x + Screen.width / 20 + 380, Screen.height - point.y - Screen.height / 5, 20, 20), xBtn)){
				toggleTooltip();
			}

			mySkin.box.alignment = TextAnchor.UpperLeft;
			mySkin.box.normal.background = guiLight;
			mySkin.box.normal.textColor = Color.black;
			if(m != null){
				int numLines = m.ToString().Split('\n').Length;
				GUI.Box (new Rect (point.x + Screen.width / 20, Screen.height - point.y - Screen.height / 5 + 20, 400, 11*numLines), m.ToString());
			}else{
				GUI.Box (new Rect (point.x + Screen.width / 20, Screen.height - point.y - Screen.height / 5 + 20, 400, 100), "Ingen data tilgjengelig før denne datoen.");
			}
		}
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

	public void ToggleText(bool b){
		showText = b;
	}
}
