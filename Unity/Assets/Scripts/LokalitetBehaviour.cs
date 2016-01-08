using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;



public class LokalitetBehaviour : MonoBehaviour {
	OnlineMaps api;
	// Use this for initialization
	void Start () {
		
		api = OnlineMaps.instance;
		api.OnChangeZoom += OnChangeZoom;
		OnChangeZoom ();
	}

	private void OnChangeZoom(){
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

	}

	void Update(){
	}
}