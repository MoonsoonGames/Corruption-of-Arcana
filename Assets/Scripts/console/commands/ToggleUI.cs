using UnityEngine;
using UnityEngine.SceneManagement;

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
            DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();


            if (SceneManager.GetActiveScene().name == "SplashScreen")
            {
                DeveloperConsoleBehaviour.OutputMessage = $"ERROR: NO UI TO TOGGLE IN THIS SCENE.";

                // Update console message
                developerConsoleBehaviour.UpdateOutputMessage();
                return false;
            }

            // Get ref to all of the UI
            Canvas allUI = GameObject.FindGameObjectWithTag("AllUI").GetComponent<Canvas>();
            // Toggle the UI
            allUI.enabled = !allUI.enabled;

            DeveloperConsoleBehaviour.OutputMessage = $"Toggled the UI, state is {allUI.isActiveAndEnabled}";
            // get ref, call the function to update the message

            developerConsoleBehaviour.UpdateOutputMessage();

            return true;
        }
    }
}

