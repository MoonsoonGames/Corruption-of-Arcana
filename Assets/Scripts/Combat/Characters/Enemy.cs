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

        public override Spell PrepareSpell()
        {
            return spells[Random.Range(0, spells.Count)];
        }

        public override void StartTurn()
        {
            //In future, determine target depending on spell so it can cast support spells on allies/self
            //CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
            //newSpellInstance.SetSpellInstance(PrepareSpell(), player, this);

            //.AddSpellInstance(newSpellInstance);
        }
    }
}