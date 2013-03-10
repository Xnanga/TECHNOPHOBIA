using UnityEngine;
using System.Collections;

public class SpawnSystem : MonoBehaviour {

	public GameObject[] spawnPoints;
	public float selectedDifficulty;
	public float playerPerformance;
	
	void Update() {
		
		
	}
}


public class SpawnQueueEntry {
	
	public GameObject enemy;
	public float time;
}