using UnityEngine;
using System.Collections;

public class TowerShooting : MonoBehaviour {
	
	public float damage;
	public float minRange = 0;
	public float maxRange;
	public float rangeStep = 0.01f;
	public GameObject target;
	public GameObject projectile;
	public GameObject spawn;
	public float projectileSpeed;
	public float cooldown;
	public float tolerance = 10f;
	public float timeStep = 0.05f;
	
	float time;
	
	void Start() {
		
		GameObject control = GameObject.FindGameObjectWithTag("GameController");
		if (control != null) {
			
			Level list = control.GetComponent<Level>();
			if (list != null)
				target = list.avaliableEnemies[0];
		}
		
		//transform.LookAt(target.prefab.transform);
		/*transform.eulerAngles.x = 0;
		transform.eulerAngles.y = 0;*/
	}
	
	void Update() {
		
		if (target != null) {
			
			// If enemy is in range
			Vector3 point = target.transform.position;
			point.z = transform.position.z;
			if (Vector3.Distance(point, transform.position) >= minRange &&
				Vector3.Distance(point, transform.position) <= maxRange) {
				
				//basicAiming();
				advanceAiming();
			}
		}
	}
	
	void basicAiming() { // Can't be trusted at all
		
		if (spawn != null && target != null && projectileSpeed > 0) {
			
			Vector3 point = this.target.transform.position;
			point.z = transform.position.z;
			
			if (Vector3.Distance(point, transform.position) > maxRange ||
				Vector3.Distance(point, transform.position) < minRange)
				return;
			
			transform.rotation = Quaternion.LookRotation(point - transform.position, transform.up);
			
			if (this.time <= 0) {
			
				GameObject shot = (GameObject) Instantiate(projectile, spawn.transform.position, projectile.transform.rotation);
				shot.GetComponent<Projectile>().initialise(this, target.transform.position, projectileSpeed, damage);
				this.time = cooldown;
			}
			else {
				
				this.time -= Time.deltaTime;
			}
		}
	}
	
	void advanceAiming() { // Can't be trusted with small numbers, and is generally very temperamental
		
		if (spawn != null && target != null && projectileSpeed > 0 && timeStep > 0 && tolerance >= timeStep) {
			
			Agent enemy = this.target.GetComponent<Agent>();
			if (enemy == null) return;
			
			// Circle's centre
			Vector3 centre = transform.position;
			centre.z = 0;
			
			// For each point along path
			for (float time = 0; true; time += timeStep) {
				
				if (!enemy.pathBoundsTest(time)) break;
				Vector3 point = enemy.positionAt(time);
				
				// Check if point is within max range and outwith min range
				/*if (Vector2.Distance(centre, point) <= maxRange &&
					Vector2.Distance(centre, point) >= minRange) {*/ // Now doing this at start of loop, with the enemy's current position
					
					// Time for projectile to reach point on curve
					float projectileTime = Vector3.Distance(spawn.transform.position, point) / projectileSpeed;
					
					// Check if projectile can reach point in around the same time as  enemy
					/*if (projectileTime >= time - (time * tolerance) &&
						projectileTime <= time + (time * tolerance)) {*/
					
					// enemy's position after a time lapse of projectileTime
					if (!enemy.realTimeBoundsTest(projectileTime)) break;
					Vector3 position = enemy.timeLapse(projectileTime);
					
					// Enemy's position is close to point on curve
					if (Vector3.Distance(position, point) <= tolerance) {
						
						// Rotate to face point
						point.z = centre.z;
						transform.rotation = Quaternion.LookRotation(point - centre, transform.up);
						
						// Fire if possible
						if (this.time <= 0) {
						
							//GameObject shot = (GameObject) Instantiate(projectile, spawn.transform.position, projectile.transform.rotation);
							GameObject shot = (GameObject) Instantiate(projectile, spawn.transform.position, Quaternion.LookRotation(point - transform.position, transform.up));
							shot.GetComponent<Projectile>().initialise(this, point, projectileSpeed, damage);
							this.time = cooldown;
						}
						else {
							
							this.time -= Time.deltaTime;
						}
						return;
					}
				//}
			}
		}
	}
	
	public void reportDead() {
		
		target = null;
	}
	
	/*void Update() {
		
		if (projectileSpeed > 0 && tolerance >= timeStep && target != null) {
			
			Vector3 p = gameObject.transform.position;
			Vector3 a = target.transform.position;
			
			float dpa = Vector3.Distance(p, a);
			
			float t = dpa / projectileSpeed;
			
		repeat:
			if (!target.GetComponent<Agent>().canHit(t)) goto end;
			Vector3 b = target.GetComponent<Agent>().positionAt(t);
			
			float dpb = Vector3.Distance(p, b);
			
			float t2 = dpb / projectileSpeed;
			
			if (t2 > t + t * tolerance) {
				
				t += timeStep;
				goto repeat;
			}
			else if (t2 < t - t * tolerance) {
				
				t -= timeStep;
				goto repeat;
			}
			
			b.z = p.z;
			
			transform.rotation = Quaternion.LookRotation(b - p, transform.up);
			
			if (time <= 0) {
			
				GameObject shot = (GameObject) Instantiate(projectile, transform.position, projectile.transform.rotation);
				shot.GetComponent<Projectile>().initialise(b, projectileSpeed);
				time = cooldown;
			}
			else {
				
				time -= Time.deltaTime;
			}
			
		end:
			int i = 0;
		}
	}*/
}
