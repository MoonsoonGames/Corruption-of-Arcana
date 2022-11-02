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
        public SpellCastingAI SpellCastingAI;
        List<CombatHelperFunctions.AISpell> aISpells;

        EnemyManager enemyManager;

        protected override void Start()
        {
            base.Start();
            enemyManager = (EnemyManager)teamManager;
            aISpells = stats.aISpells;
        }

        public override CombatHelperFunctions.SpellUtility PrepareSpell()
        {
            return SpellCastingAI.GetSpell(aISpells, enemyManager.team, enemyManager.opposingTeam);
        }

        public override void StartTurn()
        {
            //In future, determine target depending on spell so it can cast support spells on allies/self
            CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
            CombatHelperFunctions.SpellUtility spellUtility = PrepareSpell();
            newSpellInstance.SetSpellInstance(spellUtility.spell.spell, spellUtility.target, this);

            enemyManager.AddSpellInstance(newSpellInstance);
        }
    }
}