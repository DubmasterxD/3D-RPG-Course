using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }
        
        void LateUpdate()
        {
            transform.forward = mainCam.transform.forward;
        }
    }
}
