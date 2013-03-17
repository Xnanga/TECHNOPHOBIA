using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	Vector3 direction;
	float speed;
	float time;
	float range = 150f;
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
				Destroy(gameObject);
				if (!penetration) return;
			}
		}
		
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
