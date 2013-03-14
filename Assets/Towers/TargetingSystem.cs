using UnityEngine;
using System.Collections;

public class TargetingSystem : MonoBehaviour {

	void Update() {
		
		// Check for tower's without targets
		// Assign them a target based on a weighted random choice
		
		// For any tower's with targets
		// Check for any new targets in range
		// If new target has a higher priority then fire at it
	}
	
	public void updatePriorityList() {
		
		// Called when new enemies are spawned
		// Rates each enemy with a priority
		// Priorities are used to wieght down the random choice in target selection
	}
}
