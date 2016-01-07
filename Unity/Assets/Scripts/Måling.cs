﻿using System;
using System.Collections.Generic;

public class Måling : IComparable {

	private Dictionary<String, Double> data;
	private DateTime date;

	public Måling(DateTime date){
		this.date = date;
		data = new Dictionary<String, Double> ();
	}
	
	public void setDate(DateTime date) {
		this.date = date;
	}

	public DateTime getDate() {
		return date; 
	}

	public Double getValueForKey(String key) {
		double v;
		if (data.TryGetValue (key, out v)) {
			return v;
		}
		return -1;
	}

	public int CompareTo(Object obj) {
		Måling o = obj as Måling;
		if (o != null) {
			return date.CompareTo (o.getDate ());
		}
		return -1;

	}

}
