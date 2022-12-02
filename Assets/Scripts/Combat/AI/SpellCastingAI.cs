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
        public float spawnAllyUtility;

        /// <summary>
        /// Determines which spell the AI will cast
        /// </summary>
        /// <param name="spellList">The list of available spells</param>
        /// <param name="self">The caster of the spell</param>
        /// <param name="allyTeam">The allied team of the caster</param>
        /// <param name="enemyTeam">The opposing team of the caster</param>
        /// <returns></returns>
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

        /// <summary>
        /// Loops through all available spell to determine the best spell to cast based on its utility
        /// </summary>
        /// <param name="spellList">The list of available spells</param>
        /// <param name="self">The caster of the spell</param>
        /// <param name="allyTeam">The allied team of the caster</param>
        /// <param name="enemyTeam">The opposing team of the caster</param>
        /// <returns></returns>
        CombatHelperFunctions.SpellUtility UtilityCalculation(List<CombatHelperFunctions.AISpell> spellList, Character self, List<Character> allyTeam, List<Character> enemyTeam)
        {
            CombatHelperFunctions.SpellUtility bestSpell = new CombatHelperFunctions.SpellUtility();
            bestSpell.utility = -5;
            List<Character> allTargets = new List<Character>();
            allTargets = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

            foreach (Character target in allTargets)
            {
                foreach (CombatHelperFunctions.AISpell spell in spellList)
                {
                    if (self.charm == false && CanCastSpell(spell, self, target, allyTeam, enemyTeam))
                    {
                        float utility = 0;

                        utility = SpellUtility(spell, self, target, allyTeam, enemyTeam);

                        if (utility > bestSpell.utility)
                        {
                            bestSpell.spell = spell;
                            bestSpell.target = target;
                            bestSpell.utility = utility;
                        }
                    }
                    else if (self.charm && CanCastSpell(spell, self, target, enemyTeam, allyTeam))
                    {
                        float utility = 0;

                        utility = SpellUtility(spell, self, target, enemyTeam, allyTeam);

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

        /// <summary>
        /// Determines the spell utility of an individual spell
        /// </summary>
        /// <param name="spell">The spell being cast</param>
        /// <param name="self">The caster of the spell</param>
        /// <param name="target">The target of the spell</param>
        /// <param name="allyTeam">The allied team of the caster</param>
        /// <param name="enemyTeam">The opposing team of the caster</param>
        /// <returns>A float value of the effectiveness of the spell</returns>
        float SpellUtility(CombatHelperFunctions.AISpell spell, Character self, Character target, List<Character> allyTeam, List<Character> enemyTeam)
        {
            float spellUtility = 0;

            //Loop through all of the spell modules
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
                else if (self.banish == false && target.banish == false && target != self)
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
                    else if (spell.targetEnemies && enemyTeam.Contains(target))
                    {
                        int targetHealth = target.GetHealth().GetHealth();
                        int targetMaxHealth = target.GetHealth().GetMaxHealth();
                        float targetUtility = targetMaxHealth - targetHealth;

                        moduleUtility += module.value * damageUtility;
                    }

                    spellUtility += moduleUtility;
                }
            }

            if (spell.spell.spawnEnemies != null)
            {
                //Debug.Log("Spell spawns allies, increase priority");

                float spawnUtility = spawnAllyUtility * spell.spell.spawnEnemies.Length;
                spellUtility += spawnUtility;
            }

            //Debug.Log(self.stats.characterName + " casting " + spell.spell.spellName + " on " + target.stats.characterName + " has utility: " + spellUtility);

            return spellUtility;
        }

        /// <summary>
        /// Determines if the spell can be cast on the character
        /// </summary>
        /// <param name="spell">The spell being cast</param>
        /// <param name="self">The caster of the spell</param>
        /// <param name="target">The target of the spell</param>
        /// <param name="allyTeam">The allied team of the caster</param>
        /// <param name="enemyTeam">The opposing team of the caster</param>
        /// <returns>True if spell can be cast, false otherwise</returns>
        bool CanCastSpell(CombatHelperFunctions.AISpell spell, Character self, Character target, List<Character> allyTeam, List<Character> enemyTeam)
        {
            if (spell.lastUsed < spell.timeCooldown)
            {
                return false;
            }

            if (self.silence && spell.timeCooldown > 0)
            {
                return false;
            }

            if (spell.targetSelf && target == self)
            {
                return true;
            }

            if (spell.targetAllies && target != self && allyTeam.Contains(target))
            {
                return true;
            }

            if (spell.targetEnemies && target != self && enemyTeam.Contains(target))
            {
                return true;
            }

            return false;
        }
    }
}