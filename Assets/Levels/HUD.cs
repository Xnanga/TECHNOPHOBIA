using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	void OnGUI() {
		
		GameData gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
		Level level = GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>();
		GUI.Box(new Rect(Screen.width/2 - 130/2 + -150,5,130,20), "Lives: " + gameData.playerHealth);
		GUI.Box(new Rect(Screen.width/2 - 130/2 + 0,5,130,20), "Score: " + gameData.score);
		GUI.Box(new Rect(Screen.width/2 - 130/2 + 150,5,130,20), "Scrap: " + gameData.scrap);
		GUI.Box(new Rect(Screen.width/2 - 130/2 + 300,5,130,20), "Wave: " + level.currentWave + " / " + level.waveCount);
		
		if (GUI.Button(new Rect(10,5,100,20), "Main Menu")){
			Application.LoadLevel("Main Menu");}
	}
}
