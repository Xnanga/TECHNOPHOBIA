using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public GameObject pathContainer;
	public float speed = 3;
	
	bool ready = false;
	List<List<Vector3>> path;
	List<Vector3> currentPoint;
	float distance;
	float time;
	
	void Start() {
		
		if (pathContainer != null) {
			
			Path pathScript = pathContainer.GetComponent<Path>();
			if (pathScript != null) {
				
				pathScript.buildPath();
				if (pathScript.path != null) {
					
					path = pathScript.path;
					currentPoint = path[0];
					path.Remove(currentPoint);
					time = 0;
					if (path.Count > 0) distance = approximateDistance();
					ready = true;
				}
			}
		}
	}
	
	void Update() {
		
		if (ready && path.Count > 0) {
			
			time += (speed * Time.deltaTime) / distance;
			transform.position = bezierInterpolate();
			//transform.LookAt(bezierInterpolate(time + 0.001f));
			// Force sprite to look at camera.
			
			if (transform.position == path[0][0]) {
				
				currentPoint = path[0];
				path.Remove(currentPoint);
				time = 0;
				if (path.Count > 0) distance = approximateDistance();
			}
		}
	}
	
	private Vector3 bezierInterpolate() {
		
		return bezierInterpolate(this.time, 0);
	}
	
	private Vector3 bezierInterpolate(float time) {
		
		return bezierInterpolate(time, 0);
	}
	
	private Vector3 bezierInterpolate(float time, int offset) {
		
		List<Vector3> points = new List<Vector3>();
		if (offset == 0) {
			
			points = new List<Vector3>(currentPoint);
			if (path.Count >= 0) points.Add(path[0][0]);
		}
		else {
			
			points = new List<Vector3>(path[offset - 1]);
			if (path.Count >= offset) points.Add(path[offset][0]);
		}
		
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
	
	private float approximateDistance() {
		
		List<Vector3> points = new List<Vector3>(currentPoint);
		points.Add(path[0][0]);
		
		float distance = 0;
		for (float t = 0; t <= 1; t += 0.001f) {
			
			Vector3 a = bezierInterpolate(t);
			Vector3 b = bezierInterpolate(t + 0.001f);
			distance += Vector3.Distance(a, b);
		}
		
		return distance;
	}
	
	public void respawn() {
		
		Path pathScript = pathContainer.GetComponent<Path>();
			if (pathScript != null) {
				
				pathScript.buildPath();
				if (pathScript.path != null) {
					
					path = pathScript.path;
					currentPoint = path[0];
					path.Remove(currentPoint);
					time = 0;
					if (path.Count > 0) distance = approximateDistance();
					ready = true;
				}
			}
	}
	
	/*public bool canHit(float time) {
		
		time += this.time;
		int segmentsCrossedOver = 0;
		while (time > 1) {
			
			time -= 1;
			segmentsCrossedOver++;
		}
		
		if (segmentsCrossedOver >= path.Count) return false;
		else return true;
	}*/
	
	public Vector3 positionAt(float time) {
		
		time += this.time;
		int segmentsCrossedOver = 0;
		while (time > 1) {
			
			time = time - 1;
			segmentsCrossedOver++;
		}
		
		return bezierInterpolate(time, segmentsCrossedOver);
	}
	
	public bool pathBoundsTest(float time) {
		
		time += this.time;
		int segmentsCrossedOver = 0;
		while (time > 1) {
			
			time = time - 1;
			segmentsCrossedOver++;
		}
		
		if (segmentsCrossedOver >= path.Count) return false;
		else return true;
	}
}
