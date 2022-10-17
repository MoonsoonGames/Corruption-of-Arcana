using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public interface IInteractable
    {
        void Interacted(GameObject player);
    }

    public static class GeneralCombat
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
            public StatusEffects[] statusEffect;
            public int[] duration;
            public float[] chance;

            public void SetSpellInstance(E_SpellTargetType newTarget, E_DamageTypes newEffectType, int newValue, int newMultihitValue, int newHitCount, float newExecuteThreshold, StatusEffects[] newStatusEffect, float[] newChance)
            {
                target = newTarget;
                effectType = newEffectType;
                value = newValue;
                multihitValue = newMultihitValue;
                hitCount = newHitCount;
                executeThreshold = newExecuteThreshold;
                statusEffect = newStatusEffect;
                chance = newChance;
            }
        }

        public static bool ApplyChance(float chance)
        {
            bool apply = false;
            float roll = Random.Range(0f, 1f);

            if (roll <= chance)
            {
                Debug.Log("Apply success");
                apply = true;
            }
            else
            {

                Debug.Log("Apply failed");
            }

            return apply;
        }

        public struct StatusInstance
        {
            public StatusEffects status;
            public Character target;
            public int duration;

            public void SetStatusInstance(StatusEffects newStatus, Character newTarget, int newDuration)
            {
                status = newStatus;
                target = newTarget;
                duration = newDuration;
            }
        }

        [System.Serializable]
        public struct StatusModule
        {
            public E_StatusTargetType target;
            public E_Statuses status;
            public E_DamageTypes effectType;
            public int value;
            public float statModifier;

            public void SetSpellInstance(E_StatusTargetType newTarget, E_Statuses newStatus, E_DamageTypes newEffectType, int newValue, float newResistanceModifier)
            {
                target = newTarget;
                status = newStatus;
                effectType = newEffectType;
                value = newValue;
                statModifier = newResistanceModifier;
            }
        }

        public static E_DamageTypes ReplaceRandom(E_DamageTypes effectType)
        {
            if (effectType == E_DamageTypes.Random)
            {
                int randomInt = Random.Range(0, 4);
                switch (randomInt)
                {
                    case 0: return E_DamageTypes.Physical;
                    case 1: return E_DamageTypes.Ember;
                    case 2: return E_DamageTypes.Static;
                    case 3: return E_DamageTypes.Bleak;
                    case 4: return E_DamageTypes.Septic;
                }
            }

            return effectType;
        }
    }

    public static class GeneralScripts
    {
        public static List<T> CombineLists<T>(List<T> list1, List<T> list2)
        {
            List<T> outputList = new List<T>();

            foreach (T item in list1)
            {
                outputList.Add(item);
            }
            foreach (T item in list2)
            {
                outputList.Add(item);
            }

            return outputList;
        }
    }
}