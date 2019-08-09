using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        float waypointGizmosRadius = 0.2f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointGizmosRadius);
                int j = GetNextIndex(i);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(j));

            }
        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
