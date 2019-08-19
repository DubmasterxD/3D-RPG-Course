using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;

        Health currentTarget;
        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (currentTarget != null && !currentTarget.IsDead)
            {
                if (Vector3.Distance(transform.position, currentTarget.transform.position) > weaponRange)
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
                currentTarget.TakeDamage(weaponDamage);
            }
        }
    }
}
