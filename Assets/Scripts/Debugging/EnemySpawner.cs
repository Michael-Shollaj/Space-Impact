using UnityEngine;
using System.Collections;
using SpaceImpact;
using System.Collections.Generic;
using SpaceImpact.Utility;

public class EnemySpawner : MonoBehaviour {	
	// Public Variables	
	[SerializeField] private int enemyCount = 5;
	[SerializeField] private float interval = 0.5f;

	// Private Variables	
	private List<GameObject> enemies;

	// Static Variables

	private void Start() {
		enemies = new List<GameObject>();
		InvokeRepeating("SpawnEnemy", 1, interval);
	}

	private void Update() {
		if (enemies == null || enemies.Count <= 0)
			return;

		for (int i = 0; i < enemies.Count; i++) {
			if (!enemies[i].activeInHierarchy) {
				enemies.Remove(enemies[i]);
			}
		}
	}

	private void SpawnEnemy() {
		if (enemies.Count >= enemyCount)
			return;

		GameObject enemyObj = SICObjectPoolManager.SharedInstance.GetObject(SICObjectPoolName.OBJECT_ENEMY);
		if (enemyObj != null) {
			SICGameEnemy enemy = enemyObj.GetComponent<SICGameEnemy>();
			enemy.EnableElement();

			enemyObj.transform.position = new Vector3(
				UnityEngine.Random.Range(SICAreaBounds.MinPosition.x + (enemy.MainTexture.bounds.size.x / 2) + 5f, SICAreaBounds.MaxPosition.x - (enemy.MainTexture.bounds.size.x / 2)),
				UnityEngine.Random.Range(SICAreaBounds.MinPosition.y + (enemy.MainTexture.bounds.size.x / 2), SICAreaBounds.MaxPosition.y - (enemy.MainTexture.bounds.size.x / 2)),
				0f
			);
			enemies.Add(enemyObj);
		}
	}
}