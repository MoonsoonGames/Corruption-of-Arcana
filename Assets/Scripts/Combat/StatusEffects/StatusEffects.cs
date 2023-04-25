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
        public Object effectIcon;
        public Object applyEffect;
        public Sprite iconSprite;

        public CombatHelperFunctions.StatusModule[] effectModules;

        #endregion

        #region Applying and Removing

        public void Apply(Character target, int duration)
        {
            if (target.GetHealth().dying)
                return;

            //Apply status effect on target, add to character list
            CombatHelperFunctions.StatusInstance instance = new CombatHelperFunctions.StatusInstance();
            instance.SetStatusInstance(this, target, duration);
            bool applied = Timeline.instance.AddStatusInstance(instance);

            if (applied)
            {
                Vector3 spawnPos = target.transform.position;
                spawnPos.z = VFXManager.instance.transform.position.z;
                VFXManager.instance.SpawnImpact(applyEffect, spawnPos);

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
            if (target.GetHealth().dying)
                return;

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
            if (target.GetHealth().dying)
                return;

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
            if (target.GetHealth().dying == false)
                target.ApplyStatus(apply, modifier);
        }

        #endregion

        #region Health Adjustments

        public void ActivateEffect(Character target)
        {
            if (target.GetHealth().dying)
                return;

            //Apply effects when timeline ends
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        AffectTarget(target, module.effectType, module.effect, module.value);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            if (character.GetHealth().dying == false)
                                AffectTarget(character, module.effectType, module.effect, module.value);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            if (character.GetHealth().dying == false)
                                AffectTarget(character, module.effectType, module.effect, module.value);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        void AffectTarget(Character target, E_DamageTypes effectType, Object effect, int value)
        {
            if (target != null)
            {
                if (target.GetHealth().dying)
                    return;

                Vector3 spawnPos = target.transform.position;
                spawnPos.z = VFXManager.instance.transform.position.z;
                VFXManager.instance.SpawnImpact(effect, spawnPos);

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
            if (target.GetHealth().dying)
                return;

            //Apply effects when timeline ends
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.SelfHit:
                        if (attacker != null)
                        {
                            AffectTarget(target, module.effectType, module.effect, module.value);
                        }
                        break;
                    case E_StatusTargetType.Reflect:
                        if (attacker != null)
                        {
                            AffectTarget(attacker, module.effectType, module.effect, module.value);
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

        #region Simulate Status

        /// <summary>
        /// Simulates the effects of the spell on the caster and targets without applying them
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        /// <param name="hand">The hand from which this spell was cast</param>
        public void SimulateStatusValues(Character target)
        {
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(targetTeamManager);

                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        Simulate(target, module);
                        break;
                    case E_StatusTargetType.Team:
                        foreach (Character character in targetTeamManager.team)
                        {
                            Simulate(character, module);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        foreach (Character character in opponentTeamManager.team)
                        {
                            Simulate(character, module);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Simulates the effects of the spell on the caster and targets without applying them
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        /// <param name="hand">The hand from which this spell was cast</param>
        public void SimulateHitValues(Character target, Character attacker)
        {
            foreach (CombatHelperFunctions.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.SelfHit:
                        Simulate(target, module);
                        break;
                    case E_StatusTargetType.Reflect:
                        Simulate(attacker, module);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks the effect of an individual spell module
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        void Simulate(Character target, CombatHelperFunctions.StatusModule status)
        {
            Vector2Int damage = new Vector2Int(0, 0);
            int shield = 0;

            if (target != null && target.GetHealth().dying == false)
            {
                int value = status.value;

                //Debug.Log("Spell cast: " + spellName + " at " + caster.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);

                switch (status.effectType)
                {
                    case E_DamageTypes.Healing:
                        damage.x += value;
                        damage.y += value;
                        break;
                    case E_DamageTypes.Shield:
                        shield += value;
                        break;
                    case E_DamageTypes.Arcana:
                        break;
                    default:
                        damage.x -= value;
                        damage.y -= value;
                        break;
                }
            }

            target.SimulateValues(damage, shield, 0);
        }

        #endregion
    }
}