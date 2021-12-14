using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;
using SpaceImpact.GameCore;

namespace SpaceImpact {

	public class SICLaser : SICGameProjectile {
		// Public Variables	
		[SerializeField] private float rayDuration = 0.2f;
		[SerializeField] private int instances = 1;

		// Private Variables
		private float time;
		private Transform origin;
		private int instanceCount;

		// Static Variables

		# region Projectiles

		public override void Initialize(Transform owner, Transform sender) {
			base.Initialize(owner, sender);
			time = 0f;
			origin = sender;

			Vector3 scale = transform.localScale;
			scale.x = 0f;
			transform.localScale = scale;

			transform.position = origin.position;

			instanceCount = 0;
			InvokeRepeating("DestroyElementsOnHit", 0.1f, (rayDuration / instances));
		}

		public override void OnElementUpdate() {
			Vector3 endPoint = (Direction.x > 0) ? SICAreaBounds.MaxPosition : SICAreaBounds.MinPosition;

			float rayLength = (origin.position.x < 0) ?
				(endPoint.x + Mathf.Abs(origin.position.x)) :
				(endPoint.x - Mathf.Abs(origin.position.x));

			float laserLength = rayLength * 10f;

			Vector3 scale = transform.localScale;
			//scale.x = laserLength;
			scale.x = Mathf.Lerp(0f, laserLength, time * MoveSpeed);
			transform.localScale = scale;

			transform.position = origin.position;

# if UNITY_EDITOR
			Debug.DrawLine(origin.position, new Vector3(endPoint.x, origin.position.y));
			Debug.DrawLine(origin.position, origin.position + (Vector3.right * rayLength), Color.red);
# endif
		}

		public override bool OnElementConstraint() {
			time += Time.deltaTime;
			return time > rayDuration;
		}

		public override ProjectileType GetProjectileType() {
			return ProjectileType.LASER;
		}

		# endregion

		public void DestroyElementsOnHit() {
			instanceCount++;
			if (instanceCount > instances) {
				CancelInvoke("DestroyElementsOnHit");
				return;
			}

			// Moving Laser
			//RaycastHit2D[] objHit = Physics2D.RaycastAll(origin.position, Vector3.right, (transform.localScale.x / 10f), 1 << SICLayerManager.EnemyLayer);

			Vector3 endPoint = (Direction.x > 0) ? SICAreaBounds.MaxPosition : SICAreaBounds.MinPosition;

			float rayLength = (origin.position.x < 0) ?
				(endPoint.x + Mathf.Abs(origin.position.x)) :
				(endPoint.x - Mathf.Abs(origin.position.x));

			RaycastHit2D[] objHit = Physics2D.RaycastAll(origin.position, Vector3.right, rayLength, 
				1 << SICLayerManager.EnemyLayer | 1 << SICLayerManager.BossLayer | 1 << SICLayerManager.ShipLayer);

			if (objHit == null || objHit.Length <= 0)
				return;

			for (int i = 0; i < objHit.Length; i++) {
				SICGameUnit unit = objHit[i].transform.GetComponent<SICGameUnit>();
				if (unit != null) {
					if (unit.GetUnitType() == TargetType) {
						if (TargetType == UnitType.ENEMY) {
							SICGameEnemy enemyElement = objHit[i].transform.GetComponent<SICGameEnemy>();
							if (enemyElement != null) {
								enemyElement.SubtractHP(Damage);
								SICGameManager.SharedInstance.GameMetrics.AddScore(enemyElement.ScorePoint);
							}
						}

						if (TargetType == UnitType.SPACE_SHIP) {
							unit.SubtractHP(1);
							SICGameManager.SharedInstance.RespawnShip();
						}
					}
				}
			}
		}
	}
}