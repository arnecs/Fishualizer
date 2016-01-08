using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using AssemblyCSharp;
using System;

public class TimeSlider : MonoBehaviour {
	public Slider timeSlider;
	public Måling måling;
	// Use this for initialization
	void Start () {
		måling = new Måling ();
		//målinger.regDate ();
		Debug.Log (måling.getCount ());


		GameObject temp = GameObject.Find ("TimeSlider");
		if (temp != null) {
			timeSlider = temp.GetComponent<Slider> ();
			timeSlider.maxValue = måling.getCount ();
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
