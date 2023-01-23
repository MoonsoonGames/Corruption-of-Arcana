using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New TGM Command", menuName = "Utilities/DeveloperConsole/Commands/TGM Command")]
    /// <summary>
    /// Allows the player to log text to the Unity Console.
    /// </summary>
    public class ToggleGodMode : ConsoleCommand
    {
        public bool GodMode;
        public override bool Process(string[] args)
        {
            GodMode = !GodMode;
            DeveloperConsoleBehaviour.OutputMessage = $"God Mode {GodMode}";
            // get ref, call the function to update the message
            DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
            developerConsoleBehaviour.UpdateOutputMessage();

            return true;
        }
    }
}

