using UnityEngine;
using System.Collections;

public class TowerPlacement : MonoBehaviour {
	
	public Tower currentTower;
	
	Tower[] towerList;
	
	void Start() {
		
		GameObject control = GameObject.FindGameObjectWithTag("GameController");
		if (control != null) {
			
			TowerList towerList = control.GetComponent<TowerList>();
			if (towerList != null)
				this.towerList = towerList.towers;
		}
		
		currentTower = this.towerList[0];
	}
	
	void Update() {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit) && hit.collider.tag == "TowerArea") {
			
			if (currentTower != null) {
				
				// Check for tower selection and place tower prefab in this area,
				// with the centre of the tower aligned with the base of this collider
				// (the tower's base will be aligned with the centre of it's parent GO)
				
				Instantiate(currentTower.prefab, transform.position, currentTower.prefab.transform.rotation);
			}
			else {
				
				// Bring up sell/upgrade menu, which is hidden when it loses focus (player clicks somewhere else)
			}
		}
	}
}
