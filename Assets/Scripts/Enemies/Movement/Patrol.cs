using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    /// TODO: rewrite to avoid the use of gameobjects for points, consider using navmesh hits in 4 places.
    /// </summary>
    public class Patrol : MonoBehaviour
    {
        //public List<GameObject> pointsList = new List<GameObject>();
        private int destPoint = 0;
        private NavMeshAgent agent;
        public float timeToPatrol = 30f;
        public bool patrol = true;
        //public GameObject CPatrolPoint;
        
        public Vector3 originalPos;
        public Vector3[] patrolPoints;

        private enum Direction {
            North,
            East,
            South,
            West
        }

        void Start()
        {
            patrolPoints = GetPatrolPointsDiamond(1f);
            agent = GetComponent<NavMeshAgent>();

            // OLD CODE
            // CPatrolPoint = GameObject.FindGameObjectWithTag("CenterPatrolPoint");
            // pointsList.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint"));
            // foreach (GameObject point in pointsList)
            // {
            //     point.transform.SetParent(null);
            // }
            // CPatrolPoint.transform.SetParent(null);

            //Get the original position, i.e the center.
            originalPos = gameObject.transform.position;

            foreach (Vector3 point in patrolPoints)
            {
                

                // Check to see if point is valid
                // ref https://gamedev.stackexchange.com/questions/93886/find-closest-point-on-navmesh-if-current-target-unreachable
                // if not get the closest thing to it.
            }

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            StartCoroutine(Cooldown(timeToPatrol));
            GotoNextPoint();
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
                Debug.LogError("No offset added to patrol pattern, returning to avoid weird behaviour..");
                return null;
            }

            float x = transform.position.x;
            float z = transform.position.z;

            Vector3[] patrolPoints = new Vector3[4];

            patrolPoints[(int)Direction.North] = new Vector3(x + offset, z);
            patrolPoints[(int)Direction.East] = new Vector3(x, z - offset);
            patrolPoints[(int)Direction.South] = new Vector3(x - offset, z);
            patrolPoints[(int)Direction.West] = new Vector3(x, z + offset);

            return patrolPoints;
        }

        void GotoNextPoint()
        {
            // OLD CODE
            /*
            // Returns if no points have been set up
            if (pointsList.Count == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = pointsList[destPoint].transform.position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % pointsList.Count;
            */

            // Returns if no points have been set up.
            if (patrolPoints.Length == 0)
                return;

            // Set the agent to go to the currently selected point.
            agent.destination = patrolPoints[destPoint];

            // Choose the next point in the array as the destination
            // cyling to the start if necessary.
            destPoint = (destPoint + 1) % patrolPoints.Length;
        }

        public void StopPatrol()
        {
            // OLD CODE
            // agent.SetDestination(CPatrolPoint.transform.position);
            // agent.autoBraking = true;
            // if (agent.transform.position == CPatrolPoint.transform.position)
            // {
            //     foreach (GameObject point in pointsList)
            //     {
            //         point.transform.SetParent(this.gameObject.transform);
            //     }
            //     CPatrolPoint.transform.SetParent(this.gameObject.transform);

            //     Debug.Log("Disabled the patrol movement module on " + this.name);
            //     this.enabled = false;
            // }

            agent.SetDestination(originalPos);
            agent.autoBraking = true;

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
            Debug.Log("Running patrol cooldown");
            patrol = false;
        }
    }
}