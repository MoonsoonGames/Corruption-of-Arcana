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

        public void StartTurn()
        {
            foreach (Enemy enemy in team)
            {
                //In future, determine target depending on spell so it can cast support spells on allies/self
                SpellInstance newSpellInstance = new SpellInstance();
                newSpellInstance.SetSpellInstance(enemy.PrepareSpell(), player, enemy);

                timeline.AddSpellInstance(newSpellInstance);
            }
        }
    }
}