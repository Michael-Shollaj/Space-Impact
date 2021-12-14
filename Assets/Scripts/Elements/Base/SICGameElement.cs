using UnityEngine;
using System.Collections;
using SpaceImpact.Utility;

namespace SpaceImpact {

	public enum ElementType {
		NONE = 0,
		UNIT = 1,
		PROJECTILE = 2,
		PARTICLE = 3
	}

	public abstract class SICGameElement : MonoBehaviour {
		// Public Variables	
		[SerializeField] private float moveSpeed = 5f;

		// Private Variables
		protected Rigidbody2D rBody2D;
		protected Collider2D col2D;

		private float originalMoveSpeed;

		public abstract string OBJECT_ID { get; }

		public float MoveSpeed { get { return moveSpeed; } }

		public bool IsElementActive { get { return gameObject.activeInHierarchy; } }

		public virtual void Awake() {
			rBody2D = GetComponent<Rigidbody2D>();
			col2D = GetComponent<Collider2D>();

			originalMoveSpeed = moveSpeed;
		}

		public virtual void OnEnable() {
			InitializeRBody();
		}

		public virtual void Start() { }

		public virtual void OnElementUpdate() { }

		public virtual bool OnElementConstraint() { return false; }

		public virtual void ResetElement() {
			moveSpeed = originalMoveSpeed;
		}

		public void Update() {
			if (OnElementConstraint()) {
				DisableElement(false);
			}

			if (!IsElementVisible())
				return;

			OnElementUpdate();
		}

		public virtual bool IsElementVisible() {
			return !(transform.position.x < SICAreaBounds.MinExPosition.x || transform.position.x > SICAreaBounds.MaxExPosition.x) &&
				!(transform.position.y < SICAreaBounds.MinExPosition.y || transform.position.y > SICAreaBounds.MaxExPosition.y);
		}

		public void InitializeRBody() {
			if (rBody2D == null)
				return;

			rBody2D.fixedAngle = true;
			rBody2D.isKinematic = true;
		}

		public void AddSpeed(float spd) {
			this.moveSpeed += spd;
		}

		public void SubtractSpeed(float spd) {
			this.moveSpeed -= spd;
		}

		public void SetSpeed(float spd) {
			this.moveSpeed = spd;
			this.moveSpeed = Mathf.Clamp(this.moveSpeed, 0, int.MaxValue);
		}

		public void EnableElement() {
			if (IsElementActive)
				return;

			gameObject.SetActive(true);
		}

		public virtual void DisableElement(bool showVFX = true) {
			if (!IsElementActive)
				return;

			gameObject.SetActive(false);
		}

		public virtual ElementType GetElementType() {
			return ElementType.NONE;
		}

		public virtual void ShowExplosionFX() {
			GameObject explodeObj = SICObjectPoolManager.SharedInstance.GetObject(SICObjectPoolName.OBJECT_PARTICLE, ParticleType.UNIT_EXPLOSION);
			if (explodeObj != null) {
				explodeObj.transform.position = transform.position;
				SICGameParticle explode = explodeObj.GetComponent<SICGameParticle>();
				explode.EnableElement();
			}
		}
	}
}