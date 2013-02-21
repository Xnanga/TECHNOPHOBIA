using UnityEngine;
using System.Collections;

public class EnemyList : MonoBehaviour {

	public Enemy[] enemies;
}

[System.Serializable]
public class Enemy {
	
	public string name;
	public GameObject prefab;
}