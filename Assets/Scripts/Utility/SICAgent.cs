using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public class SICAgent : MonoBehaviour {
		// Public Variables	
		
		[SerializeField] private float distanceThreshold = 0.01f;
		[SerializeField] private Transform[] path;

		// Private Variables	
		private int curNode;

		private float agentSpeed;
		private float startTime;
		private float totalDistance;

		// Static Variables

		public void OnEnable() {
			curNode = 0;
			startTime = Time.time;
			totalDistance = Vector3.Distance(transform.position, path[curNode].position);
			transform.position = path[curNode].position;
		}

		public void Update() {
			if (Vector3.Distance(transform.position, path[curNode].position) <= distanceThreshold) {
				curNode++;

				if (curNode > path.Length - 1)
					curNode = 0;

				totalDistance = Vector3.Distance(transform.position, path[curNode].position);
				startTime = Time.time;
			}

			float moveSpeed = (Time.time - startTime) * agentSpeed;
			float moveTime = moveSpeed / totalDistance;
			transform.position = Vector3.Lerp(transform.position, path[curNode].position, moveTime);
		}

		public void SetPath(Transform[] path) {
			this.path = path;
		}

		public void SetAgentSpeed(float spd) {
			this.agentSpeed = spd;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			if (path == null || path.Length <= 0)
				return;

			for (int i = 0; i < path.Length; i++) {
				if (i < path.Length - 1) {
					Gizmos.DrawLine(path[i].position, path[i + 1].position);
				}

				Gizmos.DrawLine(path[path.Length - 1].position, path[0].position);
			}
		}
# endif
	}
}