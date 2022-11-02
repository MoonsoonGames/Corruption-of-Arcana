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
    [CreateAssetMenu(fileName = "NewSpellcastingAI", menuName = "Combat/Spell-casting AI", order = 3)]
    public class SpellCastingAI : ScriptableObject
    {
        public bool random = false;

        public CombatHelperFunctions.SpellUtility GetSpell(List<CombatHelperFunctions.AISpell> spellList, List<Character> allyTeam, List<Character> enemyTeam)
        {
            CombatHelperFunctions.SpellUtility spellUtility = new CombatHelperFunctions.SpellUtility();
            List<Character> allTargets = new List<Character>();
            allTargets = HelperFunctions.CombineLists(allyTeam, enemyTeam);

            if (spellList.Count == 0)
                return spellUtility;

            if (random)
            {
                spellUtility.spell = spellList[Random.Range(0, spellList.Count)];
                spellUtility.target = allTargets[Random.Range(0, allTargets.Count)];
                spellUtility.utility = 0f;
            }
            else
            {
                foreach (CombatHelperFunctions.AISpell spell in spellList)
                {

                }
            }

            return spellUtility;
        }

        CombatHelperFunctions.SpellUtility UtilityCalculation(List<CombatHelperFunctions.AISpell> spellList, List<Character> allyTeam, List<Character> enemyTeam)
        {
            CombatHelperFunctions.SpellUtility bestSpell = new CombatHelperFunctions.SpellUtility();

            foreach (Character character in allyTeam)
            {

            }

            foreach (Character character in enemyTeam)
            {

            }

            return bestSpell;
        }
    }
}
