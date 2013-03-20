using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public AudioClip clip;
	
	Vector3 direction;
	float speed;
	float time;
	float range = 2000f;
	bool penetration = false;
	
	float damage;
	
	Vector3 previousPosition;
	
	TowerShooting tower;
	
	public void Update() {
		
		transform.position += direction * speed * Time.deltaTime;
		
		time += Time.deltaTime;
		if (time * speed > range) {
			
			Destroy(gameObject);
		}
		
		// Rework collision system to use geometry instead.
		Ray ray = new Ray(previousPosition, transform.position - previousPosition);
		RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, previousPosition));
		foreach (RaycastHit hit in hits) {
			
			if (hit.collider.tag == "Enemy") {
				
				if (hit.collider.GetComponent<Health>().damage(damage))
					tower.reportDead();
				if (clip != null) GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().play(clip);
				Destroy(gameObject);
				if (!penetration) return;
			}
		}
		
		/*// Get line from previous position to current
		Vector3 line = transform.position - previousPosition;
		
		// For each enemy
		foreach (GameObject enemy in GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().avaliableEnemies) {
			
			// Test if line intersects renderer boundaries
			
		}*/
		
		// Start recording hits
		
		previousPosition = transform.position;
	}
	
	public void initialise(TowerShooting tower, Vector3 target, float speed, float damage, bool penetration) {
		
		this.tower = tower;
		direction = Vector3.Normalize(target - transform.position);
		this.speed = speed;
		this.damage = damage;
		this.penetration = penetration;
	}
}
