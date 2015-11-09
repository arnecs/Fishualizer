using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enhet {

	private string enhetsId;
	private string dato;
	private List<Data> datas;

	public Enhet(string enhetsId, string dato){
		this.enhetsId = enhetsId;
		this.dato = dato;
	}

	public string getEnhetsId(){
		return enhetsId;
	}

	public string getDato(){
		return dato;
	}

	public bool setData(string key, string value){
		if (key != null && value != null) {
			Data d = new Data (key, value);
			datas.Add (d);
			return true;
		}
		return false;
	}

	public List<Data> getAllData(){
		return datas;
	}

	public Data getDataByKey(string key){
		for(int i=0; i<datas.Count; i++){
			if(datas[i].getDataType().Equals(key)){
				return datas[i];
			}
		}
		return null;
	}
	
}
