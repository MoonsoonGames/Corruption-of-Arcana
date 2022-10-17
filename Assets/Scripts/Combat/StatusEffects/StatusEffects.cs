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
    [CreateAssetMenu(fileName = "NewStatusEffects", menuName = "Combat/Status Effects", order = 1)]
    public class StatusEffects : ScriptableObject
    {
        [Header("Basic Info")]
        public string effectName;
        [TextArea(3, 10)]
        public string effectDescription; // Basic desciption of spell effect

        public GeneralCombat.StatusModule[] effectModules;

        public void Apply(Character target)
        {
            //Apply status effect on target, add to character list
            GeneralCombat.StatusInstance instance = new GeneralCombat.StatusInstance();
            instance.SetStatusInstance(this, target);
            Timeline.instance.AddStatusInstance(instance);

            foreach(GeneralCombat.StatusModule module in effectModules)
            {
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        ModifyResistances(true, target, module.effectType, module.resistanceModifier);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            ModifyResistances(true, character, module.effectType, module.resistanceModifier);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            ModifyResistances(true, character, module.effectType, module.resistanceModifier);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        public void Remove(Character target)
        {
            //Remove status effect on target, remove from character list
            //Apply status effect on target, add to character list
            GeneralCombat.StatusInstance instance = new GeneralCombat.StatusInstance();
            instance.SetStatusInstance(this, target);
            Timeline.instance.RemoveStatusInstance(instance);

            foreach (GeneralCombat.StatusModule module in effectModules)
            {
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        ModifyResistances(false, target, module.effectType, module.resistanceModifier);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            ModifyResistances(false, character, module.effectType, module.resistanceModifier);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            ModifyResistances(false, character, module.effectType, module.resistanceModifier);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        public void ActivateEffect(Character target, Character attacker)
        {
            //Apply effects when timeline ends
            foreach (GeneralCombat.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        AffectTarget(target, module.effectType, module.value);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            AffectTarget(character, module.effectType, module.value);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            AffectTarget(character, module.effectType, module.value);
                        }
                        break;
                    case E_StatusTargetType.Reflect:
                        if (attacker != null)
                        {
                            AffectTarget(attacker, module.effectType, module.value);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        void ModifyResistances(bool apply, Character target, E_DamageTypes damageType, float value)
        {
            if (apply)
            {
                target.GetHealth().ModifyResistanceModifier(damageType, value);
            }
            else
            {
                target.GetHealth().ModifyResistanceModifier(damageType, -value);
            }
        }

        void AffectTarget(Character target, E_DamageTypes effectType, int value)
        {
            Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            E_DamageTypes realEffectType = GeneralCombat.ReplaceRandom(effectType);
            target.GetHealth().ChangeHealth(realEffectType, value);

            //Sound effects here
        }
    }
}