using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "New Achivement")]
    public class Achievement_Object : ScriptableObject
    {
        public new string name;
        public string description;

        public int count;
        public int total;

        public int stepCompleted;
    }
}
