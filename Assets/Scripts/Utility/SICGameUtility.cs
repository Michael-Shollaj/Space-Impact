using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SpaceImpact {

	public class SICGameUtility {
		public static List<GameObject> GetAllVisibleEnemies() {
			List<GameObject> result = new List<GameObject>();

			foreach (SICObjectPoolManager.ObjectPooled pooled in SICObjectPoolManager.SharedInstance.GetParents(SICObjectPoolName.OBJECT_ENEMY)) {
				result.AddRange(pooled.objectList.FindAll(a => (a.GetComponent<SICGameElement>()).IsElementVisible() && a.activeInHierarchy));
			}

			return result;
		}

		public static Transform GetRandomEnemy() {
			Transform result = null;

			if (GetAllVisibleEnemies().Count <= 0)
				return null;

			int maxCount = GetAllVisibleEnemies().Count;
			float[] weights = new float[maxCount];
			float curWeight = 0f;

			for (int i = 0; i < maxCount; i++) {
				curWeight += (1f / maxCount);
				weights[i] = curWeight;
			}

			float rand = UnityEngine.Random.Range(0f, 100f) / 100f;

			int indx = 0;
			for (int i = 0; i < weights.Length; i++) {
				if (rand < weights[i]) {
					indx = i;
					break;
				}
			}

			result = GetAllVisibleEnemies()[indx].transform;
			return result;
		}

		public static Transform GetNearestEnemy(Vector3 from) {
			Transform result = null;

			if (GetAllVisibleEnemies().Count <= 0)
				return null;

			float lowestDistance = 999999f;
			for (int i = 0; i < GetAllVisibleEnemies().Count; i++) {
				GameObject obj = GetAllVisibleEnemies()[i];
				float distance = Vector3.Distance(from, obj.transform.position);

				if (distance > lowestDistance) {
					continue;
				}

				result = obj.transform;
				lowestDistance = distance;
			}

			return result;
		}

		// TODO: FIX Finding proper target
		public static Transform GetNearestUntargettedEnemy(Vector3 from) {
			Transform result = null;

			if (GetAllVisibleEnemies().Count <= 0)
				return null;

			if (GetNearestEnemies(from).Count <= 0)
				return null;

			for (int i = 0; i < GetNearestEnemies(from).Count; i++) {
				GameObject enemyObj = GetNearestEnemies(from)[i];
				SICGameEnemy enemy = enemyObj.GetComponent<SICGameEnemy>();
				if (enemy == null)
					continue;

				if (enemy.IsTargetted)
					continue;

				result = enemyObj.transform;
				break;
			}

			return result;
		}

		public static List<GameObject> GetNearestEnemies(Vector3 from) {
			List<GameObject> result = new List<GameObject>();

			if (GetAllVisibleEnemies().Count <= 0)
				return null;

			result.AddRange(GetAllVisibleEnemies());
			result.Sort(delegate(GameObject a, GameObject b) {
				float distA = Vector3.Distance(from, a.transform.position);
				float distB = Vector3.Distance(from, b.transform.position);
				return distA.CompareTo(distB);
			});

			return result;
		}

		public static List<GameObject> GetAllProjectilesOfType(ProjectileType type) {
			return SICObjectPoolManager.SharedInstance.GetParent(SICObjectPoolName.OBJECT_PROJECTILE, type).objectList.FindAll(a => a.activeInHierarchy);
		}

		public static void SetAllElementsRecursively(Transform root, bool enable) {
			foreach (Transform obj in root) {
				SICGameElement element = obj.GetComponent<SICGameElement>();
				if (element != null) {
					if (enable)
						element.EnableElement();
					else
						element.DisableElement();
				}

				SetAllElementsRecursively(obj, enable);
			}
		}

		public static string GetSpecialIcon(ProjectileType type) {
			string result = string.Empty;

			if (type == ProjectileType.ROCKET) {
				result = "♣";
			}
			else if (type == ProjectileType.LASER) {
				result = "└";
			}
			else if (type == ProjectileType.BEAM) {
				result = "┬";
			}

			return result;
		}

		public static string GetLivesIcon(int lives) {
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < lives; i++) {
				result.Append("♥");
			}
			return result.ToString();
		}

		public static void GetElementsRecursively(Transform root, ref List<SICGameElement> elements) {
			foreach (Transform obj in root) {
				SICGameElement element = obj.GetComponent<SICGameElement>();
				if (element != null) {
					elements.Add(element);
				}

				GetElementsRecursively(obj, ref elements);
			}
		}

		public static void GetEnemyRecursively(Transform root, ref List<SICGameEnemy> elements) {
			foreach (Transform obj in root) {
				SICGameEnemy element = obj.GetComponent<SICGameEnemy>();
				if (element != null) {
					elements.Add(element);
				}

				GetEnemyRecursively(obj, ref elements);
			}
		}

		public static void GetUnitRecursively(Transform root, ref List<SICGameUnit> elements) {
			foreach (Transform obj in root) {
				SICGameUnit unit = obj.GetComponent<SICGameUnit>();
				if (unit != null) {
					elements.Add(unit);
				}

				GetUnitRecursively(obj, ref elements);
			}
		}
	}
}