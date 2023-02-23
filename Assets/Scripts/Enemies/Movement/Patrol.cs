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
namespace Necropanda.AI.Movement
{
    /// <summary>
    /// <para>Simple Patrol module that can be added to the AI.</para>
    /// Will drop points around the origin, and make the AI move to them. Most values are exposed for editing.
    /// 
    /// TODO: rewrite to avoid the use of gameobjects for points, consider using navmesh hits in 4 places. DONE
    /// TODO: fix state change when stopping patrol
    /// </summary>
    public class Patrol : MonoBehaviour
    {
        private int destPoint = 0;
        public NavMeshAgent agent;
        private EnemyAI ai;
        public float timeToPatrol = 30f;
        public bool patrol = true;

        public Vector3 originalPos;
        public List<Vector3> patrolPoints;

        void Start()
        {
            Setup();

            //Get the original position, i.e the center.
            originalPos = gameObject.transform.position;


            StartCoroutine(Cooldown(timeToPatrol));
            GotoNextPoint();
        }

        private void Setup()
        {
            agent = GetComponent<NavMeshAgent>();
            ai = GetComponent<EnemyAI>();

            for (int i = 0; i < patrolPoints.Count; i++)
            {
                IsPointValid(patrolPoints[i], i);
            }

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
        }

        /// <summary>
        /// Checks to see whether the point is valid, if not, move to the closest valid point on the navmesh.
        /// 
        /// ref: https://gamedev.stackexchange.com/questions/93886/find-closest-point-on-navmesh-if-current-target-unreachable
        /// </summary>
        /// <param name="point">the point to check</param>
        /// <param name="iterator">Which point in the list to check</param>
        public void IsPointValid(Vector3 point, int iterator)
        {
            // Check to see if point is valid
            bool isPathValid = agent.CalculatePath(point, agent.path);
            if (!isPathValid)
            {
                // This should set the destination to the closest thing on the navmesh
                if (agent.path.status == NavMeshPathStatus.PathComplete ||
                agent.hasPath && agent.path.status == NavMeshPathStatus.PathPartial)
                {
                    agent.SetDestination(point);
                    Debugger.instance.SendDebug($"Point was off navmesh, point moved to: {agent.destination}", 2);

                    patrolPoints[iterator] = agent.destination;
                }
            }
        }

        void GotoNextPoint()
        {
            // Returns if no points have been set up.
            if (patrolPoints.Count == 0)
                return;

            // Set the agent to go to the currently selected point.
            agent.destination = patrolPoints[destPoint];

            // Choose the next point in the array as the destination
            // cyling to the start if necessary.
            destPoint = (destPoint + 1) % patrolPoints.Count;
            // StartCoroutine(Cooldown(3));
        }

        public void StopPatrol()
        {
            agent.SetDestination(originalPos);
            agent.autoBraking = true;
            // Reset the state back to nothing
            ai.currentState = AIState.Nothing;

            this.enabled = false;
        }


        void Update()
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (patrol)
            {
                if (patrolPoints.Count == 0)
                {
                    // Log that the patrol list is empty. Might want to generate some basic points if thise is the case but erroring is the safest
                    Debugger.instance.SendDebug("No Patrol points setup! returning out of patrol state");
                    // Set patrol to false, this stops the patrol script.
                    patrol = false;
                }

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GotoNextPoint();
            }
            else
                StopPatrol();
        }

        IEnumerator Cooldown(float coolDown)
        {
            yield return new WaitForSeconds(coolDown);
            Debugger.instance.SendDebug("Running patrol cooldown");
            patrol = false;
        }
    }

}