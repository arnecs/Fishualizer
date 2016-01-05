using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Måling {

	string dato;
	List<Data> datas = new List<Data>();

	public Måling(string dato, List<Data> datas){
		this.dato = dato;
		this.datas = datas;
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
