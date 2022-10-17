using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewSpell", menuName = "Combat/Spells", order = 0)]
    public class Spell : ScriptableObject
    {
        [Header("Basic Info")]
        public string spellName;
        public bool overrideColor;
        public Color timelineColor = new Color(0, 0, 0, 255);
        [TextArea(3, 10)]
        public string spellDescription; // Basic desciption of spell effect
        public float speed;
        public int arcanaCost;

        public GeneralCombat.SpellModule[] spellModules;

        public void CastSpell(Character target, Character caster)
        {
            foreach (GeneralCombat.SpellModule module in spellModules)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = new List<Character>();

                for (int i = 0; i < module.hitCount; i++)
                {
                    //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                    switch (module.target)
                    {
                        case E_SpellTargetType.Caster:
                            AffectCaster(caster, module);
                            break;
                        case E_SpellTargetType.Target:
                            AffectTarget(target, module.effectType, module.value, module.executeThreshold, module.statusEffect, module.chance);
                            break;
                        case E_SpellTargetType.Chain:
                            foreach (Character character in targetTeamManager.team)
                            {
                                if (character != target)
                                {
                                    AffectTarget(character, module.effectType, module.multihitValue, module.executeThreshold, module.statusEffect, module.chance);
                                }
                                else
                                {
                                    AffectTarget(character, module.effectType, module.value, module.executeThreshold, module.statusEffect, module.chance);
                                }
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            foreach (Character character in targetTeamManager.team)
                            {
                                if (character != target)
                                {
                                    AffectTarget(character, module.effectType, module.multihitValue, module.executeThreshold, module.statusEffect, module.chance);
                                }
                                else
                                {
                                    AffectTarget(character, module.effectType, module.value, module.executeThreshold, module.statusEffect, module.chance);
                                }
                            }
                            break;
                        case E_SpellTargetType.RandomTargetTeam:
                            AffectTarget(targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], module.effectType, module.value, module.executeThreshold, module.statusEffect, module.chance);
                            break;
                        case E_SpellTargetType.RandomAll:
                            allCharacters = GeneralScripts.CombineLists(targetTeamManager.team, casterTeamManager.team);
                            AffectTarget(allCharacters[Random.Range(0, allCharacters.Count)], module.effectType, module.value, module.executeThreshold, module.statusEffect, module.chance);
                            break;
                        case E_SpellTargetType.All:
                            allCharacters = GeneralScripts.CombineLists(targetTeamManager.team, casterTeamManager.team);
                            foreach (Character character in allCharacters)
                            {
                                AffectTarget(character, module.effectType, module.value, module.executeThreshold, module.statusEffect, module.chance);
                            }
                            break;
                    }
                }
            }
        }

        void AffectCaster(Character target, GeneralCombat.SpellModule spell)
        {
            //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            target.GetHealth().ChangeHealth(spell.effectType, spell.value);

            for (int i = 0; i < spell.statusEffect.Length; i++)
            {
                if (GeneralCombat.ApplyChance(spell.chance[i]))
                {
                    //apply status i on target
                    spell.statusEffect[i].Apply(target);
                }
            }
        }

        void AffectTarget(Character target, E_DamageTypes effectType, int value, float executeThreshold, StatusEffects[] statuses, float[] chances)
        {
            //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            E_DamageTypes realEffectType = GeneralCombat.ReplaceRandom(effectType);
            target.GetHealth().ChangeHealth(realEffectType, value);

            for (int i = 0; i < statuses.Length; i++)
            {
                if (GeneralCombat.ApplyChance(chances[i]))
                {
                    //apply status i on target
                    statuses[i].Apply(target);
                }
            }

            if (target.GetHealth().GetHealthPercentage() < executeThreshold)
            {
                Debug.Log("Kill " + target.characterName + " with " + name + " at: " + (target.GetHealth().GetHealthPercentage()));
                target.GetHealth().ChangeHealth(E_DamageTypes.Perforation, 9999999);
            }

            //Sound effects here
        }
    }
}