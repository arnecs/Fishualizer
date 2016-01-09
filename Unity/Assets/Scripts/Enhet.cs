using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Enhet
{

	private string enhetsId;
	private List<Måling> målinger = new List<Måling> ();

	public Enhet (string enhetsId, List<Måling> målinger)
	{
		this.enhetsId = enhetsId;
		this.målinger = målinger;
	}

	public Enhet (string enhetsId)
	{
		this.enhetsId = enhetsId;
	}

	public string getEnhetsId ()
	{
		return enhetsId;
	}

	public void leggTilMåling (Måling måling)
	{
		målinger.Add (måling);
	}

	public List<Måling> getMålinger ()
	{
		return målinger;
	}

	public Måling getSenesteMålingGittDato(DateTime dato){
		sorterMålinger ();
		if (målinger.Count > 0) {
			for (int i = målinger.Count - 1; i >= 0; i--) {
				if (dato.CompareTo (målinger [i].getDate ()) >= 0) {
//					Debug.Log("målinger["+i+"]"); 
//					Debug.Log (målinger [i].getDate () + " .CompareTo " +  (dato));
//					Debug.Log (målinger [i].getDate ().CompareTo (dato));
					return målinger [i];
				}
			}
		}
		return null;
	}

	public void sorterMålinger ()
	{
		målinger.Sort ();
	}

	public DateTime firstDate ()
	{
		DateTime earliestDateSoFar;
		DateTime temp;
		earliestDateSoFar = new DateTime (9000, 1, 1);
		foreach (Måling m in getMålinger()) {
			temp = m.getDate ();
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
		foreach (Måling m in getMålinger()) {
			temp = m.getDate ();
			if (latestDateSoFar.CompareTo (temp) < 0) {
				latestDateSoFar = temp;
			}
		}
		return latestDateSoFar;
	}
}