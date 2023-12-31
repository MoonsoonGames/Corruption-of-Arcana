using UnityEngine;
using Necropanda.Utils.Console;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New Gravity Command", menuName = "Utilities/DeveloperConsole/Commands/Gravity Command")]
    /// <summary>
    /// Allows the player to edit and change gravity. If anything but a float is passed in, it will reset the gravity to default.
    /// </summary>
    public class GravityCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();

            // Check to make sure that an arguemnt is passed in, if not return false (do nothing)
            if (args.Length != 1) { return false; }

            // If the argument passed in can't be parsed as false, return false and reset the gravity to default.
            if (!float.TryParse(args[0], out float value))
            {
                DeveloperConsoleBehaviour.OutputMessage = $"invalid argument passed, resetting gravity";
                // get ref, call the function to update the message
                developerConsoleBehaviour.UpdateOutputMessage();

                Physics.gravity = new Vector3(Physics.gravity.x, -9.81f, Physics.gravity.z);
                return false;
            }

            DeveloperConsoleBehaviour.OutputMessage = $"Setting gravity to {value}";
            // get ref, call the function to update the message
            developerConsoleBehaviour.UpdateOutputMessage();
            // If the arg does get parsed as a usable float, set Unitys gravity to that value.
            Physics.gravity = new Vector3(Physics.gravity.x, value, Physics.gravity.z);

            return true;
        }
    }
}

