using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileReader : MonoBehaviour {
	
	void Awake() {
		List<Dictionary<string,object>> data = CSVReader.Read ("Lusetellinger");

		//Debug.Log (data.Count);
		for (int i=0; i< data.Count; i++) {
			foreach (string key in data[i].Keys) {
				object val = data[i][key];
				Debug.Log(key + "  " + val);
			}
		}
	}
}
