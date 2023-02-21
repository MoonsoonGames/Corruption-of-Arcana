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
        PlayerMapMovement player; public void SetPlayer(PlayerMapMovement player) { this.player = player; }

        public WaypointNodes[] paths;
        //public RandomEvent[] events;
        public E_Scenes loadScene;

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

        public void Arrived()
        {
            //load scene
            player.SetLevel(loadScene);

            if (loadScene != E_Scenes.Null)
            {
                //load tooltip instead
                TooltipManager.instance.ShowTooltip(true, "Enter " + loadScene.ToString(), "Press 'F' to enter level");
            }
            else
            {
                TooltipManager.instance.ShowTooltip(false, "", "");
                //load random event
            }
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
