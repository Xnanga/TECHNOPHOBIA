using UnityEngine;
using System.Collections;

public class TowerList : MonoBehaviour {

	public Tower[] towers;
}

[System.Serializable]
public class Tower {
	
	public string name;
	public GameObject prefab;
}
