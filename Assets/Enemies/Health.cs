using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float maxHealth;
	
	float health;
	
	void Start() {
		
		health = maxHealth;
	}
	
	public bool damage(float damage) {
		
		health -= damage;
		
		if (health <= 0) {
			
			kill();
			return true;
		}
		
		return false;
	}
			
	public void kill() {
		
		Destroy(gameObject);
		// Spawn scrap
		// Alert Level and Targetting Systems
	}
}