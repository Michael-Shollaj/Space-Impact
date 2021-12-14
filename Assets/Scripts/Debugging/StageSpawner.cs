using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SpaceImpact.Utility;

namespace SpaceImpact {

	public class StageSpawner : MonoBehaviour {
		// Public Variables	
		[SerializeField] private SICWaypoint waypoint;
		[SerializeField] private int enemyCount = 5;
		[SerializeField] private float interval = 0.1f;

		// Private Variables
		private List<GameObject> enemies;

		// Static Variables

		public void Start() {
			enemies = new List<GameObject>();
			InvokeRepeating("SpawnEnemy", 1, interval);
		}

		public void Update() {
			if (enemies == null || enemies.Count <= 0)
				return;

			for (int i = 0; i < enemies.Count; i++) {
				if (!enemies[i].activeInHierarchy) {
					enemies.Remove(enemies[i]);
				}
			}
		}

		public void SpawnEnemy() {
			if (enemies.Count >= enemyCount)
				return;

			GameObject enemyObj = SICObjectPoolManager.SharedInstance.GetObject(SICObjectPoolName.OBJECT_ENEMY);
			if (enemyObj != null) {
				SICGameEnemy enemy = enemyObj.GetComponent<SICGameEnemy>();
				enemy.EnableElement();

				enemyObj.transform.position = new Vector3(
					UnityEngine.Random.Range(waypoint.LowerLeft.x + (enemy.MainTexture.bounds.size.x / 2) + 5f, 
											waypoint.UpperRight.x - (enemy.MainTexture.bounds.size.x / 2)),
					UnityEngine.Random.Range(waypoint.LowerLeft.y + (enemy.MainTexture.bounds.size.x / 2),
											waypoint.UpperRight.y - (enemy.MainTexture.bounds.size.x / 2)),
					0f
				);
				enemies.Add(enemyObj);
			}
		}
	}
}