using UnityEngine;
using System.Collections;
using SpaceImpact.GameCore;

namespace SpaceImpact {

	public enum ProjectileType {
		NONE = 0,
		MISSILE = 1,
		LASER = 2,
		ROCKET = 3,
		BEAM = 4
	}

	public class SICGameProjectile : SICGameElement {
		// Public Variables	
		[SerializeField] private SpriteRenderer mainTexture;
		[SerializeField] private int durability = 1;
		[SerializeField] private int damage = 1;
		[SerializeField] private int scorePoint = 5;

		// Private Variables
		private SpriteRenderer originalTexture;
		private int originalDurability;
		private int originalDamage;
		private int originalScorePoint;

		private Vector3 direction;
		private UnitType targetType;

		protected Transform owner;
		protected Transform sender;

		// Static Variables

		public SpriteRenderer MainTexture { get { return mainTexture; } }

		public int Durability { get { return durability; } }

		public int Damage { get { return damage; } }

		public Vector3 Direction { get { return direction; } }

		public UnitType TargetType { get { return targetType; } }

		public int ScorePoint { get { return scorePoint; } } 

		public Transform Owner { get { return owner; } }

		public Transform Sender { get { return sender; } } 

		# region Game Element
		public override void Awake() {
			base.Awake();
			originalTexture = mainTexture;
			originalDurability = durability;
			originalDamage = damage;
			originalScorePoint = scorePoint;
			direction = Vector3.right;
		}

		public override void OnEnable() {
			base.OnEnable();
			ResetElement();
		}

		public override void ResetElement() {
			base.ResetElement();
			mainTexture = originalTexture;
			durability = originalDurability;
			damage = originalDamage;
			scorePoint = originalScorePoint;
		}

		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_PROJECTILE; }
		}
		# endregion

		public override ElementType GetElementType() {
			return ElementType.PROJECTILE;
		}

		public virtual void Initialize(Transform owner, Transform sender) {
			this.owner = owner;
			this.sender = sender;
		}

		public virtual ProjectileType GetProjectileType() {
			return ProjectileType.NONE;
		}

		public virtual void OnTriggerEnter2D(Collider2D col) {
			if (col2D == null)
				return;

			// Both Enemy and Boss
			SICGameUnit unit = col.GetComponent<SICGameUnit>();
			if (unit != null) {
				if (unit.GetUnitType() == targetType) {
					if (targetType == UnitType.ENEMY) {
						SICGameEnemy enemy = col.GetComponent<SICGameEnemy>();
						if (enemy != null) {
							SubtractDurability(1);
							SICGameManager.SharedInstance.GameMetrics.AddScore(enemy.ScorePoint);
							enemy.SubtractHP(damage);
						}
					}

					if (targetType == UnitType.SPACE_SHIP) {
						SICSpaceShip ship = unit.GetComponent<SICSpaceShip>();
						SubtractDurability(999);
						ship.SubtractHP(1);

						if (ship.IsInvulnerable)
							return;

						SICGameManager.SharedInstance.RespawnShip();
					}
				}
			}
		}

		public void AddDamage(int dmg) {
			this.damage += dmg;
			SetDamage(this.damage);
		}

		public void SubtractDamage(int dmg) {
			this.damage -= dmg;
			SetDamage(this.damage);
		}

		public void SetDamage(int dmg) {
			this.damage = dmg;
			this.damage = Mathf.Clamp(this.damage, 0, int.MaxValue);
		}

		public void AddDurability(int dur) {
			this.durability += dur;
			SetDurability(this.durability);
		}

		public void SubtractDurability(int dur) {
			this.durability -= dur;
			SetDurability(this.durability);

			if (this.durability <= 0)
				DisableElement();
		}

		public void SetDurability(int dur) {
			this.durability = dur;
			this.durability = Mathf.Clamp(this.durability, 0, int.MaxValue);
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

		public void SetDirection(Vector3 dir) {
			direction = dir;
		}

		public void SetTargetType(UnitType type) {
			targetType = type;
		}
	}
}