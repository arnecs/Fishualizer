// Hentet fra: https://github.com/ExcelDataReader/ExcelDataReader
// den 08/01-16 kl. 15:00


using UnityEngine;
using System.Collections;
using System; 
using System.IO; 
using System.Data;
using Excel;
using System.Collections.Generic;


 

public class EXCELREADER {

	// Use this for initialization
	void Start () {

		var list = readGenerellInfo(Application.dataPath + "/Resources/06.01.2016-Generell-Info.xls");


		int antMålinger = 0;

		foreach (Lokalitet l in list) {
			Debug.Log (l.ToString ());
			antMålinger += l.AntMålinger ();
		}

	//	Debug.Log ("Antall Målinger: " + antMålinger);

	}

	public List<Lokalitet> readGenerellInfo( string filePath)
	{

		Dictionary<string, Lokalitet> lokDict = new Dictionary<string, Lokalitet> ();
		Dictionary<string, int> headers = new Dictionary<string, int> ();


		try {
			FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

			//1. Reading from a binary Excel file ('97-2003 format; *.xls)
			IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);


			excelReader.Read();
			if (excelReader.Read()) {
				for (int i = 0; i < excelReader.FieldCount; i++) {

					string h = excelReader.GetString(i);

					//Debug.Log(h);

					if (h != null) {
						headers.Add(h, i);
					
					}
				}
			}


			while (excelReader.Read())
			{
				try {


					Lokalitet lok = null;

					// Finn riktig lokalitet
					int index = -1;
					if (headers.TryGetValue("LokalitetsID", out index)) {
						string lokNavn = excelReader.GetString(index);

						if (!lokDict.TryGetValue(lokNavn, out lok)) {

							lok = new Lokalitet();
							lok.setLokalitetsNavn(lokNavn);

							lokDict.Add(lokNavn, lok);

						}
					}

					index = -1;
					if (headers.TryGetValue("Lokalitet", out index)) {
						string loknavn = excelReader.GetString(index);

						lok.setLokalitetsNavn(loknavn);
					}



					Enhet enhet;
					// Finn riktig enhet

					index = -1;
					if (headers.TryGetValue("Enhet", out index)) {
						string enhetId = excelReader.GetString(index);

						enhet = lok.getEnhetById(enhetId);
						if (enhet == null) {
							enhet = lok.leggTilEnhet(enhetId);
						}
					}

					int bredIndex = -1;
					int lengIndex = -1;
					if (headers.TryGetValue("Lengdegrad", out lengIndex) && headers.TryGetValue("Breddegrad", out bredIndex)) {
						try {
							float bredde = excelReader.GetFloat(bredIndex);
							float lengde = excelReader.GetFloat(lengIndex);

							lok.setCoordinates(lengde, bredde);
						} catch (Exception e) {
							
						}
					}

		
				} catch (Exception e) {
					Debug.Log(e);
				}
			}

			//6. Free resources (IExcelDataReader is IDisposable)
			excelReader.Close();
		} catch (Exception e) {
			Debug.Log (e);
		}

		return new List<Lokalitet>(lokDict.Values);
	}

	public List<Lokalitet> readData( string filePath, List<Lokalitet> lokaliteter)
	{

		Dictionary<string, Lokalitet> lokDict = new Dictionary<string, Lokalitet> ();
		List<string> headers = new List<string> ();

		if (lokaliteter != null) {
			foreach (var l in lokaliteter) {
				lokDict.Add (l.getLokalitetsnavn (), l);
			}
		}



		try {
			FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

			//1. Reading from a binary Excel file ('97-2003 format; *.xls)
			IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);


			int lokNavnIndex = -1, datoIndex = -1, enhetIndex = -1, antLusTellIndex = -1;


			excelReader.Read();
			if (excelReader.Read()) {
				for (int i = 0; i < excelReader.FieldCount; i++) {

					string h = excelReader.GetString(i);

					//Debug.Log(h);

					if (h != null) {
						headers.Add(h);
						if (h.Equals("Lokalitet")) {
							lokNavnIndex = i;
						} else if (h.Equals("Enhet")){
							enhetIndex = i;
						} else if (h.Equals("Utg?ende Siste dato for lusetelling") || h.Equals ("Utgående Siste dato for lusetelling")) {
							datoIndex = i;
						} else if (h.Equals("Antall lusetellinger i perioden")) {
							antLusTellIndex = i;
						}
					}
				}
			}

			if (lokNavnIndex == -1 || enhetIndex == -1 || datoIndex == -1 || antLusTellIndex == -1) {
			Debug.Log ("Lokalitet, Enhet, Dato eller Antall Lusetellinger er ikke med");
				return new List<Lokalitet>();
			}
			

			while (excelReader.Read())
			{
				try {

					int antLusTell = excelReader.GetInt32(antLusTellIndex);

					if (antLusTell <= 0) {
						continue;
					}


				Lokalitet lok = null;
				string lokNavn;

				// Finn riktig lokalitet
				lokNavn = excelReader.GetString(lokNavnIndex);

				if (!lokDict.TryGetValue(lokNavn, out lok)) {

					lok = new Lokalitet();
					lok.setLokalitetsNavn(lokNavn);

					lokDict.Add(lokNavn, lok);

				}


				Enhet enhet;
				string enhetId;

				// Finn riktig enhet

				enhetId = excelReader.GetString(enhetIndex);

				enhet = lok.getEnhetById(enhetId);

				if (enhet == null) {
					enhet = lok.leggTilEnhet(enhetId);
				}

			
				



				DateTime dato;
				dato = excelReader.GetDateTime(datoIndex);
				Måling m = new Måling(dato);

				for (int i = 0; i < excelReader.FieldCount; i++) {
					try {
						double data = excelReader.GetDouble(i);


						m.AddData(headers[i], data);

						//.Log("Måling lagt til: " + headers[i] + ", " + data);
					


					} catch (Exception e) {
						//Debug.Log ("Måling ikke lagt til: " + e);
					}

				}
				enhet.leggTilMåling(m);
				} catch (Exception e) {
					Debug.Log(e);
				}
			}

			//6. Free resources (IExcelDataReader is IDisposable)
			excelReader.Close();
			} catch (Exception e) {
			Debug.Log (e);
			}

		return new List<Lokalitet>(lokDict.Values);
		}
}