using UnityEngine;
using System.Collections;

public class TowerArea : MonoBehaviour {

	public GameObject currentTower;
	
	bool menuFlag = false;
	Vector3 mousePos;
	bool down = true;
	
	void OnGUI() {
		
		if (menuFlag) {
			
			if (currentTower == null) {
				
				float yOffset = 0;
				if (!down) yOffset = -35;
				// Fetch list of towers.
				foreach (GameObject tower in GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().avaliableTowers) {
					
					// Display tower build button
					if (GUI.Button(new Rect(mousePos.x, mousePos.y + yOffset, 200, 30), tower.name + ": " + tower.GetComponent<Tower>().cost) &&
						GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().scrap >= tower.GetComponent<Tower>().cost) {
						
						currentTower = (GameObject) Instantiate(tower, gameObject.transform.position, tower.transform.rotation);
						GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().currentTowers.Add(currentTower);
						GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().scrap -= tower.GetComponent<Tower>().cost;
					}
					if (down) yOffset += 35;
					else yOffset -= 35;
				}
				
				// Close button
				if (GUI.Button(new Rect(mousePos.x, mousePos.y + yOffset, 200, 30), "Close")) {
					
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
		
		if (mousePos.x > Screen.width / 2) mousePos.x -= 200;
		if (mousePos.y > Screen.width / 2) down = false;
	}
}
