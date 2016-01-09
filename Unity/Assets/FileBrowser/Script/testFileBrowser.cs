using UnityEngine;
using System.Collections;
using System.IO;

//Hentet fra https://www.assetstore.unity3d.com/en/#!/content/18308
// den 12.12.2015

public class testFileBrowser : MonoBehaviour {
	//skins and textures
	public GUISkin[] skins;
	public Texture2D file,folder,back,drive;
	
	string[] layoutTypes = {"Type 0","Type 1"};
	//initialize file browser
	FileBrowser fb = new FileBrowser();
	string output = "no file";
	// Use this for initialization
	void Start () {
		//setup file browser style
		fb.guiSkin = skins[0]; //set the starting skin
		//set the various textures
		fb.fileTexture = file; 
		fb.directoryTexture = folder;
		fb.backTexture = back;
		fb.driveTexture = drive;

		fb.showSearch = true;

		//search recursively (setting recursive search may cause a long delay)
		fb.searchRecursively = true;
	}
	
	void OnGUI(){
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Space(10);
		//GUILayout.Label("Selected File: "+output);
		GUILayout.EndHorizontal();
		//draw and display output
		if(fb.draw()){ //true is returned when a file has been selected
			//the output file is a member if the FileInfo class, if cancel was selected the value is null
			output = (fb.outputFile==null)?"cancel hit":fb.outputFile.ToString();
			//TextAsset t = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(fb.outputFile.ToString());
			string filePath = fb.outputFile.ToString();

			//Hvis fila eksisterer og er i .xls-format
			if(File.Exists(filePath) && filePath.Trim().EndsWith(".xls")){
				//Her skal vi kalle på Excel-metoden til arne. Vi sender med fb.outputFile.ToString() som argument.
			}
		}
	}
}
