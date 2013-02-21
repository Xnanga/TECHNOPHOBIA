using UnityEngine;
using System.Collections;

public class TowerShooting : MonoBehaviour {

	public Enemy target;
	public float projectileSpeed;
	public float tolerance;
	public float timeStep;
	
	void Start() {
		
		GameObject control = GameObject.FindGameObjectWithTag("GameController");
		if (control != null) {
			
			EnemyList list = control.GetComponent<EnemyList>();
			if (list != null)
				target = list.enemies[0];
		}
		
		//transform.LookAt(target.prefab.transform);
		/*transform.eulerAngles.x = 0;
		transform.eulerAngles.y = 0;*/
	}
	
	void Update() {
		
		if (projectileSpeed > 0 && tolerance >= timeStep && target != null) {
			
			Vector3 p = gameObject.transform.position;
			Vector3 a = target.prefab.transform.position;
			
			float dpa = Vector3.Distance(p, a);
			
			float t = dpa / projectileSpeed;
			
		repeat:
			// Test for hit
			if (!target.prefab.GetComponent<Agent>().canHit(t)) goto end;
			Vector3 b = target.prefab.GetComponent<Agent>().positionAt(t);
			
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
			
			/*Vector2 npb = Vector3.Normalize(b - p);
			Vector2 c = transform.forward;
			
			float angle = Mathf.Acos(Vector2.Dot(npb, c));
			
			Vector3 rotation = new Vector3(0, angle, 0);
			
			transform.Rotate(Vector3.up, angle);*/
			
			/*Vector3 ndpb = Vector3.Normalize(b - p);
			transform.rotation = Quaternion.LookRotation(npb);*/
			
			/*Vector3 relativeUp = new Vector3(0, 0, -1);
			Vector3 relativePos = b - transform.position;
			transform.rotation = Quaternion.LookRotation(relativePos,relativeUp);*/
			
			//transform.LookAt(b);
			
			// Shift b into the same plane as p
			b.z = p.z;
			
			// Rotate to look at b
			transform.rotation = Quaternion.LookRotation(b - p, transform.up);
			
			// Fire projectile
			
		end:
			int i = 0;
		}
	}
}
