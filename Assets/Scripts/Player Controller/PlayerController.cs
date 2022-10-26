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

        // Animator vairables
        public Animator animator;

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
            // Check to see if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Get the movement axis
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Combine into one variable which gets used later
            Vector3 moveVector = transform.right * x + transform.forward * z;

            // Move using the controller component
            controller.Move(moveVector * speed * Time.deltaTime);

            // Input checks
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = speed * 2f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = speed / 2f;
            }

            // Calculate and apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            HandleAnimations(moveVector, true);
        }

        /// <summary>
        /// This function handles the sprite animations of taro. Interacts with the animator component.
        /// </summary>
        void HandleAnimations(Vector3 move, bool moving)
        {
            switch (move.x)
            {
                case > 0:
                    //anim code for right? direction
                    break;
                case < 0:
                    //anim code for left? direction
                    break;
            }

            switch (move.z)
            {
                case > 0:
                    //anim code for forward? direction
                    break;
                case < 0:
                    //anim code for backward? direction
                    break;
            }

            if (moving)
            {
                //anim code set to move
            }

            //Check to see player direction

            //Apply animation based on direction
        }
    }
}