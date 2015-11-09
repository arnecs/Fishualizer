using UnityEngine;
using System.Collections;

public class Lokalitet {

	private int lokalitetsnummer;
	private string lokalitetsnavn;
	private double breddegrad;
	private double lengdegrad;
	private Enhet[] enheter;

	public Lokalitet(int lokalitetsnummer, string lokalitetsnavn, double breddegrad, double lengdegrad, Enhet[] enheter){
		this.lokalitetsnummer = lokalitetsnummer;
		this.lokalitetsnavn = lokalitetsnavn;
		this.breddegrad = breddegrad;
		this.lengdegrad = lengdegrad;
		this.enheter = enheter;
	}

	public int getLokalitetsnummmer(){
		return lokalitetsnummer;
	}

	public string getLokalitetsnavn(){
		return lokalitetsnavn;
	}

	public double getBreddegrad(){
		return breddegrad;
	}

	public double getLengdegrad(){
		return lengdegrad;
	}

	public Enhet[] getEnheter(){
		return enheter;
	}

	public Enhet getEnhetById(string id){
		for(int i=0; i<enheter.Length; i++){
			if(enheter[i].getEnhetsId().Equals(id)){
				return enheter[i];
			}
		}
		return null;
	}
}
