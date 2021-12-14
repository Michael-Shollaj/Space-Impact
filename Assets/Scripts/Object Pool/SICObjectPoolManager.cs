using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceImpact {

	public class SICObjectPoolManager : MonoBehaviour {
		// Public Variables	
		[SerializeField] private SICGameElement[] refObject;

		// Private Variables
		private static SICObjectPoolManager instance;
		private List<ObjectPooled> objParents;

		// Static Variables
		public const string PARENT_NAME_FORMAT = "{0} Parent";
		public const string OBJECT_NAME_FORMAT = "{0}_{1}";

		public static SICObjectPoolManager SharedInstance { get { return instance; } }

		public List<ObjectPooled> ObjParents { get { return objParents; } }

		public class ObjectPooled {
			public Transform refObjParent;
			public SICGameElement refObj;
			public List<GameObject> objectList;

			public void AddObject(GameObject obj, bool AddToParent = true) {
				if (objectList.Contains(obj))
					return;

				if (AddToParent) {
					obj.transform.parent = refObjParent.transform;
				}

				objectList.Add(obj);
			}

			public GameObject GetObject() {
				for (int i = 0; i < objectList.Count; i++) {
					if (!objectList[i].activeInHierarchy) {
						return objectList[i];
					}
				}

				GameObject obj = (GameObject)UnityEngine.Object.Instantiate(refObj.gameObject);
				obj.name = string.Format(OBJECT_NAME_FORMAT, refObj.gameObject.name, objectList.Count);
				obj.SetActive(false);
				AddObject(obj);
				return obj;
			}
		}

		public void Awake() {
			instance = this;

			objParents = new List<ObjectPooled>();

			for (int i = 0; i < refObject.Length; i++) {
				GameObject objParent = new GameObject(string.Format(PARENT_NAME_FORMAT, refObject[i].gameObject.name));
				objParent.transform.parent = transform;

				ObjectPooled pooledObj = new ObjectPooled();
				pooledObj.refObjParent = objParent.transform;
				pooledObj.refObj = refObject[i];
				pooledObj.objectList = new List<GameObject>();
				pooledObj.GetObject();
				objParents.Add(pooledObj);
			}
		}

		public GameObject GetObject(string id) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					return objParents[i].GetObject();
				}
			}

			return null;
		}

		public GameObject GetObject(string id, StageType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGameStage stage = objParents[i].refObj.GetComponent<SICGameStage>();
					if (stage != null) {
						if (stage.GetStageType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}

		public GameObject GetObject(string id, ProjectileType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGameProjectile projectile = objParents[i].refObj.GetComponent<SICGameProjectile>();
					if (projectile != null) {
						if (projectile.GetProjectileType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}

		public GameObject GetObject(string id, BossType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGameBoss boss = objParents[i].refObj.GetComponent<SICGameBoss>();
					if (boss != null) {
						if (boss.GetBossType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}

		public GameObject GetObject(string id, ParticleType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGameParticle particle = objParents[i].refObj.GetComponent<SICGameParticle>();
					if (particle != null) {
						if (particle.GetParticleType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}

		public GameObject GetObject(string id, EnemyType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGameEnemy enemy = objParents[i].refObj.GetComponent<SICGameEnemy>();
					if (enemy != null) {
						if (enemy.GetEnemyType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}

		public GameObject GetObject(string id, UnitType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGameUnit unit = objParents[i].refObj.GetComponent<SICGameUnit>();
					if (unit != null) {
						if (unit.GetUnitType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}
		public GameObject GetObject(string id, PowerupType type) {
			for (int i = 0; i < objParents.Count; i++) {
				if (objParents[i].refObj.OBJECT_ID == id) {
					SICGamePowerup powerup = objParents[i].refObj.GetComponent<SICGamePowerup>();
					if (powerup != null) {
						if (powerup.GetPowerupType() == type) {
							return objParents[i].GetObject();
						}
					}
				}
			}

			return null;
		}


		public List<ObjectPooled> GetParents(string id) {
			return objParents.FindAll(parent => parent.refObj.OBJECT_ID == id);
		}

		public ObjectPooled GetParent(string id) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id);
		}

		public ObjectPooled GetParent(string id, StageType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGameStage).GetStageType() == type);
		}

		public ObjectPooled GetParent(string id, ProjectileType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGameProjectile).GetProjectileType() == type);
		}

		public ObjectPooled GetParent(string id, BossType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGameBoss).GetBossType() == type);
		}

		public ObjectPooled GetParent(string id, ParticleType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGameParticle).GetParticleType() == type);
		}

		public ObjectPooled GetParent(string id, EnemyType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGameEnemy).GetEnemyType() == type);
		}

		public ObjectPooled GetParent(string id, UnitType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGameUnit).GetUnitType() == type);
		}

		public ObjectPooled GetParent(string id, PowerupType type) {
			return objParents.Find(parent => parent.refObj.OBJECT_ID == id && (parent.refObj as SICGamePowerup).GetPowerupType() == type);
		}

		public void ResetAllParents() {
			for (int i = 0; i < objParents.Count; i++) {
				for (int j = 0; j < objParents[i].objectList.Count; j++) {
					SICGameElement element = objParents[i].objectList[j].GetComponent<SICGameElement>();
					if (element != null) {
						element.DisableElement(false);
					}
				}
			}
		}
	}
}