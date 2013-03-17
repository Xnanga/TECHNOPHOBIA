using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSystem : MonoBehaviour {

	public GameObject spawnPoint;
	public float timeStep = 0.05f;
	public float selectedDifficulty = 0.5f;
	public float playerPerformance;
	
	int[] distributionGraph;
	public int towerCentre;
	
	void Start() {
		
		// If there's only one enemy type, spawn five of it
		if (GetComponent<Level>().avaliableEnemies.Length == 1) {
			
			GameObject enemy = GetComponent<Level>().avaliableEnemies[0];
			List<SpawnQueueEntry> spawnQueue = new List<SpawnQueueEntry>();
			for (int i = 0; i < 5; i++)
				spawnQueue.Add(new SpawnQueueEntry(enemy, Random.Range(1.5f + 0.7f * i, 3.2f + 0.7f * i)));
			spawnPoint.GetComponent<SpawnPoint>().spawnQueue = spawnQueue;
			spawnPoint.GetComponent<SpawnPoint>().zLevel = 0;
		}
		// Else spawn three of any weak/fast enemies and two stronger ones
		else {
			
			// Find weakest and second strongest (unless there are only two) enemy
			float minValue = Mathf.Infinity;
			float maxValue = 0;
			GameObject weakest = null, strongest = null, second = null;
			foreach (GameObject enemy in GetComponent<Level>().avaliableEnemies) {
				
				float speed = enemy.GetComponent<Agent>().speed;
				float health = enemy.GetComponent<Health>().maxHealth;
				float score = speed + health;
				
				if (score < minValue) {
					
					minValue = score;
					weakest = enemy;
				}
				
				if (score > maxValue) {
					
					maxValue = score;
					if (strongest != null) second = strongest;
					strongest = enemy;
				}
			}
			
			// If there's only two enemies then use strongest instead
			if (second == weakest) second = strongest;
			
			List<GameObject> enemyList = new List<GameObject> { weakest, weakest, weakest, second, second };
			List<SpawnQueueEntry> spawnQueue = new List<SpawnQueueEntry>();
			for (int i = 0; i < 5; i++) {
				
				int rnd = Random.Range(0, enemyList.Count);
				float time = Random.Range(1.5f + 0.7f * i, 3.2f + 0.7f * i);
				spawnQueue.Add(new SpawnQueueEntry(enemyList[rnd], time));
				enemyList.RemoveAt(rnd);
			}
			
			spawnPoint.GetComponent<SpawnPoint>().spawnQueue = spawnQueue;
			spawnPoint.GetComponent<SpawnPoint>().zLevel = 0;
		}
	}
	
	public void update(GameObject enemy) {
		
		int segment = enemy.GetComponent<Agent>().currentPoint;
		float t = enemy.GetComponent<Agent>().time;
		float index = (segment * (1/ timeStep)) + (t / timeStep);
		
		int distance = towerCentre - (int) index;
		
		playerPerformance += distance;
		
		if (Random.value < 0.05) {
			
			// Spawn new enemy
		}
		
		if (GetComponent<Level>().currentEnemies.Count == 0) {
			
			// Design new wave
			// Send queue to spawn point
		}
	}
	
	public void updateGraph() { // Called when tower arrangement is changed
		
		Vector3[][] path = GetComponent<Level>().path;
		float size = path.Length / timeStep;
		distributionGraph = new int[(int) size];
		
		foreach (GameObject tower in GetComponent<Level>().currentTowers) {
			
			TowerShooting towerScript = tower.GetComponent<TowerShooting>();
			if (towerScript != null) {
				
				// Iterate through seach spaces and increment distroGraph
				foreach (SearchSpace searchSpace in towerScript.searchSpace) {
					
					int steps = 0;
					for (float t = searchSpace.start; t <= searchSpace.end; t += timeStep) {
						
						float index = (searchSpace.segment * (1 / timeStep)) + steps;
						distributionGraph[(int) index]++;
						steps++;
					}
				}
			}
		}
		calculateTowerCentre();
	}
	
	void calculateTowerCentre() {
		
		// Find maximum point
		int max = 0, index = 0;
		for (int i = 0; i < distributionGraph.Length; i++) {
			if (distributionGraph[i] > max) {
				
				max = distributionGraph[i];
				index = i;
			}
		}
		
		if (index == distributionGraph.Length - 1) {
			
			towerCentre = index;
			return;
		}
		
		// Check for plateu and find median value if there is one
		int plateuLength = 1;
		for (int i = index + 1; i < distributionGraph.Length; i++) {
			
			if (distributionGraph[i] == distributionGraph[index])
				plateuLength++;
			else
				break;
		}
		
		towerCentre = (index + (index + plateuLength)) / plateuLength;
	}
}


public class SpawnQueueEntry {
	
	public GameObject enemy;
	public float time;
	
	public SpawnQueueEntry(GameObject enemy, float time) {
		
		this.enemy = enemy;
		this.time = time;
	}
}