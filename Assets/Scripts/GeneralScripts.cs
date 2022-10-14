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
    public struct SpellInstance
    {
        public Spell spell;
        public Character caster;
        public Character target;

        public void SetSpellInstance(Spell newSpell, Character newTarget, Character newCaster)
        {
            spell = newSpell;
            target = newTarget;
            caster = newCaster;
        }
    }

    [System.Serializable]
    public struct SpellModule
    {
        public E_SpellTargetType target;
        public E_DamageTypes effectType;
        public int value;
        public int multihitValue;
        public int hitCount;
        public float executeThreshold;
        //Status effects and chances here

        public void SetSpellInstance(E_SpellTargetType newTarget, E_DamageTypes newEffectType, int newValue, int newMultihitValue, int newHitCount, float newExecuteThreshold)
        {
            target = newTarget;
            effectType = newEffectType;
            value = newValue;
            multihitValue = newMultihitValue;
            hitCount = newHitCount;
            executeThreshold = newExecuteThreshold;
        }
    }

    [System.Serializable]
    public struct StatusModule
    {
        public E_StatusTargetType target;
        public E_Statuses status;
        public E_DamageTypes effectType;
        public int value;
        public float resistanceModifier;

        public void SetSpellInstance(E_StatusTargetType newTarget, E_Statuses newStatus, E_DamageTypes newEffectType, int newValue, float newResistanceModifier)
        {
            target = newTarget;
            status = newStatus;
            effectType = newEffectType;
            value = newValue;
            resistanceModifier = newResistanceModifier;
        }
    }

    public interface IInteractable
    {
        void Interacted(GameObject player);
    }
}