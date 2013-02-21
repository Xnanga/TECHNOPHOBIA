using UnityEngine;
using System.Collections;

public class TowerPlacement : MonoBehaviour {
	
	public Tower selectedTower;
	
	Tower[] towerList;
	
	void Start() {
		
		GameObject control = GameObject.FindGameObjectWithTag("GameController");
		if (control != null) {
			
			TowerList towerList = control.GetComponent<TowerList>();
			if (towerList != null)
				this.towerList = towerList.towers;
		}
		
		selectedTower = this.towerList[0];
	}
	
	void Update() {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit) && hit.collider.tag == "TowerArea") {
			
			if (hit.transform.GetComponent<TowerArea>().currentTower == null) {
				
				// Check for tower selection and place tower prefab in this area
				
				hit.transform.GetComponent<TowerArea>().currentTower = (GameObject) Instantiate(selectedTower.prefab, hit.transform.position, selectedTower.prefab.transform.rotation);
			}
			else {
				
				// Bring up sell/upgrade menu, which is hidden when it loses focus (player clicks somewhere else)
			}
		}
	}
}
