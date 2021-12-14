using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceImpact {

	public class SICInvulnerable : SICGameParticle {
		// Public Variables	
		[SerializeField] private SICAgent[] agents;

		// Private Variables	
		private List<Vector3> agentOriginalPos;

		// Static Variables

		# region Game Element

		public override void Awake() {
		    base.Awake();

		    agentOriginalPos = new List<Vector3>();

		    if (agents.Length <= 0)
		        return;

		    for (int i = 0; i < agents.Length; i++) {
		        agentOriginalPos.Add(transform.root.position + agents[i].transform.position);
		    }
		}

		public override void OnEnable() {
		    base.OnEnable();
		    ResetFX();
		}

		public override ParticleType GetParticleType() {
		    return ParticleType.UNIT_INVULNERABILITY;
		}

		# endregion

		public void ResetFX() {
		    for (int i = 0; i < agents.Length; i++) {
		        agents[i].transform.position = agentOriginalPos[i];
		        agents[i].SetAgentSpeed(MoveSpeed);
		    }
		}
	}
}