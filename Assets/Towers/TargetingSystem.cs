using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetingSystem : MonoBehaviour {
	
	Dictionary<GameObject, float> priorityList = new Dictionary<GameObject, float>();
	Dictionary<GameObject, List<GameObject>> possibleTargets = new Dictionary<GameObject, List<GameObject>>();
	
	void Update() {
		
		updatePossibleTargetList();
		updatePriorityList();
		
		foreach (GameObject tower in GetComponent<Level>().currentTowers) {
			
			TowerShooting towerScript = tower.GetComponent<TowerShooting>();
			if (towerScript != null) {
				
				List<GameObject> enemies = new List<GameObject>();
				
				if (towerScript.target != null) {
					
					// Check for /new/ enemies with higher priority
					foreach (GameObject enemy in towerScript.getEnemiesInRange())
						if (!possibleTargets[tower].Contains(enemy))
							enemies.Add(enemy);
				}
				else
					enemies = possibleTargets[tower];
				
				float prioritySum = 0;
				foreach (GameObject enemy in enemies)
					prioritySum += priorityList[enemy];
				
				float rnd = Random.Range(0, prioritySum);
				
				float cumulativeSum = 0;
				foreach (GameObject enemy in enemies) {
					
					cumulativeSum += priorityList[enemy];
					if (rnd < cumulativeSum) {
						
						if (towerScript.target != null) priorityList[towerScript.target] += 10;
						towerScript.target = enemy;
						priorityList[enemy] -= 10;
						return;
					}
				}
			}
		}
	}
	
	private void updatePossibleTargetList() {
		
		possibleTargets.Clear();
		
		foreach (GameObject tower in GetComponent<Level>().currentTowers) {
			
			TowerShooting towerScript = tower.GetComponent<TowerShooting>();
			if (towerScript != null) {
				
				possibleTargets.Add(tower, towerScript.getEnemiesInRange());
			}
		}
	}
	
	private void updatePriorityList() {
		
		priorityList.Clear();
		
		foreach (GameObject enemy in GetComponent<Level>().currentEnemies) {
			
			float health = enemy.GetComponent<Health>().health;
			float speed = enemy.GetComponent<Agent>().speed;
			
			int segment = enemy.GetComponent<Agent>().currentPoint;
			float t = enemy.GetComponent<Agent>().time;
			Vector3[][] path = GetComponent<Level>().path;
			float distanceFromEnd = (path.Length - segment - 1) + (1 - t);
			
			float timeStep = GetComponent<SpawnSystem>().timeStep;
			int towerCentre = GetComponent<SpawnSystem>().towerCentre;
			float fdistance = (segment * (1 / timeStep)) + (t / timeStep);
			int distance = (int) fdistance;
			int distanceFromCentre = distance - towerCentre;
			
			float priority = (distanceFromEnd * 100) + (health * 30) + (speed * 30) + (distanceFromCentre * 20);
			
			priorityList.Add(enemy, priority);
		}
	}
}
