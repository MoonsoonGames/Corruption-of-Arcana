using UnityEngine;
using Cinemachine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Player
{
    /// <summary>
    /// This handles rotating the player based on the mouse input.
    /// Some extra code in here that can probably be stripped out as in its current form it'll do all types of mouse looking, not just left and right
    /// </summary>
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public PlayerController playerController;
        //public CinemachineVirtualCamera vcam;

        public float xRotation = 0f; public float GetX() { return xRotation; }
        public float yRotation = 0f; public float GetY() { return yRotation; }

        private void Start()
        {
            // Lock and disable the cursor
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Checks to see if the game or controller is suspended. If so, the script won't do anything
            if (!playerController.paused && playerController.canMove)
                DoLook();
        }

        /// <summary>
        /// This takes the mouse input and converts it into rotation on the playerbody
        /// </summary>
        void DoLook()
        {
            // Get input axis
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

            xRotation -= mouseY;
            yRotation -= mouseX;
            // Clamp the up and down rotation.
            //xRotation = Mathf.Clamp(xRotation, -90, 90);
            yRotation = Mathf.Clamp(yRotation, -1.74f, 1.74f);
        }
    }
}