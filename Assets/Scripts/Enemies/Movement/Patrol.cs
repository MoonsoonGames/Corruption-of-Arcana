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
        public List<GameObject> pointsList = new List<GameObject>();
        private int destPoint = 0;
        private NavMeshAgent agent;
        public float timeToPatrol = 30f;
        public bool patrol = true;
        public GameObject CPatrolPoint;
        
        public Vector3 originalPos;
        public Vector3[] patrolPoints;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // OLD CODE
            // CPatrolPoint = GameObject.FindGameObjectWithTag("CenterPatrolPoint");
            // pointsList.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint"));
            // foreach (GameObject point in pointsList)
            // {
            //     point.transform.SetParent(null);
            // }
            // CPatrolPoint.transform.SetParent(null);

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
        /// I hate how I've wrote this.. don't read it.
        /// </summary>
        /// <param name="offset">The offset amount to add to each direction.</param>
        void GetNavHitPointsDiamond(float offset){
            // Check to make sure offset isn't 0
            if (offset == 0){
                Debug.LogError("No offset added to patrol pattern, returning to avoid weird behaviour..");
                return;
            }

            // Get the 4 points in the cardinal directions by adding the offset.
            Vector3 north = new Vector3(transform.position.x + offset, transform.position.z);
            Vector3 east = new Vector3(transform.position.x, transform.position.z - offset);
            Vector3 south = new Vector3(transform.position.x - offset, transform.position.z);
            Vector3 west = new Vector3(transform.position.x, transform.position.z + offset);


            patrolPoints = new Vector3[4];
            patrolPoints[0] = north;
            patrolPoints[1] = east;
            patrolPoints[2] = south;
            patrolPoints[3] = west;
        }


        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (pointsList.Count == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = pointsList[destPoint].transform.position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % pointsList.Count;
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