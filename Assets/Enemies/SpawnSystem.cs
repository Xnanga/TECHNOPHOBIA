using UnityEngine;
using System.Collections;

public class SpawnSystem : MonoBehaviour {

	public GameObject spawnPoint;
	public float timeStep = 0.05f;
	public float selectedDifficulty = 0.5f;
	public float playerPerformance;
	
	int[] distributionGraph;
	public int towerCentre;
	
	void Start() {
		
		// Spawn initial wave
	}
	
	public void update(GameObject enemy) {
		
		// Called when an enemy dies
		// Evaluate enemy's position on the distroGraph
		// Increment/Decrement performance rating appropriately
		
		// If all enemies are dead
		// Design new wave
		// Send new wave data to spawn points
	}
	
	public void updateGraph() { // Called when tower arrangement is changed
		
		Vector3[][] path = gameObject.GetComponent<Level>().path;
		float size = path.Length / timeStep;
		distributionGraph = new int[(int) size];
		
		foreach (GameObject tower in gameObject.GetComponent<Level>().currentTowers) {
			
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
		
		/*// Calculate standard deviation
		
		// Find mean
		float sum = 0;
		for (int i = 0; i < distributionGraph.Length; i++)
			sum += distributionGraph[i];
		float mean = sum / distributionGraph.Length;
		
		// Sum of the differenes from the mean squared
		sum = 0;
		for (int i = 0; i < distributionGraph.Length; i++)
			sum += Mathf.Pow(distributionGraph[i] - mean, 2);
		
		// Square root of the mean of the above
		float standardDeviation = Mathf.Sqrt(sum / distributionGraph.Length);
		int asd = 0;*/
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
	
	void rateEnemies() {
		
		
	}
}


public class SpawnQueueEntry {
	
	public GameObject enemy;
	public float time;
}