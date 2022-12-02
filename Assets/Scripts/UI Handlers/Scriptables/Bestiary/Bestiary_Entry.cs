using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <Jack DrageK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "New Entry")]
    public class Bestiary_Entry : ScriptableObject
    {
        public new string Name;
        public Sprite Artwork;
        public string Description;

        public bool Tier1;
        public bool Tier2;
        public bool Tier3;

        public string Location;
        public string Resistances;
        public string Weaknesses;
    }
}
