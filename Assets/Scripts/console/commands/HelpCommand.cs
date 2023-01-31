using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New Help Command", menuName = "Utilities/DeveloperConsole/Commands/Help Command")]
    /// <summary>
    /// Allows the Player to list all the current commands.
    /// </summary>
    public class HelpCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            // Get reference to the dev console behaviour, this lets us access the command list.
            DeveloperConsoleBehaviour behaviour = GameObject.FindGameObjectWithTag("Console").GetComponent<DeveloperConsoleBehaviour>();

            // Log each one and add it to the text var.
            foreach (var command in behaviour.commands)
            {
                string helpText = command.ToString();
                DeveloperConsoleBehaviour.OutputMessage = $"All availabe commands are: \n {command}";
                // get ref, call the function to update the message
                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();
            }

            // Return true when the command is done.
            return true;
        }
    }
}


