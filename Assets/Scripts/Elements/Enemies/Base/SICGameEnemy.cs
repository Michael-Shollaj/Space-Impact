using UnityEngine;
using System.Collections;
using SpaceImpact.GameCore;
using System.Collections.Generic;
using SpaceImpact.Utility;

namespace SpaceImpact {

	public enum EnemyType {
		NONE = 0,
		ENEMY = 1,
		BOSS = 2
	}

	public class SICGameEnemy : SICGameUnit {
		// Public Variables	
		[SerializeField] private List<SICUnitFiring> unitFiring;

		// Private Variables
		private bool isTargetted;
		private bool hasCollided;

		// Static Variables

		public bool IsTargetted { get { return isTargetted; } }

		# region Game Element
		public override void OnEnable() {
			base.OnEnable();
			ResetElement();
		}

		public override void OnElementUpdate() {
			base.OnElementUpdate();
			transform.Translate(-Vector3.right * MoveSpeed * Time.deltaTime);
			UnitFiringUpdate();
		}

		public override bool OnElementConstraint() {
			return transform.position.x < SICAreaBounds.MinExPosition.x;
		}

		public override void DisableElement(bool showVFX = true) {
			base.DisableElement(showVFX);
		}

		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_ENEMY; }
		}
		# endregion

		public void UnitFiringUpdate() {
			unitFiring.ForEach(a => a.FiringUpdate(FireProjectile, -Vector3.right, UnitType.SPACE_SHIP));
		}

		public void OnTriggerEnter2D(Collider2D col) {
			SICGameElement element = col.GetComponent<SICGameElement>();
			if (element != null) {
				SICSpaceShip ship = element.GetComponent<SICSpaceShip>();
				if (ship != null && !hasCollided) {
					SICGameManager.SharedInstance.GameMetrics.AddScore(ScorePoint);

					int subtractedHP = (GetEnemyType() == EnemyType.ENEMY) ? 999 : 1;
					SubtractHP(subtractedHP);
					ship.SubtractHP(1);

					if (!ship.IsInvulnerable) {
						SICGameManager.SharedInstance.RespawnShip();
					}

					hasCollided = true;
					ResetElement();
				}

				//SICProjectiles projectile = col.GetComponent<SICProjectiles>();
				//if (projectile != null) {

				//}
			}
		}

		public void SetTargetted(bool tgt) {
			isTargetted = tgt;
		}

		public virtual EnemyType GetEnemyType() {
			return EnemyType.NONE;
		}

		public override UnitType GetUnitType() {
			return UnitType.ENEMY;
		}

		public override void ResetElement() {
			base.ResetElement();
			isTargetted = false;
			hasCollided = false;

			unitFiring.ForEach(a => a.Initialize());
		}
	}
}