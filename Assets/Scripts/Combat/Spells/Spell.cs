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
        #region Setup

        #region Basic Info

        [Header("Basic Info")]
        public string spellName;
        [TextArea(3, 10)]
        public string flavourText; // Flavour text
        [TextArea(3, 10)]
        public string spellDescription; // Basic desciption of spell effect
        public Sprite cardImage;

        #endregion

        #region Timeline Colour

        [Header("Timeline Colour")]
        public bool overrideColor;
        public Color timelineColor = new Color(0, 0, 0, 255);

        #endregion

        #region Spell Logic

        [Header("Spell Logic")]
        public float speed;
        public int arcanaCost;

        public CombatHelperFunctions.SpellModule[] spellModules;

        #endregion

        #endregion

        #region Spellcasting

        public void CastSpell(Character target, Character caster)
        {
            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = HelperFunctions.CombineLists(targetTeamManager.team, casterTeamManager.team);

                for (int i = 0; i < module.hitCount; i++)
                {
                    //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                    switch (module.target)
                    {
                        case E_SpellTargetType.Caster:
                            AffectSelf(caster, module);
                            break;
                        case E_SpellTargetType.Target:
                            AffectTarget(caster, target, module);
                            break;
                        case E_SpellTargetType.Chain:
                            foreach (Character character in targetTeamManager.team)
                            {
                                AffectTarget(caster, character, module);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            foreach (Character character in targetTeamManager.team)
                            {
                                AffectTarget(caster, character, module);
                            }
                            break;
                        case E_SpellTargetType.RandomTargetTeam:
                            AffectTarget(caster, targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], module);
                            break;
                        case E_SpellTargetType.RandomAll:
                            AffectTarget(caster, allCharacters[Random.Range(0, allCharacters.Count)], module);
                            break;
                        case E_SpellTargetType.All:
                            foreach (Character character in allCharacters)
                            {
                                AffectTarget(caster, character, module);
                            }
                            break;
                    }
                }
            }
        }

        #region Affect Characters

        void AffectSelf(Character caster, CombatHelperFunctions.SpellModule spell)
        {
            if (caster == null)
            {
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                caster.GetHealth().ChangeHealth(spell.effectType, spell.value, caster);

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    if (CombatHelperFunctions.ApplyChance(spell.statuses[i].chance))
                    {
                        //apply status i on target
                        spell.statuses[i].status.Apply(caster, spell.statuses[i].duration);
                    }
                }
            }
        }

        void AffectTarget(Character caster, Character target, CombatHelperFunctions.SpellModule spell)
        {
            if (target != null)
            {
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                E_DamageTypes realEffectType = CombatHelperFunctions.ReplaceRandom(spell.effectType);
                target.GetHealth().ChangeHealth(realEffectType, spell.value, caster);

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    if (CombatHelperFunctions.ApplyChance(spell.statuses[i].chance))
                    {
                        //apply status i on target
                        spell.statuses[i].status.Apply(target, spell.statuses[i].duration);
                    }
                }

                if (target.GetHealth().GetHealthPercentage() < spell.executeThreshold)
                {
                    //Debug.Log("Kill " + target.characterName + " with " + name + " at: " + (target.GetHealth().GetHealthPercentage()));
                    target.GetHealth().ChangeHealth(E_DamageTypes.Perforation, 9999999, caster);
                }

                //Sound effects here
            }
        }

        #endregion

        #endregion
    }
}