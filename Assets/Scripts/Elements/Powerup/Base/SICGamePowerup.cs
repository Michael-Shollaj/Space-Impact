using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SpaceImpact.Utility;

namespace SpaceImpact {

	public enum PowerupType {
		NONE = 0,
		BALL = 1
	}

	public class SICGamePowerup : SICGameUnit {
		// Public Variables	
		[SerializeField] private ProjectileType projectileType;
		[SerializeField] private bool randomize;

		// Private Variables	

		// Static Variables

		# region Game Element
		public override void OnElementUpdate() {
			base.OnElementUpdate();
			transform.Translate(-Vector3.right * MoveSpeed * Time.deltaTime);
		}

		public override bool OnElementConstraint() {
			return transform.position.x < SICAreaBounds.MinPosition.x;
		}

		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_POWERUP; }
		}
		# endregion

		# region Game Unit
		public override UnitType GetUnitType() {
			return UnitType.POWERUP;
		}
		# endregion

		public virtual PowerupType GetPowerupType() {
			return PowerupType.NONE;
		}

		public virtual void OnTriggerEnter2D(Collider2D col) {
			SICGameUnit unit = col.GetComponent<SICGameUnit>();
			if (unit != null) {
				SICSpaceShip ship = unit.GetComponent<SICSpaceShip>();
				if (ship != null) {
					ProjectileType projectile = (randomize) ? GetRandomProjectile() : projectileType;
					if (projectile == ProjectileType.MISSILE) {
						ship.AddHP(1);
					}
					else 
						ship.SetSpecial(projectile);

					DisableElement(false);
				}
			}
		}

		public ProjectileType GetRandomProjectile() {
			ProjectileType result = ProjectileType.NONE;

			int maxCount = Enum.GetNames(typeof(ProjectileType)).Length;
			float[] weights = new float[maxCount];
			float curWeight = 0f;

			for (int i = 0; i < maxCount; i++) {
				curWeight += (1f / maxCount);
				weights[i] = curWeight;
			}

			float randNum = UnityEngine.Random.Range(0f, 100f) / 100f;

			int indx = 0;
			for (int i = 0; i < weights.Length; i++) {
				if (randNum < weights[i]) {
					indx = i;
					break;
				}
			}

			result = (ProjectileType)indx;

			if (result == ProjectileType.NONE)
				result = ProjectileType.ROCKET;

			return result;
		}
	}
}