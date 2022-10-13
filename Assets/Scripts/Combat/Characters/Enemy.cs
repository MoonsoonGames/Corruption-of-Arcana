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
    public class Enemy : Character
    {
        public List<Spell> spells = new List<Spell>();

        public Spell PrepareSpell()
        {
            return spells[Random.Range(0, spells.Count)];
        }
    }
}