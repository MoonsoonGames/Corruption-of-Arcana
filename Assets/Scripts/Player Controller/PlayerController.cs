using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace NecroPanda.Player
{
    /// <summary>
    /// PlayerController class
    /// 
    /// This class is what drives the player, using unitys default horizontal and vertical input methods
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public CharacterController controller; // Ref to the Character Controller Component.

        public float speed = 12f; // The speed at which the player moves.
        public float gravity = -9.81f; // The amount of gravity that the is applied.

        public Transform groundCheck; // Transform for checking whether the player is grounded.
        public float groundDistance = 0.4f; // The distance of the player to the ground.
        public LayerMask groundMask; // Used for telling the controller what ground is.
        public bool paused; // Defines whether the game is paused, this might not be needed.

        Vector3 velocity; // The velocity(speed) of the player.
        bool isGrounded; // Tells us whether the player is grounded.

        /// <summary>
        /// Update here, ran each frome. Here we call for the inputs.
        /// </summary>
        void Update()
        {
            GetInput();   
        }

        /// <summary>
        /// This function gets all of the KEYBOARD updates and converts those inputs into movement within
        /// the world space.
        /// </summary>
        void GetInput()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = speed * 2f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = speed / 2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}