using UnityEngine;
using System.Collections;
using System.IO;

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
		//show the search bar
		fb.showSearch = true;
		//search recursively (setting recursive search may cause a long delay)
		fb.searchRecursively = true;
	}
	
	void OnGUI(){
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Label("Layout Type");
		fb.setLayout(GUILayout.SelectionGrid(fb.layoutType,layoutTypes,1));
		GUILayout.Space(10);
		//select from available gui skins
		GUILayout.Label("GUISkin");
		foreach(GUISkin s in skins){
			if(GUILayout.Button(s.name)){
				fb.guiSkin = s;
			}
		}
		GUILayout.Space(10);
		fb.showSearch = GUILayout.Toggle(fb.showSearch,"Show Search Bar");
		fb.searchRecursively = GUILayout.Toggle(fb.searchRecursively,"Search Sub Folders");
		GUILayout.EndVertical();
		GUILayout.Space(10);
		GUILayout.Label("Selected File: "+output);
		GUILayout.EndHorizontal();
		//draw and display output
		if(fb.draw()){ //true is returned when a file has been selected
			//the output file is a member if the FileInfo class, if cancel was selected the value is null
			output = (fb.outputFile==null)?"cancel hit":fb.outputFile.ToString();
			//TextAsset t = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(fb.outputFile.ToString());
			string s = Read(fb.outputFile.ToString());

			if(s != null){
				/* Midlertidig kommentert ut mens alternativet XlsReader prøves ut
				FileReader f = new FileReader();
				f.Parser(s);
				*/

				//Her skal vi kalle på Excel-metoden til arne. Vi sender med fb.outputFile.ToString() som argument.
			}
		}
	}

	public string Read(string filePath)
	{
		if (!File.Exists (filePath)) {
			return null;
		}

		string readText = File.ReadAllText(filePath);
		Debug.Log (readText);
		
		return readText;
	}
}
