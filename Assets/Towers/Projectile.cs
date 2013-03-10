using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	Vector3 direction;
	float speed;
	float time;
	float range = 150f;
	
	public void Update() {
		
		transform.position += direction * speed * Time.deltaTime;
		
		time += Time.deltaTime;
		if (time * speed > range) {
			
			Destroy(gameObject);
		}
	}
	
	public void initialise(Vector3 target, float speed) {
		
		direction = Vector3.Normalize(target - transform.position);
		this.speed = speed;
	}
}
