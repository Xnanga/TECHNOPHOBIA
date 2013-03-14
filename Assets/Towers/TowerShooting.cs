using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerShooting : MonoBehaviour {
	
	public float damage;
	public float minRange = 0;
	public float maxRange;
	public GameObject target;
	public GameObject projectile;
	public GameObject spawn;
	public float projectileSpeed;
	public float cooldown;
	public float tolerance = 10f;
	public float timeStep = 0.05f;
	public SearchSpace[] searchSpace;
	
	float time;
	
	void Start() {
		
		List<SearchSpace> searchSpace = new List<SearchSpace>();
		Vector3[][] path = GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().path;
		
		// For each curve segment
		for (int i = 0; i < path.Length; i++) {
			
			// Ignore curve segments with only one point (i.e. the end point)
			if (path[i].Length < 2) continue;
			
			// For each pair of points
			bool intersection = false;
			for (int point = 0; point < path[i].Length - 1; point++) {
				for (int otherPoint = point + 1; point < path[i].Length; otherPoint++) {
					
					// Test if circle intersects line
					Vector3 lineNormal = Vector3.Normalize(path[i][otherPoint] - path[i][point]);
					Vector3 pointToCircle = transform.position - path[i][point];
					Vector3 closestPoint = Vector3.Dot(pointToCircle, lineNormal) * lineNormal;
					
					// Distance between circle and closest point on line
					float distance = Vector3.Distance(transform.position, closestPoint);
					
					if (distance <= maxRange) {
						
						SearchSpace newSpace = new SearchSpace(i);
						bool inRange = false;
						// Iterate through the segment, testing each point to check if it's in range
						for (float t = 0; t <= 1; t += 0.001f) {
							
							Vector3 curvePoint = bezierInterpolate(time, i);
							float distace = Vector3.Distance(transform.position, curvePoint);
							
							if (!inRange && distace >= minRange && distace <= maxRange) {
								
								newSpace.start = t;
								inRange = true;
							}
							else if (inRange && (distace < minRange || distace > maxRange)) {
								
								newSpace.end = t;
								searchSpace.Add(newSpace);
								newSpace = new SearchSpace(i);
								inRange = false;
							}
						}
						
						if (inRange) {
							
							newSpace.end = 1;
							searchSpace.Add(newSpace);
						}
						
						break;
					}
				}
				
				if (intersection) break;
			}
		}
		this.searchSpace = searchSpace.ToArray();
		
		// Temporary, until the Targetting system is complete
		GameObject control = GameObject.FindGameObjectWithTag("GameController");
		target = control.GetComponent<Level>().avaliableEnemies[0];
		
	}
	
	void Update() {
		
		if (target != null) {
			
			// If enemy is in range
			Vector3 point = target.transform.position;
			point.z = transform.position.z;
			if (Vector3.Distance(point, transform.position) >= minRange &&
				Vector3.Distance(point, transform.position) <= maxRange) {
				
				fire();
			}
		}
	}
	
	void fire() {
		
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
			}
		}
	}
	
	public void reportDead() {
		
		target = null;
	}
	
	private Vector3 bezierInterpolate(float time, int offset) {
		
		List<Vector3> points = new List<Vector3>(path[offset]);
		if (path.Length - offset > 1) points.Add(path[offset + 1][0]);
		
		if (time > 1) time = 1;
		else if (time < 0) time = 0;
		
		float x = 0, y = 0, z = 0;
		int n = points.Count - 1, i = 0;
		
		foreach (Vector3 point in points) {
			
			x += getBinCoeff(n, i) * Mathf.Pow(1 - time, n - i) * Mathf.Pow(time, i) * point.x;
			y += getBinCoeff(n, i) * Mathf.Pow(1 - time, n - i) * Mathf.Pow(time, i) * point.y;
			z += getBinCoeff(n, i) * Mathf.Pow(1 - time, n - i) * Mathf.Pow(time, i) * point.z;
			i++;
		}
		
		return new Vector3(x, y, z);
	}
}

public class SearchSpace {
	
	public int segment;
	public float start;
	public float end;
	
	public SearchSpace(int segment) {
		
		this.segment = segment;
	}
}
