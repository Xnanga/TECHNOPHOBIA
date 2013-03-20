using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerShooting : MonoBehaviour {
	
	public float damage;
	public bool penetration;
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
			//bool intersection = false;
			for (int point = 0; point < path[i].Length - 1; point++) {
				for (int otherPoint = point + 1; otherPoint < path[i].Length; otherPoint++) {
					
					/*Vector3 line = path[i][otherPoint] - path[i][point];
					Vector3 lineNormal = line.normalized;
					Vector3 pointToCircle = transform.position - path[i][point];
					float closestPointMagnitude = Vector3.Dot(pointToCircle, lineNormal);
					
					Vector3 closestPoint = new Vector3(0, 0, 0);
					if (closestPointMagnitude < 0)
						closestPoint = path[i][point];
					else if (closestPointMagnitude > line.magnitude)
						closestPoint = path[i][otherPoint];
					else
						closestPoint = closestPointMagnitude * lineNormal;
					
					// Distance between circle and closest point on line
					float distance = Vector3.Distance(transform.position, closestPoint);
					
					// Test if circle intersects line and begin recording search spaces
					if (distance <= maxRange * 3) {*/
						
						SearchSpace newSpace = new SearchSpace(i);
						bool inRange = false;
						// Iterate through the segment, testing each point to check if it's in range
						for (float t = 0; t <= 1; t += timeStep) {
							
							Vector3 curvePoint = bezierInterpolate(t, i);
							curvePoint.y = transform.position.y;
							float curvePointDistance = Vector3.Distance(transform.position, curvePoint);
							
							if (!inRange && curvePointDistance >= minRange && curvePointDistance <= maxRange * 3) {
								
								newSpace.start = t;
								inRange = true;
							}
							else if (inRange && (curvePointDistance < minRange || curvePointDistance > maxRange * 3)) {
								
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
						
						//intersection = true;
						//break;
					//}
				}
				
				//if (intersection) break;
			}
		}
		this.searchSpace = searchSpace.ToArray();
		GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnSystem>().updateGraph();
		
		// Temporary, until the Targetting system is complete
		//GameObject control = GameObject.FindGameObjectWithTag("GameController");
		//target = control.GetComponent<Level>().avaliableEnemies[0];
		
	}
	
	void Update() {
		
		if (target != null) {
			
			// If enemy is in range
			Vector3 point = target.transform.position;
			//point.z = transform.position.z;
			point.y = transform.position.y;
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
			centre.y = 0;
			
			// For each search space
			foreach (SearchSpace searchSpace in this.searchSpace) {
				
				// Iterate over curve segment
				for (float t = searchSpace.start; t <= searchSpace.end; t += timeStep) {
					
					Vector3 point = enemy.positionAt(t);
					point.y = centre.y;
					
					// Time for projectile to reach point
					//float projectileTime = Vector3.Distance(spawn.transform.position, point) / projectileSpeed;
					float projectileTime = Vector3.Distance(centre, point) / projectileSpeed;
					
					// Enemy's position after a time lapse of projectileTime
					if (!enemy.realTimeBoundsTest(projectileTime)) break;
					Vector3 position = enemy.timeLapse(projectileTime);
					position.y = centre.y;
					
					// Enemy's position is close to point on curve
					if (Vector3.Distance(position, point) <= tolerance) {
						
						// Rotate to face point
						//point.y = centre.y;
						transform.rotation = Quaternion.LookRotation(point - centre, transform.up);
						
						// Fire if possible
						if (this.time <= 0) {
						
							//GameObject shot = (GameObject) Instantiate(projectile, spawn.transform.position, projectile.transform.rotation);
							GameObject shot = (GameObject) Instantiate(projectile, spawn.transform.position, Quaternion.LookRotation(point - centre, transform.up));
							point.y = spawn.transform.position.y;
							shot.GetComponent<Projectile>().initialise(this, point, projectileSpeed, damage, penetration);
							this.time = cooldown;
						}
						else {
							
							this.time -= Time.deltaTime;
						}
						return;
					}
				}
			}
			
			
			Vector3 pos = target.transform.position;
			//pos.z = centre.z;
			pos.y = centre.y;
			transform.rotation = Quaternion.LookRotation(pos - centre, transform.up);
		}
	}
	
	public void reportDead() {
		
		target = null;
	}
	
	private Vector3 bezierInterpolate(float time, int offset) {
		
		Vector3[][] path = GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().path;
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
	
	private float getBinCoeff(float N, float K)
	{
	   // This function gets the total number of unique combinations based upon N and K.
	   // N is the total number of items.
	   // K is the size of the group.
	   // Total number of unique combinations = N! / ( K! (N - K)! ).
	   // This function is less efficient, but is more likely to not overflow when N and K are large.
	   // Taken from:  http://blog.plover.com/math/choose.html
	   //
	   float r = 1;
	   float d;
	   if (K > N) return 0;
	   for (d = 1; d <= K; d++)
	   {
	      r *= N--;
	      r /= d;
	   }
	   return r;
	}
	
	public List<GameObject> getEnemiesInRange() {
		
		List<GameObject> returnList = new List<GameObject>();
		
		Collider[] inMaxRange = Physics.OverlapSphere(transform.position, maxRange);
		Collider[] inMinRange = Physics.OverlapSphere(transform.position, minRange);
		
		foreach (Collider collider in inMaxRange) {
			
			if (collider.tag == "Enemy") {
				
				bool insideMinRange = false;
				foreach (Collider otherCollider in inMinRange)
					if (otherCollider == collider)
						insideMinRange = true;
				
				if (!insideMinRange) returnList.Add(collider.gameObject);
			}
		}
		
		return returnList;
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
