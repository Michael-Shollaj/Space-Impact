using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public class SICLayerManager {
		private const string DEFAUILT_LAYER = "Default";
		public static int DefaultLayer {
			get { return LayerMask.NameToLayer(DEFAUILT_LAYER); }
		}

		private const string SHIP_LAYER = "Ship";
		public static int ShipLayer {
			get { return LayerMask.NameToLayer(SHIP_LAYER); }
		}

		private const string ENEMY_LAYER = "Enemy";
		public static int EnemyLayer {
			get { return LayerMask.NameToLayer(ENEMY_LAYER); }
		}

		private const string PROJECTILE_LAYER = "Projectile";
		public static int ProjectileLayer {
			get { return LayerMask.NameToLayer(PROJECTILE_LAYER); }
		}

		private const string BOSS_LAYER = "Boss";
		public static int BossLayer {
			get { return LayerMask.NameToLayer(BOSS_LAYER); }
		}
	}
}