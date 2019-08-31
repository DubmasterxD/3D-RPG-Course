using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Weapon equippedWeapon = null;
        float timeSinceLastAttack = Mathf.Infinity;
        
        public Health currentTarget { get; private set; }
        Mover mover;


        private void Start()
        {
            mover = GetComponent<Mover>();
            if (equippedWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (currentTarget == null || currentTarget.IsDead)
            {
                return;
            }
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

        public void EquipWeapon(Weapon weapon)
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.DespawEquippedWeapon();
            }
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
                currentTarget.TakeDamage(gameObject, equippedWeapon.Damage);
            }
        }

        void Shoot()
        {
            if (currentTarget != null)
            {
                equippedWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, currentTarget, gameObject);
            }
        }

        public object CaptureState()
        {
            return equippedWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }
    }
}
