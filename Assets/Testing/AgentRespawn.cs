using UnityEngine;
using System.Collections;

public class AgentRespawn : MonoBehaviour {

	public GameObject agent;
	public GameObject endPoint;
	
	Vector3 startingPosition;
	
	void Start() {
		
		if (agent != null && agent.transform != null) 
			startingPosition = agent.transform.position;
	}
	
	void Update() {
		
		if (agent != null && agent.transform != null && endPoint != null && endPoint.transform != null) {
			
			if (agent.transform.position == endPoint.transform.position) {
				
				Agent script = agent.GetComponent<Agent>();
				if (script != null) {
					
					agent.transform.position = startingPosition;
					script.respawn();
				}
			}
		}
	}
}
