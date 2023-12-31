using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Necropanda.Utils.Debugger;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com and @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.AI
{
    [RequireComponent(typeof(ModuleManager))]
    public class EnemyAI : MonoBehaviour
    {
        bool active = false; public bool GetActive() { return active; }
        GameObject player;
        public NavMeshAgent agent;
        Vector3 startPos;
        private Animator animator;
        public bool debugMode = false;

        [Header("Statistics")]
        public CharacterStats enemyStats;
        public bool boss;
        public GameObject bossAI;
        public bool endCombatIfKilled = false;

        //state variables
        [Header("AI State Variables")]
        public ModuleManager moduleManager;
        public AIState startState; // The state the AI will go to after the initial timer has passed in the nothing state
        public AIState currentState; // The current state of the AI. Wandering, Fleeing etc.

        public bool doOverrideState; // If true, the AI will only stay in this state. Regardless of anything else.
        public AIState overrideState;
        public int avoidancePriority = 15; // The level of aDebugvoidance priority for the agent. lower = more important. Might be worth setting this based on the type of the enemy
        public float timer = 0f; // Internal timer used for state changes and tracking.

        [Header("Wandering Variables")]
        public float wanderingCoolDown;
        public float wanderRadius;
        public bool returnHomeAfterWander = true; // This defines whether the AI should return home after wandering.

        private NavMeshHit hit; // Used for determining where the AI moves to.
        private bool blocked = false; // Internal true/false for checking whether the current AI path is blocked.


        #region Checking Variables
        bool hasDebuggedWandering;
        bool hasDebuggedPatrol;
        #endregion


        private void Start()
        {
            animator = GetComponent<Animator>();
            moduleManager = gameObject.GetComponent<ModuleManager>();
            moduleManager.ChangeModuleState(1, false);
            moduleManager.ChangeModuleState(2, false);

            agent = GetComponent<NavMeshAgent>();
            startPos = transform.position;

            wanderRadius = gameObject.GetComponent<SphereCollider>().radius;

            if (debugMode)
            {
                Debugger.instance.SendDebug($"Debug mode enabled on {gameObject.name}!", 2);
            }
        }

        public void ActivateAI(GameObject playerRef)
        {
            player = playerRef;
            active = true;
            //Debugger.instance.SendDebug("Activate AI");
        }

        // Update is called once per frame
        void Update()
        {
            if (doOverrideState)
            {
                currentState = overrideState;
            }
            else
            {
                HFSM();
            }

            Timer();

            bool moving =
                (
                agent.velocity.x > 0.5f || agent.velocity.x < -0.5f ||
                agent.velocity.y > 0.5f || agent.velocity.y < -0.5f ||
                agent.velocity.z > 0.5f || agent.velocity.z < -0.5f
                );

            animator.SetBool("Moving", moving);
        }

        /// <summary>
        /// This is the brain of the AI and controls its states.
        /// Main AI Logic. Incorporates AI HFSM (Hierarchical Finite State Machine) flow.
        /// State Explanations:
        ///         - Nothing: AI does nothing, will be static and not process anything. Currently will switch out of this state
        ///                    after 10 seconds.
        ///         - Wandering: AI will wander within a radius.
        ///         - Chasing: Chases the target or player.
        ///         - Patrolling: AI Drops points around it and patrols them for a set period.
        /// /// </summary>
        void HFSM()
        {
            switch (currentState)
            {
                case AIState.Nothing:
                    // Disable the other modules when the AI is doing nothing. We pass nothing here for that.
                    moduleManager.ChangeModuleState();
                    // Set the state to nothing.
                    currentState = AIState.Nothing;

                    // Check if the AI has been running for long enough for a state switch.
                    // probably also need to reset the timer here too.
                    if (timer > 5 && !doOverrideState)
                    {
                        currentState = startState;
                    }
                    else
                    {
                        currentState = overrideState;
                    }
                    break;

                case AIState.Chasing:
                    // Disable the other modules
                    moduleManager.ChangeModuleState();
                    try
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    catch (System.NullReferenceException)
                    {
                        player = GameObject.FindGameObjectWithTag("Player");
                    }
                    break;

                case AIState.Wandering:
                    moduleManager.ChangeModuleState(1, true);
                    moduleManager.ChangeModuleState(2, false);
                    moduleManager.wander.WanderInRadius(blocked, hit);
                    if (!hasDebuggedWandering)
                    {
                        //Debugger.instance.SendDebug("Enabled Wandering module on Enemy AI " + gameObject.name);
                        hasDebuggedWandering = true;
                    }


                    break;

                case AIState.Patrolling:
                    moduleManager.ChangeModuleState(1, false);
                    moduleManager.ChangeModuleState(2, true);
                    if (!hasDebuggedPatrol)
                    {
                        //Debugger.instance.SendDebug("Enabled Patrolling module on Enemy AI " + gameObject.name);
                        hasDebuggedPatrol = true;
                    }
                    break;

                case AIState.Following:
                    Debug.Log("test");
                    if (GetBossDistance() >= 5)
                    {

                        agent.SetDestination(bossAI.transform.position);
                    }
                    break;
            }
        }

        #region util
        public void Timer()
        {
            // Start the timer
            timer += Time.deltaTime;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                currentState = AIState.Chasing;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            currentState = AIState.Patrolling;
        }

        private float GetBossDistance()
        {
            return Vector3.Distance(transform.position, bossAI.transform.position);
        }
        #endregion
    }
}