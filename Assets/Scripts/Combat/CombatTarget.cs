using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.gameObject.GetComponent<Fighter>();
            if (!fighter.CanAttack(this.gameObject))
            {
                return false;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                fighter.Attack(this.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}
