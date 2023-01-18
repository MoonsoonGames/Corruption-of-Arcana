using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New Toggle UI Command", menuName = "Utilities/DeveloperConsole/Commands/Toggle UI Command")]
    /// <summary>
    /// Allows the player to change the scene to whatever name or build index is passed in.
    /// </summary>
    public class ToggleUI : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            // not needed
            // // Check to make sure that an arguemnt is passed in, if not return false (do nothing)
            // if (args.Length != 1) { return false; }



            // Get ref to all of the UI
            Canvas allUI = GameObject.FindGameObjectWithTag("AllUI").GetComponent<Canvas>();

            // Toggle the UI
            allUI.enabled = !allUI.enabled;


            return true;
        }
    }
}

