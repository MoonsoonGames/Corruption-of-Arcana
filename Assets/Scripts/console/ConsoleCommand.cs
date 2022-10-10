using UnityEngine;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commmands
{
    /// <summary>
    /// Base console command class, derrives from a scriptable object and interfaces with IConsoleCommand.
    /// </summary>
    public abstract class ConsoleCommand : ScriptableObject, IConsoleCommand
    {
        [SerializeField] private string commandWord = string.Empty; // Empty string for the command word.

        public string CommandWord => commandWord;   // Set the command word using the getter.

        public  abstract bool Process(string[] args);   // This is the base class, it does nothing on its own.
    }
}
