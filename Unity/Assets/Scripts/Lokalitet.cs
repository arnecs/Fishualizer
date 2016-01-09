using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lokalitet {

	private string lokalitetsId;
	private string lokalitetsnavn;

	private Vector2 coordinates;

	private Dictionary<string, Enhet> enheter = new Dictionary<string, Enhet>();

	public Lokalitet(){
	
	}

	public Lokalitet(string lokalitetsId, string lokalitetsnavn, Vector2 coord){
		this.lokalitetsId = lokalitetsId;
		this.lokalitetsnavn = lokalitetsnavn;
		this.coordinates = coord;
	}
	

	public string getLokalitetsId(){
		return lokalitetsId;
	}

	public void setLokalitetsId(string id){
		this.lokalitetsId = id;
	}

	public string getLokalitetsnavn(){
		return lokalitetsnavn;
	}

	public void setLokalitetsNavn(string navn){
		this.lokalitetsnavn = navn;
	}

	public Vector2 getCoordinates() {
		return coordinates;
	}

	public void setCoordinates(float lengdegrad, float breddegrad) {
		coordinates = new Vector2 (lengdegrad, breddegrad);
	}
		

	public List<Enhet> getEnheter(){
		return new List<Enhet>(enheter.Values);
	}

	public Enhet leggTilEnhet(string id){
		Enhet e = new Enhet (id);
		enheter.Add (id, e);
		return e;
	}

	public Enhet getEnhetById(string id){
		Enhet e = null;
		if (enheter.TryGetValue(id, out e)) {
			return e;
		}
		return null;
	}

	public string ToString() {
		return "Lokalitetsnavn: " + lokalitetsnavn + ", Enheter.Count: " + enheter.Count;


	}


	public int AntMålinger() {
		int ant = 0;

		foreach (Enhet e in new List<Enhet>(enheter.Values)) {
			ant += e.getMålinger ().Count;
		}
		return ant;
	}
}
