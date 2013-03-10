using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour {
	
	public List<SpawnQueueEntry> spawnQueue = new List<SpawnQueueEntry>();
	
	void Update() {
		
		foreach (SpawnQueueEntry entry in spawnQueue) {
			
			entry.time -= Time.deltaTime;
			if (entry.time <= 0) {
				
				GameObject enemy = (GameObject) Instantiate(entry.enemy, gameObject.transform.position, entry.enemy.transform.rotation);
				GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().currentEnemies.Add(enemy);
			}
		}
	}
}
