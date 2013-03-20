using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	void Awake() {
		
		DontDestroyOnLoad(gameObject);
	}
	
	void Start() {
		
		Application.LoadLevel("Main Menu");
	}
}
