using UnityEngine;
using System.Collections;

namespace SpaceImpact.Utility {

	public class SICGameSettings {
		// Static Variables
		public const string GAME_NAME = "Space Impact Clone";
		public const float GAME_WIDTH = 800f;
		public const float GAME_HEIGHT = 600f;
		public const int GAME_PIXELS_PER_UNIT = 100;

		public const int DEFAULT_BEAM_COUNT = 1;
		public const int DEFAULT_LASER_COUNT = 2;
		public const int DEFAULT_ROCKET_COUNT = 3;

		public const int DEFAULT_SCORE = 0;
		public const int DEFAULT_LIVES = 3;
		public const ProjectileType DEFAULT_SPECIAL = ProjectileType.ROCKET;
	}
}