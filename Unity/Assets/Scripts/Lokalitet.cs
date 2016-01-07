using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lokalitet {

	private string lokalitetsId;
	private string lokalitetsnavn;
	private double breddegrad;
	private double lengdegrad;
	private List<Enhet> enheter = new List<Enhet>();

	public Lokalitet(){
	
	}

	public Lokalitet(string lokalitetsId, string lokalitetsnavn, double breddegrad, double lengdegrad, List<Enhet> enheter){
		this.lokalitetsId = lokalitetsId;
		this.lokalitetsnavn = lokalitetsnavn;
		this.breddegrad = breddegrad;
		this.lengdegrad = lengdegrad;
		this.enheter = enheter;
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

	public double getBreddegrad(){
		return breddegrad;
	}

	public double getLengdegrad(){
		return lengdegrad;
	}

	public List<Enhet> getEnheter(){
		return enheter;
	}

	public void leggTilEnhet(string id){
		Enhet e = new Enhet (id);
		enheter.Add (e);
	}

	public Enhet getEnhetById(string id){
		for(int i=0; i<enheter.Count; i++){
			if(enheter[i].getEnhetsId().Equals(id)){
				return enheter[i];
			}
		}
		return null;
	}
}
