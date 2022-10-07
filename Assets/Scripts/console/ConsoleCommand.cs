using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base console command class
/// </summary>
namespace necropanda.utils.console.commands
{
    public abstract class ConsoleCommand : ScriptableObject, IConsoleCommand
    {
        [SerializeField] private string commandWord = string.Empty;

        public string CommandWord => commandWord;

        public  abstract bool process(string[] args);
    }
}
