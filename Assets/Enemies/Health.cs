using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float maxHealth;
	public float health;
	public int scrap;
	public int score;
	
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
		GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().reportDead(gameObject);
		GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().scrap += scrap;
		GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().score += score;
		// Spawn scrap
		// Alert Level and Targetting Systems
	}
}
