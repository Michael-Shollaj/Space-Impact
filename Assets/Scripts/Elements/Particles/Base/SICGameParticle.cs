using UnityEngine;
using System.Collections;

namespace SpaceImpact {

	public enum ParticleType {
		NONE = 0,
		UNIT_EXPLOSION = 1,
		UNIT_INVULNERABILITY = 2
	}

	public class SICGameParticle : SICGameElement {
		// Public Variables	

		// Private Variables	

		// Static Variables

		# region Game Element
		public override string OBJECT_ID {
			get { return SICObjectPoolName.OBJECT_PARTICLE; }
		}
		# endregion

		public override ElementType GetElementType() {
			return  ElementType.PARTICLE;
		}

		public virtual ParticleType GetParticleType() {
			return ParticleType.NONE;
		}
	}
}