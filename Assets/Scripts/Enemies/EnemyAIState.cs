/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.AI
{
    /// <summary>
    /// The current state of the AI. This is used in the switch case.
    /// More can be added, so it is very expandable
    /// </summary>
    public enum AIState
    {
        Nothing,
        Wandering,
        Chasing,
        Patrolling,
        Following
    }
}
