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

	//GUI
	public Button playPauseButton;
	public Slider slider;
	public InputField searchField;

	public Slider timeSlider;
	public Text timeSliderCurrentDateText;
	public float animationSpeed = 1.0f;

	public Slider animationSpeedSlider;
	public Text animationSpeedSliderText;
	public Text animationSpeedSliderTextTooltip;

	public Camera _camera;

	int defaultMarkerScale = 10;
	float minimumMarkerHeight = 5.0f;


	// Data selection
	bool showDataSelection;
	private GUIStyle rowStyle;

	public static List<string> datatyper = new List<string>();
	int valgtDatatype;


	OnlineMapsMarker3D marker;

	public Text valgtDataText;


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

		animationSpeed = 1.0f;

		//Brukes ikke før vi evt. vil skalere ALLE markers samtidig. Ligger også funksjonalitet
		// i LokalitetsBehaviour.cs
//		OnlineMaps api = OnlineMaps.instance;
//		api.OnChangeZoom += OnChangeZoom;
//		OnChangeZoom ();

		//FileReader
		fb.guiSkin = skins[0]; //set the starting skin
		fb.fileTexture = file; 
		fb.directoryTexture = folder;
		fb.driveTexture = drive;
		fb.showSearch = true;
		fb.searchRecursively = true;
		//lokaliteter[0].getMarker ().instance.
