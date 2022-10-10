/// <summary>
/// Authored & Written by @mrobertscgd
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace necropanda.utils.console.commands
{
    /// <summary>
    /// Interface for console commands.
    /// </summary>
    public interface IConsoleCommand
    {
        // The word to trigger the command, cannot be changes at runtime
        string CommandWord {get;}
        // Returns true if the command was processed, false if not.
        bool Process(string[] args);
    }
}