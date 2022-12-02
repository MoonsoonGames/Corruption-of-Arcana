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
        private NavMeshAgent agent;
        private EnemyAI ai;
        public float timeToPatrol = 30f;
        public bool patrol = true;

        public Vector3 originalPos;
        public Vector3[] patrolPoints;

        [Space]
        [Header("Temp Offset")]
        public float patrolPointOffset;

        private enum Direction
        {
            North,
            East,
            South,
            West
        }

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
            patrolPoints = GetPatrolPointsDiamond(patrolPointOffset);
            agent = GetComponent<NavMeshAgent>();
            ai = GetComponent<EnemyAI>();

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Vector3 point = patrolPoints[i];

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

                        patrolPoints[i] = agent.destination;
                    }
                }
            }
            // ref https://gamedev.stackexchange.com/questions/93886/find-closest-point-on-navmesh-if-current-target-unreachable

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
        }

        /// <summary>
        /// Gets points in the 4 cardinal directions. Creates a diamond patrol pattern.
        /// 
        /// Rewrote this.. Still feels like there's a better way to do it
        /// </summary>
        /// <param name="offset">The offset amount to add to each direction.</param>
        Vector3[] GetPatrolPointsDiamond(float offset)
        {
            // Check to make sure offset isn't 0
            if (offset == 0)
            {
                Debugger.instance.SendDebug("No offset added to patrol pattern, returning to avoid weird behaviour..", 3);
                return null;
            }

            float x = transform.position.x;
            float z = transform.position.z;

            Vector3[] patrolPoints = new Vector3[4];

            patrolPoints[(int)Direction.North] = new Vector3(x + offset, 0, z);
            patrolPoints[(int)Direction.East] = new Vector3(x, 0, z - offset);
            patrolPoints[(int)Direction.South] = new Vector3(x - offset, 0, z);
            patrolPoints[(int)Direction.West] = new Vector3(x, 0, z + offset);

            return patrolPoints;
        }

        void GotoNextPoint()
        {
            // Returns if no points have been set up.
            if (patrolPoints.Length == 0)
                return;

            // Set the agent to go to the currently selected point.
            agent.destination = patrolPoints[destPoint];

            // Choose the next point in the array as the destination
            // cyling to the start if necessary.
            destPoint = (destPoint + 1) % patrolPoints.Length;
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
        /// <summary>
        /// Callback to draw gizmos only if the object is selected.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            foreach (Vector3 point in patrolPoints)
            {
                Gizmos.DrawSphere(point, 1);
            }
        }
    }

}