using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

/// <summary>
/// Authored & Written by @mrobertscgd, adjusted and worked on collaborativley by Andrew Scott (andrewscott@icloud.com)
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Player
{
    /// <summary>
    /// PlayerController class
    /// 
    /// This class is what drives the player, using unitys default horizontal and vertical input methods
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public CharacterController controller; // Ref to the Character Controller Component.

        bool sprinting = false;
        float speed;
        public float walkSpeed = 6f; // The speed at which the player moves.
        public float sprintSpeed = 12f;
        public float gravity = -9.81f; // The amount of gravity that the is applied.
        public float moveDeadzone = 0.6f;

        public Transform groundCheck; // Transform for checking whether the player is grounded.
        public float groundDistance = 0.4f; // The distance of the player to the ground.
        public LayerMask groundMask; // Used for telling the controller what ground is.
        public bool paused = false; // Defines whether the game is paused, this might not be needed.
        public bool canMove = true;

        Vector3 velocity; // The velocity(speed) of the player.
        bool isGrounded; // Tells us whether the player is grounded.

        // Animator vairables
        public Animator animator;

        public Camera cam;
        public CinemachineBrain cmBrain;

        Vector3 right;
        Vector3 forward;

        public GameObject lookAtTarget;

        private void Start()
        {
            paused = true;
            canMove = true;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            SetupMovement();
        }

        void SetupMovement()
        {
            speed = walkSpeed;

            string currentSceneString = SceneManager.GetActiveScene().name;
            E_Scenes currentScene = HelperFunctions.StringToSceneEnum(currentSceneString);

            if (LoadCombatManager.instance.lastScene != E_Scenes.Null)
            {
                if (currentScene == LoadCombatManager.instance.lastScene)
                {
                    //Debug.Log("1" + transform.position + " || " + LoadCombatManager.instance.lastPos + paused);
                    transform.position = LoadCombatManager.instance.lastPos;
                    //Debug.Log("2" + transform.position + " || " + LoadCombatManager.instance.lastPos + paused);
                }
            }

            paused = false;
        }

        /// <summary>
        /// Update here, ran each frome. Here we call for the inputs.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprinting = true;
                speed = sprintSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                sprinting = false;
                speed = walkSpeed;
            }

            cmBrain.enabled = !paused;

            if (!paused && canMove)
            {
                GetInput();
            }
            else
            {
                HandleAnimations(new Vector3(0, 0, 0), false);
            }

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

            right = cam.transform.right;
            right = right.normalized;
            forward = cam.transform.forward;
            forward = forward.normalized;

            // Combine into one variable which gets used later
            Vector3 moveVector = right * x + forward * z;

            // Move using the controller component
            controller.Move(moveVector * speed * Time.deltaTime);

            // Calculate and apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            bool moving = moveVector != new Vector3(0, 0, 0);

            Vector3 inputVector = new Vector3(x, 0, z);

            HandleAnimations(inputVector, moving);
        }

        /// <summary>
        /// This function handles the sprite animations of taro. Interacts with the animator component.
        /// </summary>
        void HandleAnimations(Vector3 move, bool moving)
        {
            //Debug.Log("Moving: " + move);
            if (moving)
            {
                if (move.z > moveDeadzone)
                {
                    animator.SetInteger("Direction", 1);
                }
                else if (move.z < -moveDeadzone)
                {
                    animator.SetInteger("Direction", 2);
                }
                else if (move.x < -moveDeadzone)
                {
                    animator.SetInteger("Direction", 4);
                }
                else if (move.x > moveDeadzone)
                {
                    animator.SetInteger("Direction", 3);
                }
            }

            animator.SetBool("Moving", moving);
            animator.SetBool("Sprinting", sprinting);

            //Check to see player direction

            //Apply animation based on direction
        }
    }
}