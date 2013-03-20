using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {
	
	public int score;
	public int scrap;
	public int playerHealth;
	//public float performance;
	public float difficulty = 0.5f;
	
	void Start() {
		
		DontDestroyOnLoad(gameObject);
		
		// Remove previous GameData objects
		foreach (GameObject thing in GameObject.FindGameObjectsWithTag("GameData"))
			if (thing != gameObject) Destroy(thing);
		
		Application.LoadLevel("1 Bedroom");
	}
}
