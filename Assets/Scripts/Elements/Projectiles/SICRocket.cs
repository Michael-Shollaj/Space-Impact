using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;
using SpaceImpact.GameCore;

namespace SpaceImpact {

	public class SICRocket : SICGameProjectile {
		// Public Variables
		[SerializeField] private float waveLength = 1f;
		[SerializeField] private float amplitude = 5f;
		[SerializeField] private float startTime;

		// Private Variables	
		private float time;
		private Vector3 minPosition;
		private Vector3 maxPosition;

		private Transform target;

		// Static Variables

		# region Projectile

		public override void Initialize(Transform owner, Transform sender) {
			base.Initialize(owner, sender);
			transform.position = sender.position;
			time = startTime;
			minPosition = -Vector3.up;
			maxPosition = Vector3.up;

			if (TargetType == UnitType.ENEMY) {
				SetTarget(SICGameUtility.GetNearestUntargettedEnemy(sender.position));
			}

			if (TargetType == UnitType.SPACE_SHIP) {
				target = SICGameManager.SharedInstance.SpaceShip.transform;
			}

			if (target != null) {
				startTime = Vector3.Distance(transform.position, target.position);
			}
		}

		public override void OnElementUpdate() {
			time += Time.deltaTime;
			float waveUp = Mathf.Lerp(minPosition.y, maxPosition.y, Mathf.PingPong(time, waveLength) / waveLength) * amplitude;

			if (target == null) {
				if (TargetType == UnitType.ENEMY) {
					SetTarget(SICGameUtility.GetNearestUntargettedEnemy(sender.position));
				}

				if (TargetType == UnitType.SPACE_SHIP) {
					target = SICGameManager.SharedInstance.SpaceShip.transform;
				}

				transform.Translate(new Vector3(Direction.x * MoveSpeed, waveUp, 0f) * Time.deltaTime);
			}
			else {
				Vector3 dir = (target.position - transform.position).normalized;
				transform.Translate(new Vector3(dir.x * MoveSpeed, waveUp + dir.y, 0f) * Time.deltaTime);

				if (!target.gameObject.activeInHierarchy) {
					target = null;
				}
			}
		}

		public override bool OnElementConstraint() {
			return transform.position.x > SICAreaBounds.MaxPosition.x ||
				transform.position.x < SICAreaBounds.MinPosition.x;
		}

		public override ProjectileType GetProjectileType() {
			return ProjectileType.ROCKET;
		}

		# endregion

		public void OnTriggerStay2D(Collider2D col) {
			if (col2D == null)
				return;

			// Both Enemy and Boss
			SICGameUnit unit = col.GetComponent<SICGameUnit>();
			if (unit != null) {
				if (unit.GetUnitType() == TargetType) {
					if (TargetType == UnitType.ENEMY) {
						SICGameEnemy enemy = col.GetComponent<SICGameEnemy>();
						if (enemy != null) {
							enemy.SubtractHP(Damage);
							SubtractDurability(1);
							SICGameManager.SharedInstance.GameMetrics.AddScore(enemy.ScorePoint);
						}
					}

					//if (TargetType == UnitType.SPACE_SHIP) {

					//}
				}
			}
		}

		public void SetTarget(Transform tgt) {
			if (tgt == null)
				return;

			Transform tmpTarget = tgt;
			SICGameEnemy enemyTarget = tmpTarget.GetComponent<SICGameEnemy>();

			float dirToOwnerX = Mathf.Sign(tgt.position.x - owner.transform.position.x);
			float dirX = Mathf.Sign(tgt.position.x - transform.position.x);

			if (enemyTarget == null || dirX < 0 || dirToOwnerX < 0) {
				target = null;
				return;
			}

			target = tmpTarget;

			if (enemyTarget.GetEnemyType() == EnemyType.BOSS)
				return;

			enemyTarget.SetTargetted(true);
		}
	}
}