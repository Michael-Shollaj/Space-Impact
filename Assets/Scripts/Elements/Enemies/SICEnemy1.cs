using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public class SICEnemy1 : SICGameEnemy {
		// Public Variables	

		// Private Variables	

		// Static Variables

		# region Game Element
		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_ENEMY; }
		}
		# endregion

		public override EnemyType GetEnemyType() {
			return EnemyType.ENEMY;
		}
	}
}