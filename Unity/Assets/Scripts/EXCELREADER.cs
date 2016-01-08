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
		readXLS(Application.dataPath + "/Resources/BasicXls.xls");
	}

	// Update is called once per frame
	void Update () {

	}

	List<Lokalitet> readXLS( string filePath)
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
					headers.Add(excelReader.GetString(i), i);

				}
			}
			

			while (excelReader.Read())
			{
				int lokIdIndeks = -1;
				Lokalitet lok = null;
				string lokNavn;

				// Finn riktig lokalitet
				if (headers.TryGetValue("Lokalitet", out lokIdIndeks)) {
					lokNavn = excelReader.GetString(lokIdIndeks);

					if (!lokDict.TryGetValue(lokNavn, out lok)) {

						lok = new Lokalitet();
						lok.setLokalitetsNavn(lokNavn);

					}
				}

				Enhet enhet;
				int enhetIndeks;
				string enhetId;

				// Finn riktig enhet
				if (headers.TryGetValue("Enhet", out enhetIndeks)) {
					enhetId = excelReader.GetString(enhetIndeks);

					enhet = lok.getEnhetById(enhetId);

					if (enhet == null) {
						enhet = lok.leggTilEnhet(enhetId);
					}
				}
					
				for (int i = 0; i < excelReader.FieldCount; i++) {
					

				}
			}

			//6. Free resources (IExcelDataReader is IDisposable)
			excelReader.Close();
			} catch (Exception e) {
				
			}

		return null;
		}
}