using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com / @mattordev (remap func)
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    #region Interfaces

    public interface IInteractable
    {
        void Interacted(GameObject player);
    }

    #endregion

    #region General Helper Functions

    public static class HelperFunctions
    {
        /// <summary>
        /// Combines 2 lists together, even if they are of different types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns>The combined list</returns>
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

        /// <summary>
        /// Randomly sorts a list
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int RandomSort<T>(T s1, T s2)
        {
            float random1 = Random.Range(0f, 1f);
            float random2 = Random.Range(0f, 1f);

            return random1.CompareTo(random2);
        }

        //remap function
        /// <summary>
        /// Remaps the passed in variables based on min and max. Written by @mattordev
        /// </summary>
        /// <param name="inputValue">The value to be remapped</param>
        /// <param name="fromMin">The raw minimum value, e.g. sensor output min</param>
        /// <param name="fromMax">The raw maximum value, e.g. sensor output max</param>
        /// <param name="toMin">what you want to remap the raw minimum value to. e.g. "-1"</param>
        /// <param name="toMax">what you want to remap the raw maximum value to. e.g. "1"</param>
        /// <returns>The remapped, calculated value.</returns>
        float Remap(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            float i = (((inputValue - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin);
            i = Mathf.Clamp(i, toMin, toMax);
            return i;
        }
    }

    public static class CombatHelperFunctions
    {
        // Struct Issues: https://forum.unity.com/threads/serializable-class-struct-array-incorrectly-displayed-in-inspector.1286267/
        // Current version is 2021.3.4f1 || Struct issue fixed in 2021.3.5f1
        // Could try to fix with custom inspectors https://www.youtube.com/watch?v=RInUu1_8aGw

        #region Spells

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
            public int hitCount;
            public float executeThreshold;
            public StatusStruct[] statuses;

            public void SetSpellInstance(E_SpellTargetType newTarget, E_DamageTypes newEffectType, int newValue, int newHitCount, float newExecuteThreshold, StatusStruct[] newStatusStructs)
            {
                target = newTarget;
                effectType = newEffectType;
                value = newValue;
                hitCount = newHitCount;
                executeThreshold = newExecuteThreshold;
                statuses = newStatusStructs;
            }
        }

        #endregion

        #region Status Effects

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

        [System.Serializable]
        public struct StatusStruct
        {
            public StatusEffects status;
            public int duration;
            public float chance;
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

        #endregion

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

    public static class SoundEffects
    {
        [System.Serializable]
        public struct SoundModule
        {
            public E_DamageTypes effectType;
            public Sound[] sounds; //Start with lowest intensity first

            public EventReference GetSound(float intensity)
            {
                foreach(var sound in sounds)
                {
                    if (sound.intensityThreshold > intensity)
                    {
                        return sound.sound;
                    }
                }
                return new EventReference();
            }
        }

        [System.Serializable]
        public struct Sound
        {
            public EventReference sound;
            public float intensityThreshold;
        }
    }

    #endregion
}