using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour {

	public Waypoint[] waypoints;
	public Vector3[][] path;
	public float tolerance;
	
	void Awake() {
		
		List<List<Vector3>> path = new List<List<Vector3>>();
		
		if (waypoints.Length > 0) {
			
			foreach (Waypoint waypoint in waypoints) {
				
				List<Vector3> points = new List<Vector3>();
				points.Add(waypoint.points[0].transform.position);
				
				if (waypoint.points.Length > 1 && path.Count > 0 && waypoint.enforceC1Continuity) {
					
					Vector3 previousPoint = path[path.Count - 1][path[path.Count - 1].Count - 1];
					
					float totalDistance = Vector3.Distance(points[0], waypoint.points[1].transform.position);
					Vector3 extendedPoint = previousPoint + (points[0] - previousPoint) * totalDistance;
					
					Vector3 lineSegment = extendedPoint - points[0];
					Vector3 lineNormal = Vector3.Normalize(lineSegment);
					
					float mappedDistance = Vector3.Dot(waypoint.points[1].transform.position - points[0], lineNormal);
					Vector3 mappedPoint = lineNormal * mappedDistance;
					
					Vector3 truePoint = points[0] + mappedPoint;
					float distance = Vector3.Distance(truePoint, waypoint.points[1].transform.position);
					
					if (tolerance == -1 || distance <= tolerance) {
						
						points.Add(truePoint);
					}
					else {
						
						points.Add(waypoint.points[1].transform.position);
					}
				}
				
				if (waypoint.points.Length > 1) {
					
					for (int i = 2; i < waypoint.points.Length; i++) {
						
						points.Add(waypoint.points[i].transform.position);
					}
				}
				
				path.Add(points);
			}
		}
		
		this.path = new Vector3[path.Count][];
		for (int i = 0; i < path.Count; i++) {
			
			this.path[i] = path[i].ToArray();
		}
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().registerPaths();
	}
}

[System.Serializable]
public class Waypoint
{
	// anchor point and control points
	public GameObject[] points;
	public bool enforceC1Continuity;
}