/// <summary>
/// Console Command Interface
/// </summary>
namespace necropanda.utils.console.commands
{
    public interface IConsoleCommand
    {
        string CommandWord {get;}
        bool Process(string[] args);
    }
}