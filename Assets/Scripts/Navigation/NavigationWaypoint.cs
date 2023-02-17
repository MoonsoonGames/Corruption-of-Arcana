using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class NavigationWaypoint : MonoBehaviour
    {
        PlayerMapMovement player;

        public WaypointNodes[] paths;
        //public RandomEvent[] events;
        //public E_Scenes loadScene;

        private void Start()
        {
            player = GameObject.FindObjectOfType<PlayerMapMovement>();
        }

        public void ChooseDestination()
        {
            player.SetDestination(this);
        }

        public Image image;

        public void SetAvailable(bool available)
        {
            if (image == null) return;
            image.color = available ? Color.green : Color.red;
        }
    }

    [System.Serializable]
    public struct WaypointNodes
    {
        public NavigationWaypoint waypoint;
        public NavigationWaypoint[] minorWaypoints;
        //Entrance Count
    }
}
