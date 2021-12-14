using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceImpact {

	[System.Serializable]
	public class SICElementMover {
		// Public Variables	
		[SerializeField] private bool isLooping;
		[SerializeField] private float speed = 0.1f;
		[SerializeField] private float distanceThreshold = 0.01f;
		[SerializeField] private float startDelay;
		[SerializeField] private bool copyPosX;
		[SerializeField] private bool copyPosY;
		[SerializeField] private bool copyPosZ;
		//[SerializeField] private bool startAtOwnerPosition;
		[SerializeField] private List<Vector3> waypoints;

		// Private Variables	
		private int curNode;

		private float startTime;
		private float totalDistance;
		private bool isFinished;

		private float time;
		private bool initTime;

		private Transform owner;

		// Static Variables

		public bool IsFinished { get { return isFinished; } }

		public List<Vector3> Path { get { return waypoints; } }

		public void Initialize(Transform owner) {
			this.owner = owner;
			curNode = 0;
			startTime = Time.time;
			isFinished = false;
			time = 0f;
			initTime = false;
			totalDistance = Vector3.Distance(owner.position, waypoints[curNode]);
		}

		public void UpdateMove() {
			if (time < startDelay) {
				time += Time.deltaTime;
				return;
			}

			if (!initTime) {
				startTime = Time.time;
				initTime = true;
			}

			if (isFinished)
				return;

			UpdateOwnerPosition();

			if (Vector3.Distance(owner.position, waypoints[curNode]) <= distanceThreshold) {
				curNode++;

				if (isLooping && curNode > waypoints.Count - 1) {
					curNode = 0;
				}

				totalDistance = Vector3.Distance(owner.position, waypoints[curNode]);
				startTime = Time.time;
			}

			float moveSpeed = (Time.time - startTime) * speed;
			float moveTime = moveSpeed / totalDistance;
			owner.position = Vector3.Lerp(owner.position, waypoints[curNode], moveTime);

			if (!isLooping && Vector3.Distance(owner.position, waypoints[curNode]) <= distanceThreshold)
				isFinished = true;
		}

		public void SetPath(List<Vector3> path) {
			this.waypoints = path;
		}

		public void SetSpeed(float spd) {
			this.speed = spd;
		}

		public void UpdateOwnerPosition() {
			if (!copyPosX && !copyPosY && !copyPosZ)
				return;

			for (int i = 0; i < waypoints.Count; i++) {
				Vector3 pos = waypoints[i];
				if (copyPosX) {
					pos.x = owner.position.x;
				}
				else if (copyPosY) {
					pos.y = owner.position.y;
				}
				else if (copyPosZ) {
					pos.z = owner.position.z;
				}
				waypoints[i] = pos;
			}
		}

		public void DrawGizmos() {
			if (waypoints == null || waypoints.Count <= 0)
				return;

			for (int i = 0; i < waypoints.Count; i++) {
				if (i < waypoints.Count - 1) {
					Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
				}

				Gizmos.DrawLine(waypoints[waypoints.Count - 1], waypoints[0]);
			}
		}
	}
}