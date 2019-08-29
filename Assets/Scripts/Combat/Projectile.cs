using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool isFollowingTarget = false;
    [SerializeField] GameObject hitEffect = null;

    float damage = 0;
    Health target;

    private void Update()
    {
        if (target != null)
        {
            if(isFollowingTarget && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
        transform.LookAt(GetAimLocation());
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
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

