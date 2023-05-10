using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <Jack Drage>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "New Curio")]
    public class Curios_Object : ScriptableObject
    {
        public string Name;
        public Sprite Artwork;
        public string Description;
        public bool isCollected;
    }
}
