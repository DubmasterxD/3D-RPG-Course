using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.gameObject.GetComponent<Fighter>();
            if (!fighter.CanAttack(gameObject))
            {
                return false;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                fighter.Attack(gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}
