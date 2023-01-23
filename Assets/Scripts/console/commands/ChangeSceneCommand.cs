using UnityEngine;
using UnityEngine.SceneManagement;
using Necropanda.Utils.Console;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New Change Scene Command", menuName = "Utilities/DeveloperConsole/Commands/Change Scene Command")]
    /// <summary>
    /// Allows the player to change the scene to whatever name or build index is passed in.
    /// </summary>
    public class ChangeSceneCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            // Check to make sure that an arguemnt is passed in, if not return false (do nothing)
            if (args.Length != 1) { return false; }

            var sceneToLoad = args[0];

            // update the output message
            DeveloperConsoleBehaviour.OutputMessage = $"Changing scene to {args[0]}.";
            // get ref, call the function to update the message
            DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
            developerConsoleBehaviour.UpdateOutputMessage();
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            return true;
        }
    }
}

