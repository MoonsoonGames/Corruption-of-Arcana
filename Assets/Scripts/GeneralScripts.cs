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
    public struct SpellInstance
    {
        public Spell spell;
        public Character caster;
        public Character target;

        public void SetSpellInstance(Spell newSpell, Character newTarget, Character newCaster)
        {
            spell = newSpell;
            target = newTarget;
            caster = newCaster;
        }
    }

    public interface IInteractable
    {
        void Interacted(GameObject player);
    }
}