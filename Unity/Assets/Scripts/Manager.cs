using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

using InfinityCode;
using AssemblyCSharp;
using UnityEditor;
using System.Security.Cryptography.X509Certificates;


public class Manager : MonoBehaviour
{

	public static List<Lokalitet> lokaliteter;

	public OnlineMaps onlineMaps;

	public List<DateTime> dates;
	public static DateTime currentDate;

	public Button playPauseButton;
	public Slider slider;
	public InputField searchField;
	public Slider timeSlider;
	public Text timeSliderCurrentDateText;


	public Camera _camera;



	OnlineMapsMarker3D marker;

	
	bool animating;

	// Use this for initialization
	void Start ()
	{
		lokaliteter = new List<Lokalitet> ();

		dates = new List<DateTime> ();
		currentDate = firstDate ();
		GameObject temp = GameObject.Find ("TimeSlider");
		if (temp != null) {
			timeSlider = temp.GetComponent<Slider> ();
		}

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

		//Debug.Log (currentDate);
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
		//Debug.Log (currentDate.ToString ("yyyy-MM-dd"));
	}
}