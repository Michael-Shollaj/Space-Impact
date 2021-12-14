using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	[System.Serializable]
	public class SICUnitFiring {
		// Public Variables	
		[SerializeField] private float fireRate = 0.5f;
		[SerializeField] private ProjectileType projectileType;

		// Private Variables	
		private float fireTime;

		// Static Variables

		public void Initialize() {
			fireTime = 0f;
		}

		public void FiringUpdate(System.Action<ProjectileType, Vector3, UnitType> firing, Vector3 direction, UnitType targetType) {
			fireTime += Time.deltaTime;

			if (fireTime >= fireRate) {

				if (firing != null) {
					firing(projectileType, direction, targetType);
				}

				fireTime = 0f;
			}
		}
	}
}