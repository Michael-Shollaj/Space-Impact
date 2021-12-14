using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;
using System.Collections.Generic;

namespace SpaceImpact {

	public enum StageType {
		NONE = 0,
		STAGE_1 = 1,
		STAGE_2 = 2,
		STAGE_3 = 3,
		STAGE_4 = 4,
		STAGE_5 = 5
	}

	public class SICGameStage : SICGameElement {
		// Public Variables	
		[SerializeField] private SICWaypoint waypoints;
		[SerializeField] private Transform pStartPosition;

		// Private Variables
		private List<Vector3> elementPos;

		private List<SICGameElement> stageElements;
		private List<SICGameEnemy> stageEnemies;
		private List<SICGameUnit> stageUnits;

		private SICGameBoss stageBoss;

		// Static Variables

		private const string POINT_A_NAME = "Point A";
		private const string POINT_B_NAME = "Point B";

		public SICWaypoint Waypoints { get { return waypoints; } }

		public Transform PStartPosition { get { return pStartPosition; } }

		public List<SICGameElement> StageElements { get { return stageElements; } }

		public SICGameBoss StageBoss { get { return stageBoss; } }

		public Transform PointA { get { return waypoints.PointA; } }

		public Transform PointB { get { return waypoints.PointB; } } 

		# region Game Element
		public override void Awake() {
			base.Awake();
			elementPos = new List<Vector3>();

			stageElements = new List<SICGameElement>();
			SICGameUtility.GetElementsRecursively(transform, ref stageElements);

			if (stageElements.Count <= 0)
				return;

			stageUnits = new List<SICGameUnit>();
			SICGameUtility.GetUnitRecursively(transform, ref stageUnits);

			stageEnemies = new List<SICGameEnemy>();
			SICGameUtility.GetEnemyRecursively(transform, ref stageEnemies);

			stageBoss = stageEnemies.Find(a => a.GetEnemyType() == EnemyType.BOSS) as SICGameBoss;

			for (int i = 0; i < stageElements.Count; i++) {
				elementPos.Add(stageElements[i].transform.position);
			}
		}

		public override void OnEnable() {
			base.OnEnable();
			ResetStage();
			AddElementsToPool();
		}

		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_GAME_STAGE;  }
		}
		# endregion

		public void AddElementsToPool() {
			if (stageElements == null || stageElements.Count <= 0)
				return;

			for (int i = 0; i < stageElements.Count; i++) {
				for (int j = 0; j < SICObjectPoolManager.SharedInstance.ObjParents.Count; j++) {
					if (!stageElements[i].IsElementActive)
						continue;

					SICGameElement reference = SICObjectPoolManager.SharedInstance.ObjParents[j].refObj;
					SICGameElement obj = stageElements[i];

					SICGameStage refStage = reference.GetComponent<SICGameStage>();
					SICGameStage objStage = obj.GetComponent<SICGameStage>();
					if (refStage != null && objStage != null) {
						if (refStage.GetStageType() != objStage.GetStageType()) {
							continue;
						}
					}

					SICGameProjectile refProjectile = reference.GetComponent<SICGameProjectile>();
					SICGameProjectile objProjectile = obj.GetComponent<SICGameProjectile>();
					if (refProjectile != null && objProjectile != null) {
						if (refProjectile.GetProjectileType() != objProjectile.GetProjectileType()) {
							continue;
						}
					}

					SICGameBoss refBoss = reference.GetComponent<SICGameBoss>();
					SICGameBoss objBoss = obj.GetComponent<SICGameBoss>();
					if (refBoss != null && objBoss != null) {
						if (refBoss.GetBossType() != objBoss.GetBossType()) {
							continue;
						}
					}

					SICGameParticle refParticle = reference.GetComponent<SICGameParticle>();
					SICGameParticle objParticle = obj.GetComponent<SICGameParticle>();
					if (refParticle != null && objParticle != null) {
						if (refParticle.GetParticleType() != objParticle.GetParticleType()) {
							continue;
						}
					}

					SICGameEnemy refEnemy = reference.GetComponent<SICGameEnemy>();
					SICGameEnemy objEnemy = obj.GetComponent<SICGameEnemy>();
					if (refEnemy != null && objEnemy != null) {
						if (refEnemy.GetEnemyType() != objEnemy.GetEnemyType()) {
							continue;
						}
					}

					SICGameUnit refUnit = reference.GetComponent<SICGameUnit>();
					SICGameUnit objUnit = obj.GetComponent<SICGameUnit>();
					if (refUnit != null && objUnit != null) {
						if (refUnit.GetUnitType() != objUnit.GetUnitType()) {
							continue;
						}
					}

					SICGamePowerup refPowerup = reference.GetComponent<SICGamePowerup>();
					SICGamePowerup objPowerup = obj.GetComponent<SICGamePowerup>();
					if (refPowerup != null && objPowerup != null) {
						if (refPowerup.GetPowerupType() != objPowerup.GetPowerupType()) {
							continue;
						}
					}

					// Does not include the type
					if (stageElements[i].OBJECT_ID == SICObjectPoolManager.SharedInstance.ObjParents[j].refObj.OBJECT_ID) {
						SICObjectPoolManager.SharedInstance.ObjParents[j].AddObject(stageElements[i].gameObject, false);
					}
				}
			}
		}

		public void ResetStage() {
			SICGameUtility.SetAllElementsRecursively(transform, true);

			if (stageElements.Count <= 0)
				return;

			for (int i = 0; i < stageElements.Count; i++) {
				stageElements[i].transform.position = elementPos[i];
			}
		}

		public virtual StageType GetStageType() {
			return StageType.NONE;
		}
	}
}