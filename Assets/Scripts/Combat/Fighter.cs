using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Weapon equippedWeapon = null;
        float timeSinceLastAttack = Mathf.Infinity;

        Health currentTarget;
        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (currentTarget != null && !currentTarget.IsDead)
            {
                if (Vector3.Distance(transform.position, currentTarget.transform.position) > equippedWeapon.Range)
                {
                    mover.MoveTo(currentTarget.transform.position, 1);
                }
                else
                {
                    mover.Cancel();
                    AttackBehaviour();
                }
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            Animator anim = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, anim);
            equippedWeapon = weapon;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(currentTarget.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().ResetTrigger("cancelAttack");
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject target)
        {
            return !target.GetComponent<Health>().IsDead;
        }

        public void Attack(GameObject target)
        {
            currentTarget = target.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("cancelAttack");
            mover.Cancel();
            currentTarget = null;
        }

        //Animation event
        void Hit()
        {
            if (currentTarget != null)
            {
                currentTarget.TakeDamage(equippedWeapon.Damage);
            }
        }

        void Shoot()
        {
            if (currentTarget != null)
            {
                equippedWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, currentTarget);
            }
        }
    }
}
