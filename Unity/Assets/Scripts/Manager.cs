using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

using InfinityCode;
using AssemblyCSharp;


public class Manager : MonoBehaviour
{

	public static ArrayList lokaliteter;

	public OnlineMaps onlineMaps;
	public Button playPauseButton;
	public Slider slider;

	public InputField searchField;

	public Camera _camera;
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
		lokaliteter = new ArrayList ();
		Populate ();
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
	}

	//	private void OnChangeZoom(){
	//		Debug.Log (onlineMaps._zoom);
	//		foreach (OnlineMapsMarker3D m in OnlineMapsControlBase3D.instance.markers3D) {
	//			//m.scale = (onlineMaps._zoom * 2);
	//		}
	//	}

	void Populate ()
	{
		
		lokaliteter = new ArrayList ();

		lokaliteter.Add (new Lokalitet ("12394", "Ørnøya", new Vector2(63.759167f, 8.449133f)));
		lokaliteter.Add (new Lokalitet ("31959", "Rataren", new Vector2(63.782383f, 8.526367f)));

		for (int i = 0; i < lokaliteter.Count; i++) {
			Lokalitet l = lokaliteter [i] as Lokalitet;

			GameObject cylinder = (GameObject)Resources.Load ("markerPrefab", typeof(GameObject));



			marker = new OnlineMapsMarker3D (cylinder);
			Vector2 position = l.getCoordinates ();
		
			marker.position = position;
			marker.label = l.getLokalitetsnavn ();

			marker.scale = 20;

			marker.position = position;
			marker.label = l.getLokalitetsnavn ();
			marker.scale = onlineMaps._zoom * 2;

			OnlineMapsControlBase3D control = onlineMaps.GetComponent<OnlineMapsControlBase3D> ();

			control.AddMarker3D (marker);

		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log("Update");

		//Debug.Log (onlineMaps._zoom);


		//OnlineMapsControlBase3D.instance.markers3D[0] .scale += 0.02f;
		//OnlineMapsControlBase3D.instance.markers3D[1] .scale += 0.05f;

	
	}

	private bool startAnimation ()
	{
		// Prøv å start animasjon av data
		animating = true;

		return true;
	}

	private bool stopAnimation ()
	{
		// Stop animasjon av data
		animating = false;

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
		}
	}

	public DateTime firstDate ()
	{
		DateTime earliestDateSoFar;
		earliestDateSoFar = new DateTime (0, 0, 0);
		if (lokaliteter != null) {
			foreach (Lokalitet l in lokaliteter) {
				foreach (Enhet e in l.getEnheter ()) {
					foreach (Måling m in e.getMålinger()) {
						//compare dates
					}
				}
			}
		}
		return earliestDateSoFar;

	}

	public DateTime lastDate ()
	{
		DateTime latestDateSoFar;
		latestDateSoFar = new DateTime (0, 0, 0);
		return latestDateSoFar;
	}

	public int totalDates ()
	{
		if (lastDate () != null && firstDate () != null) {
			return (int)(lastDate () - firstDate ()).TotalDays;
		}
		return 0;
	}
}