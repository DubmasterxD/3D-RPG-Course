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

        // Update is called once per frame
        void Update()
        {
            transform.forward = mainCam.transform.forward;
        }
    }
}
