using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public class SICExplode : SICGameParticle {
		// Public Variables	
		[SerializeField] private ParticleSystem particle;

		// Private Variables	

		// Static Variables

		# region Game Element
		public override bool OnElementConstraint() {
			return !particle.IsAlive();
		}
		# endregion

		public override ParticleType GetParticleType() {
			return ParticleType.UNIT_EXPLOSION;
		}
	}
}