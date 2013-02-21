using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour {

	public Waypoint[] waypoints;
	public List<List<Vector3>> path = new List<List<Vector3>>();
	
	public void buildPath() {
		
		if (waypoints.Length > 0) {
			
			foreach (Waypoint waypoint in waypoints) {
				
				List<Vector3> points = new List<Vector3>();
				points.Add(waypoint.points[0].transform.position);
				
				if (waypoint.points.Length > 0) {
					
					for (int i = 1; i < waypoint.points.Length; i++) {
						
						points.Add(waypoint.points[i].transform.position);
					}
				}
				
				path.Add(points);
			}
		}
		else path = null;
	}
}

[System.Serializable]
public class Waypoint
{
	// anchor point and control points
	public GameObject[] points;
}