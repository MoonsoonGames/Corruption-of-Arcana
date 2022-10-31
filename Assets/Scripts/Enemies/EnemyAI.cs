using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com and @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.AI
{
    public class EnemyAI : MonoBehaviour
    {
        bool active = false; public bool GetActive() { return active; }
        GameObject player;
        NavMeshAgent agent;
        Vector3 startPos;

        public Object enemyObject;
        public bool boss;

        //state variables
        public AIState currentState; // The current state of the AI. Wandering, Fleeing etc.
        public int avoidancePriority = 15; // The level of avoidance priority for the agent. lower = more important. Might be worth setting this based on the type of the enemy
        public float timer = 0f; // Internal timer used for state changes and tracking.


        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            startPos = transform.position;
        }

        public void ActivateAI(GameObject playerRef)
        {
            player = playerRef;
            active = true;
            Debug.Log("Activate AI");
        }

        public void DeactivateAI()
        {
            player = null;
            active = false;
            Debug.Log("Deactivate AI");
            agent.SetDestination(startPos);
        }

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                HFSM();
                Timer();
                agent.SetDestination(player.transform.position);
            }
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
                default:
                    currentState = AIState.Nothing;
                    break;
            }
        }

        #region util
        public void Timer()
        {
            // Start the timer
            timer += Time.deltaTime;
        }
        #endregion
    }
}