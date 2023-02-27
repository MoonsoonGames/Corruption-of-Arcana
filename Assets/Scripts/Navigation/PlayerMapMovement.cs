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
    public class PlayerMapMovement : MonoBehaviour
    {
        //All of this data will need to be saved as well as the current position when a random event occurs
        public NavigationWaypoint defaultWaypoint;
        NavigationWaypoint currentWaypoint;
        NavigationWaypoint[] minorWaypoints;
        NavigationWaypoint nextWaypoint;
        int currentWaypointIndex = 0;
        NavigationWaypoint destinationWaypoint;
        public float speed = 0.6f;
        public float distanceThreshold = 5f;

        bool setup = false;

        private void Start()
        {
            NavigationWaypoint loadWaypoint = SceneToNode(LoadingScene.instance.navScene);
            if (loadWaypoint != null)
                currentWaypoint = loadWaypoint;
            else
                currentWaypoint = defaultWaypoint;

            UpdateWaypoints();
            currentWaypoint.SetPlayer(this);
            currentWaypoint.Arrived();

            Vector3 pos = currentWaypoint.transform.position;
            pos.z = transform.position.z;
            transform.position = pos;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            setup = true;
        }

        E_Scenes enterLevel = E_Scenes.Null; public void SetLevel(E_Scenes scene) { enterLevel = scene; }

        /// <summary>
        /// Update here, ran each frome. Here we call for the inputs.
        /// </summary>
        void Update()
        {
            if (!setup) return;
            if (enterLevel != E_Scenes.Null)
            {
                if (Input.GetButton("Interact"))
                {
                    LoadingScene.instance.LoadScene(enterLevel, E_Scenes.Null, false);
                }
            }

            if (nextWaypoint == null) return;

            if (HelperFunctions.AlmostEqualVector3(transform.position, nextWaypoint.transform.position, distanceThreshold, new Vector3(0, 0, 1)))
            {
                if (currentWaypointIndex > minorWaypoints.Length)
                {
                    transform.position = new Vector3(destinationWaypoint.transform.position.x, destinationWaypoint.transform.position.y, transform.position.z);
                    //Debug.Log("Arrived");
                    currentWaypoint = destinationWaypoint;
                    ArrivedAtNode(currentWaypoint);
                    UpdateWaypoints();
                }
                else
                {
                    ArrivedAtNode(nextWaypoint);
                    GetNextWaypoint();
                }
            }
            else
            {
                //Debug.Log("Moving");
                Vector3 dir = nextWaypoint.transform.position - transform.position;
                dir.Normalize();
                dir.z = 0;

                transform.position += dir * speed;
            }
        }

        public void SetDestination(NavigationWaypoint destination)
        {
            bool canTravel = false;
            
            foreach (var item in currentWaypoint.paths)
            {
                if (item.waypoint == destination)
                {
                    canTravel = true;
                    minorWaypoints = item.minorWaypoints;
                }
            }

            if (canTravel)
            {
                currentWaypointIndex = 0;
                this.destinationWaypoint = destination;
                nextWaypoint = minorWaypoints[currentWaypointIndex];
            }
            else
            {
                Debug.Log("Waypoint not available");
            }
        }

        void ArrivedAtNode(NavigationWaypoint node)
        {
            node.Arrived();
            //Check scene in node, enterring at a specific entrance depending on path
            //Check random events in node
        }

        void GetNextWaypoint()
        {
            currentWaypointIndex++;

            currentWaypoint = nextWaypoint;

            if (currentWaypointIndex >= minorWaypoints.Length)
                nextWaypoint = destinationWaypoint;
            else
                nextWaypoint = minorWaypoints[currentWaypointIndex];
        }

        void UpdateWaypoints()
        {
            NavigationWaypoint[] allWaypoints = GameObject.FindObjectsOfType<NavigationWaypoint>();

            foreach (var waypoint in allWaypoints)
            {
                bool available = false;

                foreach (var paths in currentWaypoint.paths)
                {
                    if (paths.waypoint == waypoint)
                    {
                        available = true;
                    }
                }

                waypoint.SetAvailable(available);
            }
        }

        public SceneNode[] sceneNodes;

        public NavigationWaypoint SceneToNode(E_Scenes scene)
        {
            foreach (var item in sceneNodes)
            {
                if (item.scene == scene)
                {
                    return item.node;
                }
            }

            return null;
        }

        [System.Serializable]
        public struct SceneNode
        {
            public E_Scenes scene;
            public NavigationWaypoint node;
        }
    }
}