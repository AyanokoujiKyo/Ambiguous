using UnityEngine;

namespace EasyDestuctibleWall
{
    public class DestructionManager : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float health = 100f;

        [Header("Damage multipliers")]
        [SerializeField] private float impactMultiplier = 2.25f;
        [SerializeField] private float twistMultiplier = 0.0025f;

        [Header("Stability / Filtering")]
        [Tooltip("Dacă e OFF, peretele NU ia damage din coliziuni random (ușă, pereți etc.).")]
        [SerializeField] private bool armed = false;

        [Tooltip("Damage doar dacă obiectul care lovește are unul din tag-urile de mai jos (ex: HammerHead).")]
        [SerializeField] private bool onlyDamageFromAllowedTags = true;

        [SerializeField] private string[] allowedTags = new[] { "Hammer", "HammerHead" };

        [Tooltip("Ignoră loviturile mai mici decât pragul (m/s).")]
        [SerializeField] private float minImpactSpeed = 1.2f;

        [Tooltip("Ignoră damage-ul din rotație până nu e armat.")]
        [SerializeField] private bool torqueDamageOnlyWhenArmed = true;

        private Rigidbody cachedRigidbody;
        private float minImpactSpeedSqr;

        private void Awake()
        {
            cachedRigidbody = GetComponent<Rigidbody>();
            minImpactSpeedSqr = minImpactSpeed * minImpactSpeed;
        }
        public void SetArmed(bool value) => armed = value;
        public void ApplyDamage(float amount)
        {
            if (!armed) return;
            health -= amount;
            TryFracture();
        }

        private void FixedUpdate()
        {
            if (torqueDamageOnlyWhenArmed && !armed) return;

            float ang = cachedRigidbody.angularVelocity.sqrMagnitude;
            if (ang > 1.5f) 
            {
                health -= Mathf.Round(ang * twistMultiplier);
                TryFracture();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!armed) return;
            float relVelSqr = collision.relativeVelocity.sqrMagnitude;
            if (relVelSqr < minImpactSpeedSqr) return;
            if (onlyDamageFromAllowedTags && !HasAllowedTag(collision.collider)) return;

            float dmg = relVelSqr * impactMultiplier;
            if (collision.rigidbody)
            {
                float m = Mathf.Clamp(collision.rigidbody.mass, 0.5f, 3.0f); 
                dmg *= m;
            }

            health -= dmg;
            TryFracture();
        }

        private bool HasAllowedTag(Collider col)
        {
            if (allowedTags == null || allowedTags.Length == 0) return true;

            for (int i = 0; i < allowedTags.Length; i++)
            {
                string t = allowedTags[i];
                if (!string.IsNullOrEmpty(t) && col.CompareTag(t)) return true;
            }
            return false;
        }

        private void TryFracture()
        {
            if (health > 0f) return;
            foreach (Transform child in transform)
            {
                Rigidbody spawnRB = child.gameObject.AddComponent<Rigidbody>();
                child.parent = null;

                spawnRB.linearVelocity = cachedRigidbody.GetPointVelocity(child.position);
                spawnRB.AddTorque(cachedRigidbody.angularVelocity, ForceMode.VelocityChange);
            }

            Destroy(gameObject);
        }
    }
}
