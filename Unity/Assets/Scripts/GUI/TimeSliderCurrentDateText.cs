using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSliderCurrentDateText : MonoBehaviour {
	public Slider timeSlider;
	public Text currentDateText;
	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find ("TimeSlider");
		if (temp != null) {
			timeSlider = temp.GetComponent<Slider> ();
		}

		temp = GameObject.Find ("CurrentDate");
		if (temp != null) {
			currentDateText = temp.GetComponent<Text> ();
		}
		Debug.Log (currentDateText.text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