//		Material myMaterial = Resources.Load("Resources/mymat", typeof(Material)) as Material;
//		if (myMaterial != null) {
//			Debug.Log (myMaterial.color);// = new Color (255, 0, 0);
//		}

		Populate(Application.dataPath + "/Resources/06.01.2016-Lusetellinger-1712.xls");

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


				Debug.Log (output);
				if(output != "cancel hit"){
					filePath = fb.outputFile.ToString();

					//Hvis fila eksisterer og er i .xls-format
					if(File.Exists(filePath) && filePath.Trim().EndsWith(".xls")){
						Debug.Log(filePath);
						toggleFileBrowser();
						//Her skal vi kalle på Excel-metoden til arne. Vi sender med fb.outputFile.ToString() som argument.
						Populate(filePath);
					}
				}else{
					toggleFileBrowser();
				}
			}
		}


		var dataSelectionRect = new Rect (0, 30, 500, datatyper.Count * 19 + 4);

		if (showDataSelection && !dataSelectionRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) && Input.GetMouseButton(0)) {
			toggleDataSelection ();
		} else if (showDataSelection) {
			if (rowStyle == null) {
				
				rowStyle = new GUIStyle (GUI.skin.button);
				rowStyle.alignment = TextAnchor.MiddleLeft;
				rowStyle.fontSize = 12;
				RectOffset margin = rowStyle.margin;


				rowStyle.margin = new RectOffset (-margin.left, -margin.right, 1, 1);
			}


			//GUI.BeginScrollView (new Rect (0, 30, 500, Screen.height - 60), new Vector2 (0f, 0f), new Rect (0, 0, 30, datatyper.Count * 26 + 4));
			GUILayout.BeginArea (dataSelectionRect, GUI.skin.box);


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
					dataTypeChanged ();
				}
			}
			rowStyle.normal.textColor = color;
			rowStyle.hover.textColor = hoverColor;

			GUILayout.EndArea();

				//GUI.EndScrollView();

		}

		if (animationSpeedSliderTextTooltip != null && animationSpeedSliderTextTooltip.text == "Dager per sekund") {
			animationSpeedSliderTextTooltip.transform.position = new Vector3 (Input.mousePosition.x+60, Input.mousePosition.y+20, 0);
		}

	}

	//	private void OnChangeZoom(){
	//		Debug.Log (onlineMaps._zoom);
	//		foreach (OnlineMapsMarker3D m in OnlineMapsControlBase3D.instance.markers3D) {
	//			//m.scale = (onlineMaps._zoom * 2);
	//		}
	//	}

	public void dataTypeChanged(){
		valgtDataText.text = datatyper [valgtDatatype];
		oppdaterMarkers ();
	}
	void Populate (string filePath)
	{
		/*
		lokaliteter = new ArrayList ();

		lokaliteter.Add (new Lokalitet ("12394", "Ørnøya", new Vector2(63.759167f, 8.449133f)));
		lokaliteter.Add (new Lokalitet ("31959", "Rataren", new Vector2(63.782383f, 8.526367f)));
		*/
		var excelReader = new EXCELREADER ();

		lokaliteter = excelReader.readGenerellInfo (Application.dataPath + "/Resources/06.01.2016-Generell-Info.xls");
		lokaliteter = excelReader.readData (filePath, lokaliteter);

		OnlineMapsControlBase3D control = onlineMaps.GetComponent<OnlineMapsControlBase3D> ();
		control.RemoveAllMarker3D ();

		for (int i = 0; i < lokaliteter.Count; i++) {
			Lokalitet l = lokaliteter [i] as Lokalitet;


			GameObject mapObject = (GameObject)Resources.Load ("markerPrefab", typeof(GameObject));
			//GameObject mapObject = Instantiate(Resources.Load("markerPrefab", typeof(GameObject))) as GameObject;
			//mapObject.name = lokaliteter[i].getLokalitetsnavn();


			Vector2 position = l.getCoordinates ();
			marker = control.AddMarker3D (position, mapObject);
		
			marker.position = position;
			marker.label = l.getLokalitetsnavn ();
			marker.scale = defaultMarkerScale;
			marker.customData = l;

			marker.range.max = 12;
			marker.range.min = 1;
			
			control = onlineMaps.GetComponent<OnlineMapsControlBase3D> ();

			l.setMarker (marker);
			//control.AddMarker3D (marker);

			List<Enhet> enheter = l.getEnheter();

			float radius = 0.02f;

			for(int j=0; j<l.getEnheter().Count; j++){
				Enhet e = enheter[j] as Enhet;
				
				GameObject mapObjectChild = (GameObject)Resources.Load ("markerEnhetPrefab", typeof(GameObject));
				//GameObject mapObjectChild = Instantiate(Resources.Load("markerEnhetPrefab", typeof(GameObject))) as GameObject;
				//mapObjectChild.name = enheter[j].getEnhetsId();


				var angle = j * Mathf.PI * 2 / enheter.Count;
				position = l.getCoordinates ();



				marker = control.AddMarker3D (new Vector2(position.x + Mathf.Cos(angle)*radius, position.y + Mathf.Sin(angle)*radius*0.5f), mapObjectChild);

				

				marker.label = l.getLokalitetsnavn ();
				marker.scale = defaultMarkerScale;
			
				e.setMarker (marker);

				//Destroy(mapObjectChild);

			}
			//Destroy(mapObject);
		}
			
		dataTypeChanged ();

		UpdateSliderDates ();


	}

	void UpdateSliderDates() {
		dates = new List<DateTime> ();



		animationSpeedSlider = GameObject.Find ("AnimationSpeedSlider").GetComponent<Slider> ();
		animationSpeedSliderText = GameObject.Find("AnimationSpeedSliderText").GetComponent<Text>();
		animationSpeedSliderTextTooltip = GameObject.Find ("AnimationSpeedTooltipText").GetComponent<Text> ();
		currentDate = firstDate ();
		timeSlider =  GameObject.Find ("TimeSlider").GetComponent<Slider> ();

		currentDate = firstDate ();

		setTimeSliderMaxValue ();
		setTimeSliderCurrentDateText ();

	}

	// Update is called once per frame
	void Update ()
	{

	}

	private bool startAnimation ()
	{
		// Prøv å start animasjon av data
		animating = true;
		InvokeRepeating ("incrementDay", 1/animationSpeed, 1/animationSpeed);

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
				if (timeSlider.value == timeSlider.maxValue) {
					timeSlider.value = timeSlider.minValue;
				}

			}
		}
	}

	public void onAnimationSpeedChange(){
		animationSpeed = animationSpeedSlider.value;
		animationSpeedSliderText.text = String.Format ("{0:0.0}", animationSpeed);
		if (animating) {
			CancelInvoke ("incrementDay");
			InvokeRepeating ("incrementDay", 1/animationSpeed, 1/animationSpeed);
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

	public void oppdaterMarkers(){

		//Setter default høyde på alle markers

		foreach (Lokalitet l in lokaliteter) {
			l.getMarker ().instance.transform.localScale = new Vector3 ((float)defaultMarkerScale, minimumMarkerHeight, (float)defaultMarkerScale);
			float d = 10f;
			foreach (Enhet e in l.getEnheter ()) {
				e.getMarker ().instance.transform.localScale = new Vector3 ((float)defaultMarkerScale, minimumMarkerHeight, (float)defaultMarkerScale);
				try {
					d += (float)e.getSenesteMålingGittDato (currentDate).getValueForKey (datatyper[valgtDatatype]);

				} catch (Exception ex){
					//Debug.Log (ex); //Ikke enable, skaper massiv lag!
				}
				skalerMarker (e.getMarker (), d);
			}

			skalerMarker (l.getMarker (), d);
		}
	}
	public void skalerMarker(OnlineMapsMarker3D marker, float d){
		if (d < minimumMarkerHeight) {
			marker.instance.transform.localScale = new Vector3 ((float)defaultMarkerScale, minimumMarkerHeight, (float)defaultMarkerScale);
		} else {
			marker.instance.transform.localScale = new Vector3 ((float)defaultMarkerScale, d, (float)defaultMarkerScale);
		}

	}
	public void onDateChanged(){
		currentDate = (firstDate ().AddDays (timeSlider.value));
		setTimeSliderCurrentDateText ();
		Måling målingen;
		oppdaterMarkers ();
		if (timeSlider.value == timeSlider.maxValue) {
			startPauseAnimation ();
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