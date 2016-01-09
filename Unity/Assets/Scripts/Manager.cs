using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;

using InfinityCode;
using UnityEditor;
using System.Security.Cryptography.X509Certificates;


public class Manager : MonoBehaviour
{

	public static List<Lokalitet> lokaliteter;

	public OnlineMaps onlineMaps;

	public List<DateTime> dates;
	public static DateTime currentDate;
	public static DateTime firstRegisteredDate;

	public Button playPauseButton;
	public Slider slider;
	public InputField searchField;

	public Slider timeSlider;
	public Text timeSliderCurrentDateText;
	public float animationSpeed;

	public Slider animationSpeedSlider;
	public Text animationSpeedSliderText;

	public Camera _camera;


	// Data selection
	bool showDataSelection;
	private GUIStyle rowStyle;

	public static List<string> datatyper;
	int valgtDatatype;


	OnlineMapsMarker3D marker;

	
	bool animating;
	bool browsingFile;

	//FileBrowser

	//skins and textures
	public GUISkin[] skins;
	public Texture2D file,folder,back,drive;
	string[] layoutTypes = {"Type 0","Type 1"};
	FileBrowser fb = new FileBrowser();
	string output = "no file";

	// Use this for initialization
	void Start ()
	{
		lokaliteter = new List<Lokalitet> ();
		animationSpeed = 0.1f;
		dates = new List<DateTime> ();
		animationSpeedSlider = GameObject.Find ("AnimationSpeedSlider").GetComponent<Slider> ();
		animationSpeedSliderText = GameObject.Find("AnimationSpeedSliderText").GetComponent<Text>();
		currentDate = firstDate ();
		timeSlider =  GameObject.Find ("TimeSlider").GetComponent<Slider> ();

		Populate ();
		populateDates ();
		currentDate = firstDate ();
//		Debug.Log("Earliest date: " + firstDate ().ToString ("yyyy-MM-dd"));
//		Debug.Log("Latest date: " + lastDate ().ToString ("yyyy-MM-dd"));
//		Debug.Log ("Total amount of dates: " + totalDates ());
		setTimeSliderMaxValue ();
		setTimeSliderCurrentDateText ();

		//Brukes ikke før vi evt. vil skalere ALLE markers samtidig. Ligger også funksjonalitet
		// i LokalitetsBehaviour.cs
//		OnlineMaps api = OnlineMaps.instance;
//		api.OnChangeZoom += OnChangeZoom;
//		OnChangeZoom ();


		//FileReader
		fb.guiSkin = skins[0]; //set the starting skin
		fb.fileTexture = file; 
		fb.directoryTexture = folder;
		fb.backTexture = back;
		fb.driveTexture = drive;
		fb.showSearch = true;
		fb.searchRecursively = true;
	}

	void OnGUI(){
		if(browsingFile){
		GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Space(10);
			//GUILayout.Label("Selected File: "+output);
			GUILayout.EndHorizontal();
			//draw and display output
			if(fb.draw()){ //true is returned when a file has been selected
				string filePath = "";
				//the output file is a member if the FileInfo class, if cancel was selected the value is null
				output = (fb.outputFile==null)?"cancel hit":fb.outputFile.ToString();

				if(output != "cancel hit"){
					filePath = fb.outputFile.ToString();

					//Hvis fila eksisterer og er i .xls-format
					if(File.Exists(filePath) && filePath.Trim().EndsWith(".xls")){
						Debug.Log(filePath);
						toggleFileBrowser();
						//Her skal vi kalle på Excel-metoden til arne. Vi sender med fb.outputFile.ToString() som argument.

					}
				}else{
					toggleFileBrowser();
				}
			}
		}

		if (showDataSelection) {
			if (rowStyle == null) {
				
				rowStyle = new GUIStyle (GUI.skin.button);
				rowStyle.alignment = TextAnchor.MiddleLeft;
				rowStyle.fontSize = 12;
				RectOffset margin = rowStyle.margin;


				rowStyle.margin = new RectOffset (-margin.left, -margin.right, 1, 1);
			}


			//GUI.BeginScrollView (new Rect (0, 30, 500, Screen.height - 60), new Vector2 (0f, 0f), new Rect (0, 0, 30, datatyper.Count * 26 + 4));
			GUILayout.BeginArea (new Rect (0, 30, 500, datatyper.Count * 19 + 4), GUI.skin.box);




			var color = rowStyle.normal.textColor;
			var hoverColor = rowStyle.hover.textColor;
			for (int i = 0; i < datatyper.Count; i++) {
				if (valgtDatatype == i) { 
					rowStyle.normal.textColor = new Color (0.2f, 0.8f, 0.4f);
					rowStyle.hover.textColor = new Color (0.2f, 0.8f, 0.4f);
				} else {
					rowStyle.normal.textColor = color;
					rowStyle.hover.textColor = hoverColor;
				}

				if (GUILayout.Button (datatyper [i], rowStyle, GUILayout.Height (18))) {
					valgtDatatype = i;
				}


			}
			rowStyle.normal.textColor = color;
			rowStyle.hover.textColor = hoverColor;

			GUILayout.EndArea();

				//GUI.EndScrollView();

		}

	}

