using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace necropanda.utils.console.commands
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
                Debug.Log(helpText);
            }

            // Return true when the command is done.
            return true;
        }
    }
}


