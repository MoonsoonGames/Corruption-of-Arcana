using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Console Command Interface
/// </summary>
namespace necropanda.utils.console.commands
{
    public interface IConsoleCommand
    {
        string CommandWord {get;}
        bool process(string[] args);
    }
}