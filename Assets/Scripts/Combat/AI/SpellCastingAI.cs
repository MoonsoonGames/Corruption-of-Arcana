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

        public float damageUtility;
        public float controlUtility;
        public float supportSelfUtility;
        public float supportAllyUtility;

        public CombatHelperFunctions.SpellUtility GetSpell(List<CombatHelperFunctions.AISpell> spellList, Character self, List<Character> allyTeam, List<Character> enemyTeam)
        {
            CombatHelperFunctions.SpellUtility spellUtility = new CombatHelperFunctions.SpellUtility();
            List<Character> allTargets = new List<Character>();
            allTargets = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

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
                spellUtility = UtilityCalculation(spellList, self, allyTeam, enemyTeam);
            }

            //Debug.Log(self.stats.characterName + " is planning to cast " + spellUtility.spell.spell.spellName + " on " + spellUtility.target + " with a utility: " + spellUtility.utility);

            return spellUtility;
        }

        CombatHelperFunctions.SpellUtility UtilityCalculation(List<CombatHelperFunctions.AISpell> spellList, Character self, List<Character> allyTeam, List<Character> enemyTeam)
        {
            CombatHelperFunctions.SpellUtility bestSpell = new CombatHelperFunctions.SpellUtility();
            List<Character> allTargets = new List<Character>();
            allTargets = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

            foreach (Character target in allTargets)
            {
                foreach (CombatHelperFunctions.AISpell spell in spellList)
                {
                    if (spell.lastUsed >= spell.timeCooldown)
                    {
                        float utility = SpellUtility(spell, self, target, allyTeam, enemyTeam);

                        if (self.charm)
                        {
                            utility = SpellUtility(spell, self, target, enemyTeam, allyTeam);
                        }

                        if (utility > bestSpell.utility)
                        {
                            bestSpell.spell = spell;
                            bestSpell.target = target;
                            bestSpell.utility = utility;
                        }
                    }
                }
            }

            return bestSpell;
        }

        float SpellUtility(CombatHelperFunctions.AISpell spell, Character self, Character target, List<Character> allyTeam, List<Character> enemyTeam)
        {
            bool canCast = CanCastSpell(spell, self, target, allyTeam, enemyTeam);

            float spellUtility = canCast ?  2 : 0;

            foreach (CombatHelperFunctions.SpellModule module in spell.spell.spellModules)
            {
                float moduleUtility = 0;

                if (spell.targetSelf && target == self)
                {
                    float targetUtility = 0;

                    if (module.effectType == E_DamageTypes.Healing)
                    {
                        int targetHealth = target.GetHealth().GetHealth();
                        int targetMaxHealth = target.GetHealth().GetMaxHealth();
                        targetUtility = targetMaxHealth - targetHealth;
                    }

                    moduleUtility += (module.value + targetUtility) * supportSelfUtility;
                }

                if (self.banish == false && target.banish == false && target != self)
                {
                    if (spell.targetAllies && allyTeam.Contains(target))
                    {
                        float targetUtility = 0;

                        if (module.effectType == E_DamageTypes.Healing)
                        {
                            int targetHealth = target.GetHealth().GetHealth();
                            int targetMaxHealth = target.GetHealth().GetMaxHealth();
                            targetUtility = targetMaxHealth - targetHealth;
                        }

                        moduleUtility += (module.value + targetUtility) * supportAllyUtility;
                    }

                    if (spell.targetEnemies && enemyTeam.Contains(target))
                    {
                        int targetHealth = target.GetHealth().GetHealth();
                        int targetMaxHealth = target.GetHealth().GetMaxHealth();
                        float targetUtility = targetMaxHealth - targetHealth;

                        moduleUtility += module.value * damageUtility;
                    }
                }

                spellUtility += moduleUtility;
            }

            if (spellUtility > 0)
                Debug.Log(self.stats.characterName + " casting " + spell.spell.spellName + " on " + target.stats.characterName + " has utility: " + spellUtility);
            return spellUtility;
        }

        bool CanCastSpell(CombatHelperFunctions.AISpell spell, Character self, Character target, List<Character> allyTeam, List<Character> enemyTeam)
        {
            if (spell.targetSelf && target == self)
            {
                return true;
            }

            if (self.banish == false && target.banish == false && target != self)
            {
                if (spell.targetAllies && allyTeam.Contains(target))
                {
                    return true;
                }

                if (spell.targetEnemies && enemyTeam.Contains(target))
                {
                    return true;
                }
            }

            return false;
        }
    }
}