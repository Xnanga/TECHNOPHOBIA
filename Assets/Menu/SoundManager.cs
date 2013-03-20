using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	void Awake() {
		
		DontDestroyOnLoad(gameObject);
	}
	
	public void play(AudioClip clip) {
		
		StartCoroutine(player(clip));
	}
	
	IEnumerator player(AudioClip clip) {
		
		audio.PlayOneShot(clip);
		yield return new WaitForSeconds(clip.length);
	}
}
