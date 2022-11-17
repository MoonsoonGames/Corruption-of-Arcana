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
        #region Setup

        [Header("Basic Info")]
        public string effectName;
        [TextArea(3, 10)]
        public string effectDescription; // Basic desciption of spell effect
        public Object applyEffect;
        public Object effect;

        public CombatHelperFunctions.StatusModule[] effectModules;

        #endregion

        #region Applying and Removing

        public void Apply(Character target, int duration)
        {
            //Apply status effect on target, add to character list
            CombatHelperFunctions.StatusInstance instance = new CombatHelperFunctions.StatusInstance();
            instance.SetStatusInstance(this, target, duration);
            bool applied = Timeline.instance.AddStatusInstance(instance);

            if (applied)
            {
                VFXManager.instance.SpawnImpact(effect, target.transform.position);

                foreach (CombatHelperFunctions.StatusModule module in effectModules)
                {
                    switch (module.target)
                    {
                        case E_StatusTargetType.Self:
                            ModifyStats(true, target, module.effectType, module.statModifier);
                            TurnModifiers(true, target, module.status);
                            break;
                        case E_StatusTargetType.Team:
                            TeamManager targetTeamManager = target.GetManager();
                            foreach (Character character in targetTeamManager.team)
                            {
                                ModifyStats(true, character, module.effectType, module.statModifier);
                                TurnModifiers(true, character, module.status);
                            }
                            break;
                        case E_StatusTargetType.OpponentTeam:
                            TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                            foreach (Character character in opponentTeamManager.team)
                            {
                                ModifyStats(true, character, module.effectType, module.statModifier);
                                TurnModifiers(true, character, module.status);
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
            CombatHelperFunctions.StatusInstance instance = new CombatHelperFunctions.StatusInstance();
            instance.SetStatusInstance(this, target, 0);
            Timeline.instance.RemoveStatusInstance(instance);

            foreach (CombatHelperFunctions.StatusModule module in effectModules)
            {
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        ModifyStats(false, target, module.effectType, module.statModifier);
                        TurnModifiers(false, target, module.status);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            ModifyStats(false, character, module.effectType, module.statModifier);
                            TurnModifiers(false, character, module.status);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            ModifyStats(false, character, module.effectType, module.statModifier);
                            TurnModifiers(false, character, module.status);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        #endregion

        #region While Active

        #region Resistances and Stats

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
                Debug.Log("Reverse stat adjustment");
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

        #endregion

        #region Turn Modifiers

        public void ActivateTurnModifiers(Character target)
        {
            //Apply effects when timeline ends
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        TurnModifiers(true, target, module.status);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            TurnModifiers(true, character, module.status);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            TurnModifiers(true, character, module.status);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        void TurnModifiers(bool apply, Character target, E_Statuses modifier)
        {
            target.ApplyStatus(apply, modifier);
        }

        #endregion

        #region Health Adjustments

        public void ActivateEffect(Character target)
        {
            //Apply effects when timeline ends
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
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

        void AffectTarget(Character target, E_DamageTypes effectType, int value)
        {
            if (target != null)
            {
                VFXManager.instance.SpawnImpact(effect, target.transform.position);

                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                E_DamageTypes realEffectType = CombatHelperFunctions.ReplaceRandomDamageType(effectType);
                target.GetHealth().ChangeHealth(realEffectType, value, null);

                if (target.GetHealth().GetHealth() < 1)
                {
                    target.CheckOverlay();
                }

                //Sound effects here
            }
        }

        public void HitEffect(Character target, Character attacker)
        {
            //Apply effects when timeline ends
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
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

        #endregion

        #endregion
    }
}