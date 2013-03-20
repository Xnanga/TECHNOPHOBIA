using UnityEngine;
using System.Collections;

public class end : MonoBehaviour {
	
	public int boxWidth = 200;
	public int buttonWidth = 100;
	public int textFieldWidth = 100;
	
	void OnGUI() {
		
		// Display score
		// Back to menu
		// Quit
		
		GUI.backgroundColor = Color.green;
	
		GUI.Box(new Rect((Screen.width / 2 - boxWidth/2) ,(Screen.height/2 - 180/2),boxWidth,180), "Game Over");
		
		//name = GUI.TextField (new Rect((Screen.width / 2 - textFieldWidth/2),(Screen.height/2 - 40),textFieldWidth,20), name);
		GUI.Label(new Rect((Screen.width / 2 - textFieldWidth/2),(Screen.height/2 - 40),textFieldWidth,20),
			"Score: " + GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().score);
		
		if(GUI.Button(new Rect((Screen.width / 2 - buttonWidth/2),(Screen.height/2 - 10),buttonWidth,20), "Main Menu")) {
			Application.LoadLevel("Main Menu");
		}
		
		/*if(GUI.Button(new Rect((Screen.width / 2 - buttonWidth/2),(Screen.height/2 + 20),buttonWidth,20), "Leaderboard")) {
			//Application.LoadLevel(1);
		}*/
		
		if(GUI.Button(new Rect((Screen.width / 2 - buttonWidth/2),(Screen.height/2 + 20), buttonWidth,20), "QUIT")){
			Application.Quit();
		}
	}
}
