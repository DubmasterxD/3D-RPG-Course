using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction = null;

        public void StartAction(IAction action)
        {
            if (action != currentAction)
            {
                CancelCurrentAction();
                currentAction = action;
            }
        }

        public void CancelCurrentAction()
        {
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
        }
    }
}