	//	private void OnChangeZoom(){
	//		Debug.Log (onlineMaps._zoom);
	//		foreach (OnlineMapsMarker3D m in OnlineMapsControlBase3D.instance.markers3D) {
	//			//m.scale = (onlineMaps._zoom * 2);
	//		}
	//	}

	void Populate ()
	{
		/*
		lokaliteter = new ArrayList ();

		lokaliteter.Add (new Lokalitet ("12394", "Ørnøya", new Vector2(63.759167f, 8.449133f)));
		lokaliteter.Add (new Lokalitet ("31959", "Rataren", new Vector2(63.782383f, 8.526367f)));
		*/
		var excelReader = new EXCELREADER ();

		lokaliteter = excelReader.readGenerellInfo (Application.dataPath + "/Resources/06.01.2016-Generell-Info.xls");
		lokaliteter = excelReader.readData (Application.dataPath + "/Resources/06.01.2016-Lusetellinger-1712.xls", lokaliteter);

		for (int i = 0; i < lokaliteter.Count; i++) {
			Lokalitet l = lokaliteter [i] as Lokalitet;


			GameObject cylinder = (GameObject)Resources.Load ("markerPrefab", typeof(GameObject));


			marker = new OnlineMapsMarker3D (cylinder);
			Vector2 position = l.getCoordinates ();
		

			marker.position = position;
			marker.label = l.getLokalitetsnavn ();
			marker.scale = onlineMaps._zoom * 2;
			OnlineMapsControlBase3D control = onlineMaps.GetComponent<OnlineMapsControlBase3D> ();
			l.setMarker (marker);
			control.AddMarker3D (marker);

		}


	}

	// Update is called once per frame
	void Update ()
	{
			
	}

	private bool startAnimation ()
	{
		// Prøv å start animasjon av data
		animating = true;
		InvokeRepeating ("incrementDay", animationSpeed, animationSpeed);

		return true;
	}

	private bool stopAnimation ()
	{
		// Stop animasjon av data
		animating = false;
		CancelInvoke ("incrementDay");
		return true;
	}

	public void startPauseAnimation ()
	{
		Text t = playPauseButton.GetComponentInChildren<Text> ();
		if (animating) {
			if (stopAnimation ()) {
				// Bytt bilde på playPauseButton?
				t.text = "►";

			}
		} else {
			if (startAnimation ()) {
				// Bytt bilde på playPauseButton?
				t.text = "❙❙";

			}
		}
	}

	public void onAnimationSpeedChange(){
		animationSpeed = animationSpeedSlider.value;
		animationSpeedSliderText.text = animationSpeed.ToString ();
		if (animating) {
			CancelInvoke ("incrementDay");
			InvokeRepeating ("incrementDay", animationSpeed, animationSpeed);
		}
	}

	public void incrementDay(){
		timeSlider.value++;
	}

	public void toggleFileBrowser()
	{
		OnlineMapsControlBase3D control = onlineMaps.GetComponent<OnlineMapsControlBase3D> ();
		if (!browsingFile) {
			control.allowZoom = false;
			browsingFile = true;
		} else {
			control.allowZoom = true;
			browsingFile = false;
		}
	}

