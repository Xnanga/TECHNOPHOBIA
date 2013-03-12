using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	Vector3 direction;
	float speed;
	float time;
	float range = 150f;
	
	float damage;
	
	Vector3 previousPosition;
	
	TowerShooting tower;
	
	public void Update() {
		
		transform.position += direction * speed * Time.deltaTime;
		
		time += Time.deltaTime;
		if (time * speed > range) {
			
			Destroy(gameObject);
		}
		
		Ray ray = new Ray(previousPosition, transform.position - previousPosition);
		RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(transform.position, previousPosition));
		foreach (RaycastHit hit in hits) {
			
			if (hit.collider.tag == "Enemy") {
				
				if (hit.collider.GetComponent<Health>().damage(damage))
					tower.reportDead();
				Destroy(gameObject);
			}
		}
		
		previousPosition = transform.position;
	}
	
	public void initialise(TowerShooting tower, Vector3 target, float speed, float damage) {
		
		this.tower = tower;
		direction = Vector3.Normalize(target - transform.position);
		this.speed = speed;
		this.damage = damage;
	}
}
