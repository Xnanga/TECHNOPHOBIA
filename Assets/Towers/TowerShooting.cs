using UnityEngine;
using System.Collections;

public class TowerShooting : MonoBehaviour {
	
	public float range;
	public GameObject target;
	public GameObject projectile;
	public float projectileSpeed;
	public float cooldown;
	public float tolerance = 0.005f;
	public float timeStep = 0.001f;
	
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
	}
}
