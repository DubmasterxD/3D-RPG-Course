using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        Vector3 guardPosition;

        GameObject player;
        Fighter fighter;
        Health thisCharacter;
        Mover mover;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            thisCharacter = GetComponent<Health>();
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (!thisCharacter.IsDead)
            {
                if (PlayerInRange() && fighter.CanAttack(player))
                {
                    fighter.Attack(player);
                }
                else
                {
                    mover.StartMovementAction(guardPosition);
                }
            }
        }

        private bool PlayerInRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }  
}
