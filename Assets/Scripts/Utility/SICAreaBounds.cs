using UnityEngine;
using System.Collections;

namespace SpaceImpact.Utility {

	[ExecuteInEditMode]
	public class SICAreaBounds : MonoBehaviour {
		// Public Variables

		// Private Variables
		private const float OFFSET = 0.5f;

		// Static Variables

		private static Transform thisT;

		private static Vector3 ptUpperRight;
		private static Vector3 ptLowerRight;
		private static Vector3 ptUpperLeft;
		private static Vector3 ptLowerLeft;

		private static Vector3 ptExUpperRight;
		private static Vector3 ptExLowerRight;
		private static Vector3 ptExUpperLeft;
		private static Vector3 ptExLowerLeft;

		public static Transform ThisT { get { return thisT; } }

		public static Vector3 MinPosition { get { return thisT.position + ptLowerLeft; } }

		public static Vector3 MaxPosition { get { return thisT.position + ptUpperRight; } }

		public static Vector3 MinExPosition { get { return thisT.position + ptExLowerLeft; } }

		public static Vector3 MaxExPosition { get { return thisT.position + ptExUpperRight; } }

		public static Vector3 PtUpperRight { get { return ptUpperRight; } }

		public static Vector3 PtLowerRight { get { return ptLowerRight; } }

		public static Vector3 PtUpperLeft { get { return ptUpperLeft; } }

		public static Vector3 PtLowerLeft { get { return ptLowerLeft; } }

		public static Vector3 PtExUpperRight { get { return ptExUpperRight; } }

		public static Vector3 PtExLowerRight { get { return ptExLowerRight; } }

		public static Vector3 PtExUpperLeft { get { return ptExUpperLeft; } }

		public static Vector3 PtExLowerLeft { get { return ptExLowerLeft; } }

		private void Awake() {
			thisT = transform;
		}

		private void OnEnable() {
			ptUpperRight = new Vector3((SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT,
				(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT, 0.0f);
			ptLowerRight = new Vector3((SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT,
				-(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT, 0.0f);
			ptUpperLeft = new Vector3(-(SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT,
				(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT, 0.0f);
			ptLowerLeft = new Vector3(-(SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT,
				-(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT, 0.0f);

			ptExUpperRight = new Vector3((SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT + OFFSET,
				(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT + OFFSET, 0.0f);
			ptExLowerRight = new Vector3((SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT + OFFSET,
				-(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT - OFFSET, 0.0f);
			ptExUpperLeft = new Vector3(-(SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT - OFFSET,
				(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT + OFFSET, 0.0f);
			ptExLowerLeft = new Vector3(-(SICGameSettings.GAME_WIDTH / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT - OFFSET,
				-(SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT - OFFSET, 0.0f);
		}

	# if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.DrawLine(transform.position + ptUpperRight, transform.position + ptLowerRight);
			Gizmos.DrawLine(transform.position + ptLowerRight, transform.position + ptLowerLeft);
			Gizmos.DrawLine(transform.position + ptLowerLeft, transform.position + ptUpperLeft);
			Gizmos.DrawLine(transform.position + ptUpperLeft, transform.position + ptUpperRight);

			Gizmos.color = Color.green;

			Gizmos.DrawLine(transform.position + ptExUpperRight, transform.position + ptExLowerRight);
			Gizmos.DrawLine(transform.position + ptExLowerRight, transform.position + ptExLowerLeft);
			Gizmos.DrawLine(transform.position + ptExLowerLeft, transform.position + ptExUpperLeft);
			Gizmos.DrawLine(transform.position + ptExUpperLeft, transform.position + ptExUpperRight);
		}
	# endif
	}
}