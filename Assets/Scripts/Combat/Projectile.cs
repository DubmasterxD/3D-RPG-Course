using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isFollowingTarget = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;

        float damage = 0;
        GameObject instigator;
        Health target;

        private void Update()
        {
            if (target != null)
            {
                if (isFollowingTarget && !target.IsDead)
                {
                    transform.LookAt(GetAimLocation());
                }
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            else
            {
                return target.transform.position + Vector3.up * targetCapsule.height / 3 * 2;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == target.gameObject)
            {
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, transform.rotation);
                }
                target.TakeDamage(instigator, damage);
                DestroyParticle();
            }
        }

        private void DestroyParticle()
        {
            foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
            {
                var emission = particle.emission;
                emission.rateOverTime = 0;
            }
            foreach (TrailRenderer trail in GetComponentsInChildren<TrailRenderer>())
            {
                trail.emitting = false;
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }

}
