using UnityEngine;
using System.Collections;

namespace SpaceImpact.Utility {

	public class SICCamera : MonoBehaviour {
		private Camera cam;
		private float cameraSize;

		public float CameraSize { get { return cameraSize; } }

		private void Awake() {
			cam = GetComponent<Camera>();
		}

		private void Start() {
			if (cam != null) {
				if (!cam.orthographic) {
					cam.orthographic = true;
				}

				cameraSize = (SICGameSettings.GAME_HEIGHT / 2) / SICGameSettings.GAME_PIXELS_PER_UNIT;
				cam.orthographicSize = cameraSize;
			}
		}
	}
}