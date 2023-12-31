using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Utils.Console.Commands;
using Necropanda.Utils.Debugger;

/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console
{
    public class DeveloperConsole
    {
        // Readonly variables that aren't to change.
        private readonly string prefix;
        public readonly Dictionary<string, IConsoleCommand> commands;

        // Set up the prefix and commands
        public DeveloperConsole(string prefix, IConsoleCommand[] commands)
        {
            this.prefix = prefix;
            this.commands = new Dictionary<string, IConsoleCommand>();

            foreach (var command in commands)
            {
                this.commands.Add(command.CommandWord, command);
            }
        }

        /// <summary>
        /// This processes the command, it makes sure that passed in command is ready for further use by stripping the prefix and splitting the args.
        /// 
        /// In simple terms, it formats the command for futher use.
        /// </summary>
        /// <param name="inputValue"></param>
        public void ProcessCommand(string inputValue)
        {
            // If the passed in value doesn't start with the prefix, do nothing.
            if (!inputValue.StartsWith(prefix)) { return; }

            // Remove the prefix
            inputValue = inputValue.Remove(0, prefix.Length);

            // Split up the input at each space. Creates an array.
            string[] inputSplit = inputValue.Split(' ');

            // Set the first word to ALWAYS be the command word.
            string commandInput = inputSplit[0];
            // Skip the first element, this gives us the arguments.
            string[] args = inputSplit.Skip(1).ToArray();

            // Process the rest and call the command.
            ProcessCommand(commandInput, args);
        }

        /// <summary>
        /// This calls the commands, by iterating over the command list untill it finds a match.
        /// </summary>
        /// <param name="commandInput">Command word itself.</param>
        /// <param name="args">Arguemnts that will be used.</param>
        public void ProcessCommand(string commandInput, string[] args)
        {
            if (!commands.ContainsKey(commandInput))
            {
                Debug.LogWarning($"No Command found with \"{commandInput}\" phrase!");
                return;
            }

            commands[commandInput].Process(args);
        }
    }
}
