using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public enum BossType {
		NONE = 0,
		BOSS_1 = 1,
		BOSS_2 = 2,
		BOSS_3 = 3,
		BOSS_4 = 4,
		BOSS_5 = 5,
		BOSS_6 = 6
	}

	public class SICGameBoss : SICGameEnemy {
		// Public Variables	

		// Private Variables	

		// Static Variables

		# region Game Element
		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_ENEMY; }
		}
		# endregion

		public virtual BossType GetBossType() {
			return BossType.NONE;
		}

		public override EnemyType GetEnemyType() {
			return EnemyType.BOSS;
		}
	}
}