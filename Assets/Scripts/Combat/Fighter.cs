using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
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
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat==Stat.Damage)
            {
                yield return equippedWeapon.Damage;
            }
        }
        
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat==Stat.Damage)
            {
                yield return equippedWeapon.PercentageBonus;
            }
        }

        public object CaptureState()
        {
            return equippedWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }

        //Animation event
        void Hit()
        {
            if (currentTarget != null)
            {
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
                currentTarget.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            if (currentTarget != null)
            {
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
                equippedWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, currentTarget, gameObject, damage);
            }
        }
    }
}
