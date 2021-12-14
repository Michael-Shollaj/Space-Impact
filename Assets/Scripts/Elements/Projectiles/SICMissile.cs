using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;
using SpaceImpact.GameCore;

namespace SpaceImpact {

	public class SICMissile : SICGameProjectile {

		# region Projectiles
		public override void Initialize(Transform owner, Transform sender) {
			base.Initialize(owner, sender);
			transform.position = sender.position;
		}

		public override void OnElementUpdate() {
			transform.Translate(Direction * MoveSpeed * Time.deltaTime);
		}

		public override bool OnElementConstraint() {
			return transform.position.x > SICAreaBounds.MaxPosition.x ||
				transform.position.x < SICAreaBounds.MinPosition.x;
		}

		public override ProjectileType GetProjectileType() {
			return ProjectileType.MISSILE;
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


				if (projectileElement.GetProjectileType() != ProjectileType.MISSILE &&
					projectileElement.GetProjectileType() != ProjectileType.ROCKET)
					return;

				projectileElement.SubtractDurability(999);
				SICGameManager.SharedInstance.GameMetrics.AddScore(projectileElement.ScorePoint);
				SubtractDurability(999);
				ShowExplosionFX();
			}
		}
	}
}