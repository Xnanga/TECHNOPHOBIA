using UnityEngine;
using System.Collections;

public class TowerArea : MonoBehaviour {

	public GameObject currentTower;
	public bool menuFlag = false;
	
	void OnGUI() {
		
		if (menuFlag) {
			
			if (currentTower == null) {
				
				float yOffset = 0;
				// Fetch list of towers.
				foreach (GameObject tower in GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().avaliableTowers) {
					
					// Display tower build button
					if (GUI.Button(new Rect(Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position).x,
						Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position).y + yOffset, 120, 30), tower.name)) {
						
						currentTower = (GameObject) Instantiate(tower, gameObject.transform.position, tower.transform.rotation);
						GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().currentTowers.Add(currentTower);
					}
					yOffset += 35;
				}
				
				// Close button
				if (GUI.Button(new Rect(Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position).x,
					Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position).y + yOffset, 120, 30), "Close")) {
					
					GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().disableTowerMenu();
				}
			}
			else {
				
				// Draw upgrade menu
			}
		}
	}
}