	public void searchValueChanged ()
	{


		if (Input.GetKey (KeyCode.Backspace) || searchField.text.Equals ("")) {
			return;
		}

		string query = searchField.text.Substring (0, searchField.caretPosition);



	


		Lokalitet l = null;
		for (int i = 0; i < lokaliteter.Count; i++) {
			if (((Lokalitet)lokaliteter [i]).getLokalitetsnavn ().ToUpper ().StartsWith (query.ToUpper ())) {
				l = (Lokalitet)lokaliteter [i];
				break;
			}
		}

		if (l != null) {
			searchField.text = l.getLokalitetsnavn ();
			searchField.caretPosition = query.Length;
			searchField.selectionAnchorPosition = searchField.caretPosition;
			searchField.selectionFocusPosition = searchField.text.Length;
		}
	}

	public void searchEnd ()
	{
		if (Input.GetKey (KeyCode.Return)) {
			search ();
		}
	}

	public void search ()
	{
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
				onlineMaps.SetPosition (l.getCoordinates ().x, l.getCoordinates ().y);
			}
		}
	}
		
	public DateTime firstDate ()
	{
		DateTime earliestDateSoFar;
		DateTime temp;
		earliestDateSoFar = new DateTime (9000, 1, 1);
		foreach (Lokalitet l in lokaliteter) {
			temp = l.firstDate ();
			if (earliestDateSoFar.CompareTo (temp) > 0) {
				earliestDateSoFar = temp;
			}
		}
		return earliestDateSoFar;

	}

	public DateTime lastDate ()
	{
		DateTime latestDateSoFar;
		DateTime temp;
		latestDateSoFar = new DateTime (1000, 1, 1);
		foreach (Lokalitet l in lokaliteter) {
			temp = l.lastDate ();
			if (latestDateSoFar.CompareTo (temp) < 0) {
				latestDateSoFar = temp;
			}
		}
		return latestDateSoFar;
	}

	public int totalDates(){

		DateTime firstD = new DateTime();
		DateTime lastD = new DateTime();
		firstD = firstDate ();
		lastD = lastDate ();


		if (!firstD.Equals(new DateTime(9000,1,1)) && !lastD.Equals(new DateTime(1000,1,1))){
			return (int)(lastD - firstD).TotalDays;
		}
		return 0;
	}

	public void setTimeSliderMaxValue(){
		timeSlider.maxValue = totalDates ();
	}

	public void setTimeSliderCurrentDateText (){
		GameObject temp = GameObject.Find ("CurrentDateText");
		if (temp != null) {
			timeSliderCurrentDateText = temp.GetComponent<Text>();
			timeSliderCurrentDateText.text = currentDate.ToString ("yyyy-MM-dd");//change to currentDate() when it's implemented
		}
	}

	public void populateDates(){
		DateTime temp = firstDate ();
		int numberOfDates = totalDates ();
		for (int i = 0; i <= numberOfDates; i++) {
			dates.Add (temp.AddDays ((double)i));
			//Debug.Log (dates [i].ToString ("yyyy-MM-dd"));
		}
	}

	public void onDateChanged(){
		currentDate = (firstDate ().AddDays (timeSlider.value));
		setTimeSliderCurrentDateText ();
		Måling målingen;
		foreach (Lokalitet l in lokaliteter) {
			foreach (Enhet e in l.getEnheter ()) {
				try {
					//Eksempel:
					//marker.scale = (float)e.getSenesteMålingGittDato (currentDate).getValueForKey ("Totalt antall lus funnet, Fastsittende lakselus i perioden");
					//Legg til funksjonalitet slik at hver marker endrer utseende gitt getValueForKey(key) på en måling.
					l.getMarker ().scale = (float)e.getSenesteMålingGittDato (currentDate).getValueForKey ("Totalt antall lus funnet, Fastsittende lakselus i perioden");
				} catch (Exception ex){
					//Debug.Log (ex); //Ikke enable, skaper massiv lag!
				}
			}
		}
	}

	public void toggleDataSelection() {
		OnlineMapsControlBase3D control = onlineMaps.GetComponent<OnlineMapsControlBase3D> ();
		if (!showDataSelection) {
			control.allowZoom = false;
			showDataSelection = true;
		} else {
			control.allowZoom = true;
			showDataSelection = false;
		}

	}
}