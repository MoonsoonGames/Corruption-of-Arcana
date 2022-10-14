using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewStatusEffects", menuName = "Combat/Status Effects", order = 1)]
    public class StatusEffects : ScriptableObject
    {
        //Effect info here
        //Priority determines speed

        public void Apply()
        {
            //Apply status effect on target, add to character list
        }

        public void Remove()
        {
            //Remove status effect on target, remove from character list
        }

        public void TimelineEnd()
        {
            //Apply effects when timeline ends
        }
    }
}