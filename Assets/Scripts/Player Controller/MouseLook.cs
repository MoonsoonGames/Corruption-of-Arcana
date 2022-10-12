using UnityEngine;
using Cinemachine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace NecroPanda.Player
{
    /// <summary>
    /// This handles rotating the player based on the mouse input.
    /// Some extra code in here that can probably be stripped out as in its current form it'll do all types of mouse looking, not just left and right
    /// </summary>
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public Transform playerBody;
        public PlayerController playerController;

        float xRotation = 0f;

        private void Start()
        {
            // Lock and disable the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Checks to see if the game or controller is suspended. If so, the script won't do anything
            if (playerController.paused)
            {
                return;
            }
            else
            {
                DoLook();
            }
        }

        /// <summary>
        /// This takes the mouse input and converts it into rotation on the playerbody
        /// </summary>
        void DoLook()
        {
            // Get in input axis
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            // Clamp the up and down rotation.
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            // Apply the rotation
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}