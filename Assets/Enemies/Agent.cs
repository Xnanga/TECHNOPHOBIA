using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public GameObject pathContainer;
	public float speed = 3;
	public int score;
	
	bool ready = false;
	Vector3[][] path;
	public int currentPoint;
	float distance;
	public float time;
	
	void Start() {
		
		if (pathContainer != null) {
			
			path = pathContainer.GetComponent<Path>().path;
			if (path != null) {
				
				currentPoint = 0;
				time = 0;
				if (path.Length - currentPoint > 1) distance = approximateDistance();
				ready = true;
			}
		}
	}
	
	void Update() {
		
		if (ready && path.Length - currentPoint > 1) {
			
			// Add in time overstep thingy here to help smoothen transition between segments
			time += (speed * Time.deltaTime) / distance;
			if (time > 1) {
				
				currentPoint++;
				float deltaTime = ((time - 1) * distance) / speed;
				if (path.Length - currentPoint > 1) distance = approximateDistance();
				time = (speed * deltaTime) / distance;
			}
			if (currentPoint == path.Length - 1) {
				
				GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().reportDead(gameObject);
				GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().score -= score;
				if (--GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().playerHealth == 0) {
					
					// game over
					Application.LoadLevel("End");
				}
				Destroy(gameObject);
				return;
			}
			
			Vector3 position = bezierInterpolate();
			//position.z = transform.position.z;
			position.y = transform.position.y;
			transform.position = position;
			//transform.LookAt(bezierInterpolate(time + 0.001f));
			// Force sprite to look at camera.
			
			if (transform.position == path[currentPoint + 1][0]) {
				
				currentPoint++;
				time = 0;
				if (currentPoint == path.Length - 1) {
					
					GameObject.FindGameObjectWithTag("GameController").GetComponent<Level>().reportDead(gameObject);
					Destroy(gameObject);
					if (--GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>().playerHealth == 0) {
						
						// game over
						Application.LoadLevel("End");
					}
					return;
				}
				if (path.Length - currentPoint > 1) distance = approximateDistance();
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
		
		List<Vector3> points = new List<Vector3>(path[currentPoint + offset]);
		if (path.Length - currentPoint - offset > 1) points.Add(path[currentPoint + offset + 1][0]);
		
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
	
	/*private float approximateDistance() {
		
		//List<Vector3> points = new List<Vector3>(currentPoint);
		//points.Add(path[0][0]);
		
		float distance = 0;
		for (float t = 0; t <= 1; t += 0.001f) {
			
			Vector3 a = bezierInterpolate(t);
			Vector3 b = bezierInterpolate(t + 0.001f);
			distance += Vector3.Distance(a, b);
		}
		
		return distance;
	}*/
	
	private float approximateDistance() {
		
		return approximateDistance(0);
	}
	
	private float approximateDistance(int offset) {
		
		float distance = 0;
		for (float t = 0; t <= 1; t += 0.001f) {
			
			Vector3 a = bezierInterpolate(t, offset);
			Vector3 b = bezierInterpolate(t + 0.001f, offset);
			distance += Vector3.Distance(a, b);
		}
		
		return distance;
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
		
		if (path.Length - currentPoint - segmentsCrossedOver > 1) return true;
		else return false;
	}
	
	public bool realTimeBoundsTest(float time) {
		
		float bTime = (speed * time) / distance;
		bTime += this.time;
		int segmentsCrossedOver = 0;
		
		while (bTime > 1) {
			
			bTime = bTime - 1;
			if (path.Length - currentPoint - segmentsCrossedOver > 1) return false;
			
			//bTime -= this.time;
			time = (bTime * distance) / speed;
			
			bTime = (speed * time) / approximateDistance(segmentsCrossedOver);
			//bTime += this.time;
		}
		
		return true;
	}
	
	public Vector3 timeLapse(float time) {
		
		// Convert real time to bezier time
		float bTime = (speed * time) / distance;
		bTime += this.time;
		int segmentsCrossedOver = 0;
		
		// If bezier time is greater than 1
		while (bTime > 1) {
			
			bTime = bTime - 1;
			segmentsCrossedOver++;
			
			// convert remaining bezier time back into real time
			//bTime -= this.time;
			time = (bTime * approximateDistance(segmentsCrossedOver - 1)) / speed;
			
			// Convert back to bezier time with the new distance
			bTime = (speed * time) / approximateDistance(segmentsCrossedOver);
			//bTime += this.time;
		}
		
		// Get enemies final position and return it
		Vector3 returnValue =  bezierInterpolate(bTime, segmentsCrossedOver);
		return returnValue;
	}
}
