using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New Log Command", menuName = "Utilities/DeveloperConsole/Commands/Log Command")]
    /// <summary>
    /// Allows the player to log text to the Unity Console.
    /// </summary>
    public class LogCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            // Get the text to log based on the args passed in.
            string logText = string.Join(" ", args);

            // Log the passed in text.
            Debug.Log(logText);
            DeveloperConsoleBehaviour.OutputMessage = $"Logged: {logText}";
            // get ref, call the function to update the message
            DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
            developerConsoleBehaviour.UpdateOutputMessage();

            return true;
        }
    }
}

