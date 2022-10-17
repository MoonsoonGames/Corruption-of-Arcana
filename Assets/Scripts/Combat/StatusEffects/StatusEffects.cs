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

        public void Apply(Character target, int duration)
        {
            //Apply status effect on target, add to character list
            GeneralCombat.StatusInstance instance = new GeneralCombat.StatusInstance();
            instance.SetStatusInstance(this, target, duration);
            bool applied = Timeline.instance.AddStatusInstance(instance);

            if (applied)
            {
                foreach (GeneralCombat.StatusModule module in effectModules)
                {
                    switch (module.target)
                    {
                        case E_StatusTargetType.Self:
                            ModifyStats(true, target, module.effectType, module.statModifier);
                            break;
                        case E_StatusTargetType.Team:
                            TeamManager targetTeamManager = target.GetManager();
                            foreach (Character character in targetTeamManager.team)
                            {
                                ModifyStats(true, character, module.effectType, module.statModifier);
                            }
                            break;
                        case E_StatusTargetType.OpponentTeam:
                            TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                            foreach (Character character in opponentTeamManager.team)
                            {
                                ModifyStats(true, character, module.effectType, module.statModifier);
                            }
                            break;
                        default:
                            //do nothing
                            break;
                    }
                }
            }
        }

        public void Remove(Character target)
        {
            //Remove status effect on target, remove from character list
            //Apply status effect on target, add to character list
            GeneralCombat.StatusInstance instance = new GeneralCombat.StatusInstance();
            instance.SetStatusInstance(this, target, 0);
            Timeline.instance.RemoveStatusInstance(instance);

            foreach (GeneralCombat.StatusModule module in effectModules)
            {
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        ModifyStats(false, target, module.effectType, module.statModifier);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            ModifyStats(false, character, module.effectType, module.statModifier);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            ModifyStats(false, character, module.effectType, module.statModifier);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        public void ActivateEffect(Character target)
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
                    default:
                        //do nothing
                        break;
                }
            }
        }

        public void HitEffect(Character target, Character attacker)
        {
            //Apply effects when timeline ends
            foreach (GeneralCombat.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.SelfHit:
                        if (attacker != null)
                        {
                            AffectTarget(target, module.effectType, module.value);
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

        void ModifyStats(bool apply, Character target, E_DamageTypes damageType, float value)
        {
            if (apply)
            {
                target.GetHealth().ModifyResistanceModifier(damageType, value);
                if (damageType == E_DamageTypes.Arcana)
                {
                    ArcanaManager manager = target.GetComponent<ArcanaManager>();
                    if (manager != null)
                    {
                        //Debug.Log("Haste");
                        int arcanaValue = (int)value;
                        manager.AdjustArcanaMax(arcanaValue);
                    }
                }
            }
            else
            {
                target.GetHealth().ModifyResistanceModifier(damageType, -value);
                if (damageType == E_DamageTypes.Arcana)
                {
                    ArcanaManager manager = target.GetComponent<ArcanaManager>();
                    if (manager != null)
                    {
                        int arcanaValue = (int)value;
                        manager.AdjustArcanaMax(-arcanaValue);
                    }
                }
            }
        }

        void AffectTarget(Character target, E_DamageTypes effectType, int value)
        {
            //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            E_DamageTypes realEffectType = GeneralCombat.ReplaceRandom(effectType);
            target.GetHealth().ChangeHealth(realEffectType, value, null);

            //Sound effects here
        }
    }
}