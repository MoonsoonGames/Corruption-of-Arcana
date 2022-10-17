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
    public class EnemyManager : TeamManager
    {
        public Character player;

        public override void StartTurn()
        {
            base.StartTurn();
            foreach (Character character in team)
            {
                //In future, determine target depending on spell so it can cast support spells on allies/self
                GeneralCombat.SpellInstance newSpellInstance = new GeneralCombat.SpellInstance();
                newSpellInstance.SetSpellInstance(character.PrepareSpell(), player, character);

                timeline.AddSpellInstance(newSpellInstance);
            }
        }
    }
}