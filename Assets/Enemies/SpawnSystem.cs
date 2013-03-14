using UnityEngine;
using System.Collections;

public class SpawnSystem : MonoBehaviour {

	public GameObject[] spawnPoints;
	public float selectedDifficulty;
	public float playerPerformance;
	
	void Update() {
		
		
	}
	
	public void updateGraph() {
		
		Vector3[][] path = GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().path;
		float[] distributionGraph = new float[path.Length];
		
		foreach (GameObject tower in GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().currentTowers) {
			
			TowerShooting towerScript = tower.GetComponent<TowerShooting>();
			if (towerScript != null) {
				
				
			}
		}
	}
}


public class SpawnQueueEntry {
	
	public GameObject enemy;
	public float time;
}