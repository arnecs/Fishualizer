using UnityEngine;
using System.Collections;
using System; 
using System.IO; 
using System.Data;
using Excel;


 

public class EXCELREADER : MonoBehaviour {

	// Use this for initialization
	void Start () {
		readXLS(Application.dataPath + "/Resources/BasicXls.xls");
	}

	// Update is called once per frame
	void Update () {

	}

	void readXLS( string filePath)
	{

		FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

		//1. Reading from a binary Excel file ('97-2003 format; *.xls)
		IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);



		//3. DataSet - The result of each spreadsheet will be created in the result.Tables
		//DataSet result = excelReader.AsDataSet();

		//4. DataSet - Create column names from first row
		//excelReader.IsFirstRowAsColumnNames = true;
		//DataSet result = excelReader.AsDataSet();

		//5. Data Reader methods
		while (excelReader.Read())
		{
			for (int i = 0; i < excelReader.FieldCount; i++) {
				
				print (excelReader.GetString (i));
			}
		}

		//6. Free resources (IExcelDataReader is IDisposable)
		excelReader.Close();
	}
}