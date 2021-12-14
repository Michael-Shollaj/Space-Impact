using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceImpact {

	public enum UnitType {
		NONE = 0,
		SPACE_SHIP = 1,
		ENEMY = 2,
		POWERUP = 3
	}

	public class SICGameUnit : SICGameElement {
		// Public Variables	
		[SerializeField] private SpriteRenderer mainTexture;
		[SerializeField] private int healthPoints = 1;
		[SerializeField] private int scorePoint = 0;
		[SerializeField] private bool invulnerable;
		[SerializeField] private SICInvulnerable invulnerableVFX;
		[SerializeField] private Transform projectileNozzle;
		[SerializeField] private List<SICGameProjectile> refProjectiles;
		[SerializeField] private List<SICElementMover> elementMover;

		// Private Variables	
		private SpriteRenderer originalTexture;
		private int originalHealthPoints;
		private int originalScorePoint;
		private bool originalInvulnerable;

		private int curMove;

		// Static Variables

		public SpriteRenderer MainTexture { get { return mainTexture; } }

		public bool IsInvulnerable { get { return invulnerable; } }

		# region Game Element
		public override void Awake() {
			base.Awake();
			originalTexture = mainTexture;
			originalHealthPoints = healthPoints;
			originalScorePoint = scorePoint;
			originalInvulnerable = invulnerable;

			if (elementMover.Count > 0) {
				//transform.position = elementMover[0].Path[0];
				for (int i = 0; i < elementMover.Count; i++) {
					elementMover[i].Initialize(transform);
				}
			}
		}

		public override void OnEnable() {
			base.OnEnable();
			curMove = 0;
			SetInvulnerability(invulnerable);
		}

		public override void OnElementUpdate() {
			base.OnElementUpdate();

			if (elementMover.Count > 0) {
				elementMover[curMove].UpdateMove();
				if (elementMover[curMove].IsFinished) {
					curMove++;
					curMove = Mathf.Clamp(curMove, 0, elementMover.Count - 1);
				}
			}

			//for (int i = 0; i < elementMover.Count; i++) {
			//    elementMover[i].UpdateMove();
			//}
		}

		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_UNIT; }
		}
		# endregion

		public override ElementType GetElementType() {
			return ElementType.UNIT;
		}

		public void FireProjectile(ProjectileType type, Vector3 direction, UnitType targetType) {
			if (projectileNozzle == null || type == ProjectileType.NONE)
				return;

			GameObject projectileObj = SICObjectPoolManager.SharedInstance.GetObject(GetRefProjectile(type).OBJECT_ID, type);
			if (projectileObj != null) {
				SICGameProjectile projectile = projectileObj.GetComponent<SICGameProjectile>();
				projectile.EnableElement();
				projectile.SetTargetType(targetType);
				projectile.SetDirection(direction);
				projectile.Initialize(transform, projectileNozzle);
			}
		}

		public SICGameProjectile GetRefProjectile(ProjectileType type) {
			return refProjectiles.Find(a => a.GetProjectileType() == type);
		}

		public virtual UnitType GetUnitType() {
			return UnitType.NONE;
		}

		public int HealthPoints { get { return healthPoints; } }

		public int ScorePoint { get { return scorePoint; } }

		public override void DisableElement(bool showVFX = true) {
			base.DisableElement(showVFX);

			if (!showVFX)
				return;

			ShowExplosionFX();
		}

		public void AddHP(int hp) {
			this.healthPoints += hp;
			SetHP(this.healthPoints);
		}

		public void SubtractHP(int hp) {
			if (invulnerable)
				return;

			this.healthPoints -= hp;
			SetHP(this.healthPoints);

			if (healthPoints <= 0)
				DisableElement();
		}

		public virtual void SetHP(int hp) {
			this.healthPoints = hp;
			this.healthPoints = Mathf.Clamp(this.healthPoints, 0, int.MaxValue);
		}

		public void AddScorePoint(int score) {
			this.scorePoint += score;
			SetScorePoint(this.scorePoint);
		}

		public void SubtractScorePoint(int score) {
			this.scorePoint -= score;
			SetScorePoint(this.scorePoint);
		}

		public virtual void SetScorePoint(int score) {
			this.scorePoint = score;
			this.scorePoint = Mathf.Clamp(this.scorePoint, 0, int.MaxValue);
		}

		public void SetInvulnerability(bool invulnerable) {
			this.invulnerable = invulnerable;

			if (invulnerableVFX == null)
				return;

			invulnerableVFX.gameObject.SetActive(this.invulnerable);
		}

		public override void  ResetElement() {
 			base.ResetElement();
			mainTexture = originalTexture;
			healthPoints = originalHealthPoints;
			scorePoint = originalScorePoint;
			invulnerable = originalInvulnerable;
		}

# if UNITY_EDITOR
		private void OnDrawGizmos() {
			for (int i = 0; i < elementMover.Count; i++) {
				elementMover[i].DrawGizmos();
			}
		}
# endif
	}
}