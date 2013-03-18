using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSystem : MonoBehaviour {

	public GameObject spawnPoint;
	public float timeStep = 0.05f;
	public float selectedDifficulty = 0.5f;
	public float playerPerformance;
	
	Dictionary<GameObject, float> enemyScores = new Dictionary<GameObject, float>();
	int spawnDispersion;
	
	int[] distributionGraph;
	public int towerCentre;
	
	void Start() {
		
		rateEnemies();
		
		// If there's only one enemy type, spawn five of it
		if (enemyScores.Count == 1) {
			
			GameObject enemy = GetComponent<Level>().avaliableEnemies[0];
			List<SpawnQueueEntry> spawnQueue = new List<SpawnQueueEntry>();
			for (int i = 0; i < 5; i++)
				spawnQueue.Add(new SpawnQueueEntry(enemy, Random.Range(1.5f + 0.7f * i, 2.2f + 0.7f * i)));
			spawnPoint.GetComponent<SpawnPoint>().spawnQueue.AddRange(spawnQueue);
		}
		// Else spawn three of any weak/fast enemies and two stronger ones
		else {
			
			// Find weakest and second strongest (unless there are only two) enemy
			float minValue = Mathf.Infinity;
			float maxValue = 0;
			GameObject weakest = null, strongest = null, second = null;
			foreach (GameObject enemy in enemyScores.Keys) {
				
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
				float time = Random.Range(1.5f + 0.7f * i, 2.2f + 0.7f * i);
				spawnQueue.Add(new SpawnQueueEntry(enemyList[rnd], time));
				enemyList.RemoveAt(rnd);
			}
			
			spawnPoint.GetComponent<SpawnPoint>().spawnQueue.AddRange(spawnQueue);
		}
	}
	
	public void update(GameObject enemy) {
		
		// Check if is at end
		// Down perfromance by something
		
		int segment = enemy.GetComponent<Agent>().currentPoint;
		float t = enemy.GetComponent<Agent>().time;
		float index = (segment * (1/ timeStep)) + (t / timeStep);
		
		int distance = towerCentre - (int) index;
		
		playerPerformance += distance;
		
		if (Random.value < 0.4) {
			
			// Spawn new enemy
			float max = 0;
			foreach (GameObject thing in enemyScores.Keys)
				max += enemyScores[thing] + (selectedDifficulty * enemyScores[thing]);
			float rnd = Random.Range(0, max);
			
			float consecutiveScore = 0;
			foreach (GameObject thing in enemyScores.Keys) {
				
				consecutiveScore += enemyScores[thing] + (selectedDifficulty * enemyScores[thing]);
				if (rnd < consecutiveScore) {
					
					// Spawn it!
					float time = Random.Range(1.5f + 0.7f * spawnDispersion, 2.2f + 0.7f * spawnDispersion);
					spawnPoint.GetComponent<SpawnPoint>().spawnQueue.Add(new SpawnQueueEntry(thing, time));
					if (spawnDispersion++ == 5) spawnDispersion = 0;
					break;
				}
			}
		}
		
		if (GetComponent<Level>().currentEnemies.Count == 0) {
			
			// Design new wave
			List<SpawnQueueEntry> spawnQueue = new List<SpawnQueueEntry>();
			
			// Select number of enemies to spawn
			// Random number multiplied by (performance * 1 / difficulty)
			float modifier = Mathf.Abs(playerPerformance * Mathf.Pow(selectedDifficulty, 2));
			float num = Random.Range(4.0f + 0.1f * modifier, 6.0f + 0.1f * modifier);
			
			if (num > 30) num = 30;
			
			for (int i = 0; i < (int) num; i++) {
				
				// Select enemy and spawn
				float max = 0;
				foreach (GameObject thing in enemyScores.Keys)
					max += enemyScores[thing] + (selectedDifficulty * enemyScores[thing]);
				float rnd = Random.Range(0, max);
				
				float consecutiveScore = 0;
				foreach (GameObject thing in enemyScores.Keys) {
					
					consecutiveScore += enemyScores[thing] + (selectedDifficulty * enemyScores[thing]);
					if (rnd < consecutiveScore) {
						
						// Spawn it!
						float time = Random.Range(1.5f + 0.7f * spawnDispersion, 2.2f + 0.7f * spawnDispersion);
						spawnPoint.GetComponent<SpawnPoint>().spawnQueue.Add(new SpawnQueueEntry(thing, time));
						if (spawnDispersion++ == 15) spawnDispersion = 0;
						break;
					}
				}
			}
			
			// Send queue to spawn point
			spawnPoint.GetComponent<SpawnPoint>().spawnQueue.AddRange(spawnQueue);
			
			// mutate difficulty and reset performance
			selectedDifficulty = selectedDifficulty * (playerPerformance * 0.1f);
			/////////////////////////////////////////////////////////////////////
			/////////////////The above needs to be redone - it goes up infinetly
			/////////////////////////////////////////////////////////////////////
			playerPerformance = 0;
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
		
		// Find maximum points
		Dictionary<int, int> distroAnalysis = new Dictionary<int, int>(); // strating index, length
		int max = 0, index = 0;
		bool fall = true;
		for (int i = 0; i < distributionGraph.Length; i++) {
			
			if ((fall && distributionGraph[i] >= max) ||
				(!fall && distributionGraph[i] > max)) {
				
				max = distributionGraph[i];
				index = i;
				distroAnalysis.Add(index, 0);
				fall = false;
			}
			else if (!fall && distributionGraph[i] == max)
				distroAnalysis[index]++;
			else if (distributionGraph[i] < max)
				fall = true;
		}
		
		// Find maximum, remove all others
		List<int> toBeRemoved = new List<int>();
		foreach (int point in distroAnalysis.Keys)
			if (distributionGraph[point] != max) toBeRemoved.Add(point);
		foreach (int point in toBeRemoved)
			distroAnalysis.Remove(point);
		
		// Find point with largest spread
		int largest = 0;
		index = 0;
		foreach (KeyValuePair<int, int> point in distroAnalysis) {
			
			if (point.Value > largest) {
				
				index = point.Key;
				largest = point.Value;
			}
		}
		
		// Return mid point of the above
		towerCentre = (index + (index + distroAnalysis[index])) / 2;
		
		/*
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
		
		towerCentre = (index + (index + plateuLength)) / plateuLength;*/
	}
	
	private void rateEnemies() {
		
		// Round up enemies, calculate total score (speed + health)
		float totalScore = 0;
		foreach (GameObject enemy in GetComponent<Level>().avaliableEnemies) {
			
			float speed = enemy.GetComponent<Agent>().speed;
			float health = enemy.GetComponent<Health>().maxHealth;
			float score = speed + health;
			
			totalScore += score;
		}
		
		// Make list of (total - score) for each enemy
		enemyScores = new Dictionary<GameObject, float>();
		int i = 0;
		foreach (GameObject enemy in GetComponent<Level>().avaliableEnemies) {
			
			float speed = enemy.GetComponent<Agent>().speed;
			float health = enemy.GetComponent<Health>().maxHealth;
			float score = speed + health;
			
			enemyScores.Add(GetComponent<Level>().avaliableEnemies[i], totalScore - score);
			i++;
		}
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