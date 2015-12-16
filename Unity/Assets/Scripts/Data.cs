using UnityEngine;
using System.Collections;

public class Data {

	private string dataType;
	private string value;

	public Data(string dataType, string value){
		this.dataType = dataType;
		this.value = value;
	}

	public string getDataType(){
		return dataType;
	}

	public string getValue(){
		return value;
	}
}
