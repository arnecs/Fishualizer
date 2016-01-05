using UnityEngine;
using System.Collections;

public class Enhet {

	private string enhetsId;
	private Data[] datas;

	public Enhet(string enhetsId, Data[] datas){
		this.enhetsId = enhetsId;
		this.datas = datas;
	}

	public string getEnhetsId(){
		return enhetsId;
	}

	public Data[] getAllData(){
		return datas;
	}

	public Data getDataByKey(string key){
		for(int i=0; i<datas.Length; i++){
			if(datas[i].getDataType().Equals(key)){
				return datas[i];
			}
		}
		return null;
	}
	
}
