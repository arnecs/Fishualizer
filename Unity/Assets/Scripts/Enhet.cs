using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enhet {

	private string enhetsId;
	private List<Måling> målinger = new List<Måling>();

	public Enhet(string enhetsId, List<Måling> målinger){
		this.enhetsId = enhetsId;
		this.målinger = målinger;
	}

	public Enhet(string enhetsId){
		this.enhetsId = enhetsId;
	}

	public string getEnhetsId(){
		return enhetsId;
	}

	public void leggTilMåling(Måling måling){
		målinger.Add (måling);
	}

	public List<Måling> getMålinger(){
		return målinger;
	}

	public void sorterMålinger() {
		målinger.Sort ();
	}
}