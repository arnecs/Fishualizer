using UnityEngine;
using System;
using System.Collections;

public class InspiserLokalitet : MonoBehaviour {

	bool showTooltip;
	bool showText;
	Lokalitet l;
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
		
		l = (Lokalitet)gameObject.GetComponent<OnlineMapsMarker3DInstance>().marker.customData;
		//m = (Måling)e.getSenesteMålingGittDato(Manager.currentDate);
		labelText = l.getLokalitetsnavn ();
		showText = true;

	}

	private string VerticalText(string s)
	{
		string st = "";
		foreach (char c in s)
		{
			st += c + "\n";
		}
		return st.ToUpper();
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

		GUI.depth = 1000;
		if (showText && api.zoom > 12) {
			mySkin.label.normal.textColor = Color.black;
			mySkin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (pointText.x - 51, Screen.height - pointText.y + 10, 100, 100), labelText);
			GUI.Label (new Rect (pointText.x - 49, Screen.height - pointText.y + 10, 100, 100), labelText);
			GUI.Label (new Rect (pointText.x - 50, Screen.height - pointText.y + 1 + 10, 100, 100), labelText);
			GUI.Label (new Rect (pointText.x - 50, Screen.height - pointText.y - 1 + 10, 100, 100), labelText);
			
			mySkin.label.normal.textColor = Color.white;
			GUI.Label (new Rect (pointText.x - 50, Screen.height - pointText.y + 10, 100, 100), labelText);
			//GUI.matrix = matrixBackup;
		}
		
		
		GUI.depth = 1;
		if (showTooltip) {
			//m = (Måling)e.getSenesteMålingGittDato(Manager.currentDate);
			
			mySkin.box.alignment = TextAnchor.UpperCenter;
			mySkin.box.normal.textColor = Color.white;
			mySkin.box.normal.background = guiDark;
			
			var barRect = new Rect (point.x + Screen.width / 20, Screen.height - point.y - Screen.height / 5, 380, 20);
			
			GUI.Box (barRect, l.getLokalitetsnavn());
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
		point = Camera.main.WorldToScreenPoint (transform.position);
		if(showTooltip){
			showTooltip = false;
		}else{
			showTooltip = true;
		}
	}

	public void setValueText(string l, double d, float t){
		labelText = l + " " + t.ToString() + "°C\n" + d.ToString ("0.000");
	}
	
	public void ToggleText(bool b){
		showText = b;
	}
}
