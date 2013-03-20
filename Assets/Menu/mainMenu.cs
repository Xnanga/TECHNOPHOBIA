using UnityEngine;
using System.Collections;

public class mainMenu : MonoBehaviour {
	
	//public string name = "";
	public int boxWidth = 200;
	public int buttonWidth = 100;
	public int textFieldWidth = 100;
	
	void OnGUI () {
		
		//GUI.DrawTexture(new Rect(0 + Screen.width/4,0 + Screen.height/4,Screen.width/2, Screen.height/2), backgroundImage);
		//GUI.DrawTexture(new Rect((Screen.width / 2 - boxWidth/2) ,(Screen.height/2 - 180/2),boxWidth,180), backgroundImage);
		GUI.backgroundColor = Color.green;
	
		GUI.Box(new Rect((Screen.width / 2 - boxWidth/2) ,(Screen.height/2 - 180/2),boxWidth,180), "Main Menu");
		
		//name = GUI.TextField (new Rect((Screen.width / 2 - textFieldWidth/2),(Screen.height/2 - 40),textFieldWidth,20), name);
		
		if(GUI.Button(new Rect((Screen.width / 2 - buttonWidth/2),(Screen.height/2 - 10),buttonWidth,20), "Start Game")) {
			Application.LoadLevel("Game");
		}
		
		if(GUI.Button(new Rect((Screen.width / 2 - buttonWidth/2),(Screen.height/2 + 20),buttonWidth,20), "Options")) {
			//Application.LoadLevel(1);
		}
		
		if(GUI.Button(new Rect((Screen.width / 2 - buttonWidth/2),(Screen.height/2 + 50), buttonWidth,20), "QUIT")){
			Application.Quit();
		}
		
		//name = GUI.TextField (new Rect((Screen.width / 2 - textFieldWidth/2),40,textFieldWidth,20), name);

	}

}
