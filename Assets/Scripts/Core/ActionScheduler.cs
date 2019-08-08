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
                if (currentAction != null)
                    currentAction.Cancel();
                currentAction = action;
            }
        }
    }
}
