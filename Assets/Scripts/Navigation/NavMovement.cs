using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class NavMovement : MonoBehaviour
    {
        //public CharacterController controller; // Ref to the Character Controller Component.

        bool sprinting = false;
        public float speed = 12f; // The speed at which the player moves.
        public float moveDeadzone = 0.6f;

        public bool paused = false; // Defines whether the game is paused, this might not be needed.

        Vector3 velocity; // The velocity(speed) of the player.
        bool isGrounded; // Tells us whether the player is grounded.

        Vector3 horizontal;
        Vector3 vertical;

        private void Start()
        {
            paused = true;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            SetupMovement();
        }

        void SetupMovement()
        {
            /* Get last nav pos from load manager
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
            */

            paused = false;
        }

        /// <summary>
        /// Update here, ran each frome. Here we call for the inputs.
        /// </summary>
        void Update()
        {
            if (!paused)
            {
                GetInput();
            }
        }

        /// <summary>
        /// This function gets all of the KEYBOARD updates and converts those inputs into movement within
        /// the world space.
        /// </summary>
        void GetInput()
        {
            // Get the movement axis
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            horizontal = gameObject.transform.right;
            horizontal = horizontal.normalized;
            vertical = gameObject.transform.up;
            vertical = vertical.normalized;

            // Combine into one variable which gets used later
            Vector3 moveVector = horizontal * x + vertical * z;

            // Move using the controller component
            gameObject.transform.position += moveVector * speed * Time.deltaTime;
            //controller.Move(moveVector * speed * Time.deltaTime);

            /*
            bool moving = moveVector != new Vector3(0, 0, 0);

            Vector3 inputVector = new Vector3(x, 0, z);

            HandleAnimations(inputVector, moving);
            */
        }

        /// <summary>
        /// This function handles the sprite animations of taro. Interacts with the animator component.
        /// </summary>
        void HandleAnimations(Vector3 move, bool moving)
        {
            /*
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
            */
        }
    }
}