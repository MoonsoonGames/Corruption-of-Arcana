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
        
        // Movement variables
        float x;
        float z;
        bool sprinting;

        Vector3 moveVector;   // Combined input for all movement

        /// <summary>
        /// Update here, ran each frome. Here we call for the inputs.
        /// </summary>
        void Update()
        {
            GetInput();
            HandleAnimations(); 
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
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            // Combine into one variable which gets used later
            moveVector = transform.right * x + transform.forward * z;

            // Move using the controller component
            controller.Move(moveVector * speed * Time.deltaTime);

            // Input checks
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprinting = true;
                speed = speed * 2f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                sprinting = false;
                speed = speed / 2f;
            }

            // Calculate and apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        /// <summary>
        /// This function handles the sprite animations of taro. Interacts with the animator component.
        /// </summary>
        void HandleAnimations()
        {
            //Check to see player direction
            if (controller.isGrounded)
            {
                Vector3 moveDirection = transform.TransformDirection(moveVector);

                //Apply animation based on direction
                moveDirection *= speed;

                animator.SetBool("Move", moveDirection != new Vector3(0, 0, 0));
                
                // Setup the switch for forward and backwards.
                switch (moveVector.x)
                {
                    // Forwards
                    case > 0:
                        animator.SetInteger("Direction", 1);
                    break; 

                    // Backwards
                    case < 0:
                        animator.SetInteger("Direction", 2);
                    break; 
                }

                // Setup the switch for left and right.
                switch (moveVector.z)
                {
                    // Left
                    case > 0:
                        animator.SetInteger("Direction", 3);
                    break;

                    // Right
                    case < 0:
                        animator.SetInteger("Direction", 4);
                    break;
                }

                // Set up the sprinting variable. Based on input.
                animator.SetBool("Sprinting", sprinting);
            }
        }
    }
}