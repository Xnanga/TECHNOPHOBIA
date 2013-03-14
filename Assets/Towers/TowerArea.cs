using UnityEngine;
using System.Collections;

public class TowerArea : MonoBehaviour {

	public GameObject currentTower;
	
	bool menuFlag = false;
	Vector3 mousePos;
	
	void OnGUI() {
		
		if (menuFlag) {
			
			if (currentTower == null) {
				
				float yOffset = 0;
				// Fetch list of towers.
				foreach (GameObject tower in GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().avaliableTowers) {
					
					// Display tower build button
					if (GUI.Button(new Rect(mousePos.x, mousePos.y + yOffset, 120, 30), tower.name)) {
						
						currentTower = (GameObject) Instantiate(tower, gameObject.transform.position, tower.transform.rotation);
						GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().currentTowers.Add(currentTower);
						GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnSystem>().updateGraph();
					}
					yOffset += 35;
				}
				
				// Close button
				if (GUI.Button(new Rect(mousePos.x, mousePos.y + yOffset, 120, 30), "Close")) {
					
					GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().disableTowerMenu();
					toggleMenu();
				}
			}
			else {
				
				// Draw upgrade menu
			}
		}
	}
	
	public void toggleMenu() {
		
		menuFlag = !menuFlag;
		mousePos = Input.mousePosition;
		mousePos.y = Screen.height - mousePos.y;
	}
}
