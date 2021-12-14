using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public class SICPowerup : SICGamePowerup {
		// Public Variables	

		// Private Variables	

		// Static Variables

		# region Game Powerup
		public override PowerupType GetPowerupType() {
			return PowerupType.BALL;
		}
		# endregion
	}
}