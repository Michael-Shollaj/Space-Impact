using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;
using SpaceImpact.GameCore;

namespace SpaceImpact {

	public class SICBeam : SICGameProjectile {
		// Public Variables	
		[SerializeField] private float beamHeight = 50f;

		// Private Variables

		// Static Variables

		# region Projectiles

		public override void Initialize(Transform owner, Transform sender) {
			base.Initialize(owner, sender);
			transform.position = sender.position;

			Vector3 scale = transform.localScale;
			scale.y = beamHeight;
			transform.localScale = scale;
		}

		public override void OnElementUpdate() {
			transform.Translate(Direction * MoveSpeed * Time.deltaTime);
		}

		public override bool OnElementConstraint() {
			return transform.position.x > SICAreaBounds.MaxPosition.x ||
				transform.position.x < SICAreaBounds.MinPosition.x;
		}

		public override ProjectileType GetProjectileType() {
			return ProjectileType.BEAM;
		}

		# endregion

		public override void OnTriggerEnter2D(Collider2D col) {
			base.OnTriggerEnter2D(col);
			if (col.gameObject.layer == SICLayerManager.ProjectileLayer) {
				SICGameProjectile projectileElement = col.GetComponent<SICGameProjectile>();

				if (projectileElement == null)
					return;

				if (projectileElement.Owner == owner)
					return;

				if (projectileElement.GetProjectileType() == ProjectileType.LASER)
					return;

				projectileElement.SubtractDurability(999);
				SICGameManager.SharedInstance.GameMetrics.AddScore(projectileElement.ScorePoint);
				SubtractDurability(999);
				ShowExplosionFX();
			}
		}

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
	}
}