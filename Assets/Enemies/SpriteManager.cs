using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {

	public Material[] sprites;
	public float frameTime;
	
	float time;
	int currentSprite;
	
	void Update() {
		
		time += Time.deltaTime;
		if (time >= frameTime) {
			
			currentSprite++;
			if (currentSprite >= sprites.Length) currentSprite = 0;
			gameObject.renderer.material = sprites[currentSprite];
			time = 0;
		}
	}
}
