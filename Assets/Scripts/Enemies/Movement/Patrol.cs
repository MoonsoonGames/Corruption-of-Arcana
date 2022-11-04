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

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            CPatrolPoint = GameObject.FindGameObjectWithTag("CenterPatrolPoint");
            pointsList.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint"));
            foreach (GameObject point in pointsList)
            {
                point.transform.SetParent(null);
            }
            CPatrolPoint.transform.SetParent(null);

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            StartCoroutine(Cooldown(timeToPatrol));
            GotoNextPoint();
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
            agent.SetDestination(CPatrolPoint.transform.position);
            agent.autoBraking = true;
            if (agent.transform.position == CPatrolPoint.transform.position)
            {
                foreach (GameObject point in pointsList)
                {
                    point.transform.SetParent(this.gameObject.transform);
                }
                CPatrolPoint.transform.SetParent(this.gameObject.transform);

                Debug.Log("Disabled the patrol movement module on " + this.name);
                this.enabled = false;
            }
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