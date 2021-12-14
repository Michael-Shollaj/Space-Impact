using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;

namespace SpaceImpact {

	[ExecuteInEditMode]
	public class SICWaypoint : MonoBehaviour {
		// Public Variables	
		[SerializeField] private Transform pointA;
		[SerializeField] private Transform pointB;

		// Private Variables
		private Vector3 upperRight;
		private Vector3 lowerRight;
		private Vector3 upperLeft;
		private Vector3 lowerLeft;

		// Static Variables

		public Transform PointA { get { return pointA; } }

		public Transform PointB { get { return pointB; } }

		public Vector3 UpperRight { get { return upperRight; } }

		public Vector3 LowerRight { get { return lowerRight; } }

		public Vector3 UpperLeft { get { return upperLeft; } }

		public Vector3 LowerLeft { get { return lowerLeft; } }

		private void OnEnable() {
			upperRight = pointB.position + SICAreaBounds.PtUpperRight;
			lowerRight = pointB.position + SICAreaBounds.PtLowerRight;
			upperLeft = pointA.position + SICAreaBounds.PtUpperLeft;
			lowerLeft = pointA.position + SICAreaBounds.PtLowerLeft;
		}

# if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.DrawLine(upperRight, lowerRight);
			Gizmos.DrawLine(lowerRight, lowerLeft);
			Gizmos.DrawLine(lowerLeft, upperLeft);
			Gizmos.DrawLine(upperLeft, upperRight);
		}

# endif
	}
}