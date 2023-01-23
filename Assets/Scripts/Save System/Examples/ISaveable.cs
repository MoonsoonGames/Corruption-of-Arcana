/// <summary>
/// Written by @mattordev using Dino Dappers tutorial - https://www.youtube.com/watch?v=f5GvfZfy3yk&t=2s
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem
{
    /// <summary>
    /// Interface, holds the two functions that are used to Save and Load
    /// </summary>
    public interface ISaveable
    {
        // Saves the state. Object is used to keep it serializable
        object CaptureState();
        void RestoreState(object state);
    }
}
