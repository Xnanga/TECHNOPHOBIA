using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
	
	public Vector3[][] path;
	
	// Avaliable enemy/tower lists
	public GameObject[] avaliableEnemies;
	public GameObject[] avaliableTowers;
	
	// Current enemy/tower lists
	public List<GameObject> currentEnemies = new List<GameObject>();
	public List<GameObject> currentTowers = new List<GameObject>();
	
	// Currently selected tower area
	GameObject selectedArea;
	
	public void registerPaths() {
		
		path = GameObject.FindGameObjectWithTag("Path").GetComponent<Path>().path;
	}
	
	void Update() {
		
		// Targeting system
		
		// Tower placement system
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit) && hit.collider.tag == "TowerArea") {
			
			// Disable any previously selected tower area
			if (selectedArea != null) selectedArea.GetComponent<TowerArea>().toggleMenu();
			hit.transform.GetComponent<TowerArea>().toggleMenu();
			// Record the above tower area
			selectedArea = hit.transform.gameObject;
		}
	}
	
	public void disableTowerMenu() {
		
		selectedArea = null;
	}
	
	public void reportDead(GameObject enemy) {
		
		currentEnemies.Remove(enemy);
		//gameObject.GetComponent<TargetingSystem>().updatePriorityList();
		gameObject.GetComponent<SpawnSystem>().update(Enemy);
	}
}
