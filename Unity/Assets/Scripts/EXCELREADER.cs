// Hentet fra: https://github.com/ExcelDataReader/ExcelDataReader
// den 08/01-16 kl. 15:00


using UnityEngine;
using System.Collections;
using System; 
using System.IO; 
using System.Data;
using Excel;
using System.Collections.Generic;


 

public class EXCELREADER : MonoBehaviour {

	// Use this for initialization
	void Start () {


		var list = readXLS(Application.dataPath + "/Resources/06.01.2016-Lusetellinger-2.xls");


		foreach (Lokalitet l in list) {
			Debug.Log (l.ToString ());
		}



	}

	// Update is called once per frame
	void Update () {

	}

	List<Lokalitet> readXLS( string filePath)
	{

		Dictionary<string, Lokalitet> lokDict = new Dictionary<string, Lokalitet> ();
		List<string> headers = new List<string> ();



		try {
			FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

			//1. Reading from a binary Excel file ('97-2003 format; *.xls)
			IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);


			int lokNavnIndex = -1, datoIndex = -1, enhetIndex = -1;


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
						} else if (h.Equals("Utg?ende Siste dato for lusetelling")) {
							datoIndex = i;
						}
					}
				}
			}

			if (lokNavnIndex == -1 || enhetIndex == -1 || datoIndex == -1) {
			Debug.Log ("Lokalitet, Enhet eller dato er ikke med");
				return new List<Lokalitet>();
			}
			

			while (excelReader.Read())
			{

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


					


					} catch (Exception e) {
						//Debug.Log (e);
					}

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