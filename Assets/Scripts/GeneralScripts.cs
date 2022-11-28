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

    public interface ICancelInteractable
    {
        void CancelInteraction(GameObject player);
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

        public static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            List<char> newCharacters = new List<char>();
            newCharacters.Add(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newCharacters.Add(' ');
                newCharacters.Add(text[i]);
            }

            string newString = string.Concat(newCharacters.ToArray());

            return newString;
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
        public static float Remap(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            float i = (((inputValue - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin);
            i = Mathf.Clamp(i, toMin, toMax);
            return i;
        }

        [System.Serializable]
        public struct UtilityModule
        {
            public E_UtilityScripts type;
            public Vector3 axes;
            public float speed;
            public float time;
            [HideInInspector]
            public float currentTime;
            [HideInInspector]
            public bool forward;
        }

        public static bool AlmostEqualFloat(float a, float b, float threshold)
        {
            return Mathf.Abs(a - b) <= threshold;
        }

        public static bool AlmostEqualVector3(Vector3 a, Vector3 b, float threshold)
        {
            bool x = AlmostEqualFloat(a.x, b.x, threshold);
            bool y = AlmostEqualFloat(a.y, b.y, threshold);
            bool z = AlmostEqualFloat(a.z, b.z, threshold);
            return x && y && z;
        }
    }

    public static class CombatHelperFunctions
    {
        // Struct Issues: https://forum.unity.com/threads/serializable-class-struct-array-incorrectly-displayed-in-inspector.1286267/
        // Current version is 2021.3.4f1 || Struct issue fixed in 2021.3.5f1
        // Could try to fix with custom inspectors https://www.youtube.com/watch?v=RInUu1_8aGw

        #region Spells

        #region Basic Info

        [System.Serializable]
        public struct SpellInstance
        {
            public Spell spell;
            public bool empowered;
            public bool weakened;
            public Character caster;
            public Character target;

            public void SetSpellInstance(Spell newSpell, bool newEmpowered, bool newWeakened, Character newTarget, Character newCaster)
            {
                spell = newSpell;
                empowered = newEmpowered;
                weakened = newWeakened;
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
            public float valueScalingDamageTaken;
            public int valueScalingPerDiscard;
            public int valueScalingPerStatus;
            public StatusStruct[] statuses;

            public void SetSpellInstance(E_SpellTargetType newTarget, E_DamageTypes newEffectType, int newValue, int newHitCount, float newExecuteThreshold, int newValueScalingPerDiscard, StatusStruct[] newStatusStructs)
            {
                target = newTarget;
                effectType = newEffectType;
                value = newValue;
                hitCount = newHitCount;
                executeThreshold = newExecuteThreshold;
                valueScalingPerDiscard = newValueScalingPerDiscard;
                statuses = newStatusStructs;
            }
        }

        #endregion

        #region AI

        public struct SpellUtility
        {
            public AISpell spell;
            public Character target;
            public float utility;
        }

        [System.Serializable]
        public struct AISpell
        {
            public Spell spell;
            public bool spawnAsCard;
            public bool targetSelf;
            public bool targetAllies;
            public bool targetEnemies;
            public int timeCooldown;
            //[HideInInspector]
            public int lastUsed;
        }

        #endregion

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

        [System.Serializable]
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

        #region Replacing Random

        public static Character ReplaceRandomTarget(List<Character> characters)
        {
            if (CombatManager.instance.redirectedCharacter != null)
            {
                if (CombatManager.instance.redirectedCharacter.GetHealth().dying == false)
                {
                    Debug.Log("Redirect to target");
                    return CombatManager.instance.redirectedCharacter;
                }
                    
            }

            if (characters.Count > 0)
            {
                List<Character> targets = new List<Character>();

                foreach (Character character in characters)
                {
                    if (character.CanBeTargetted())
                    {
                        targets.Add(character);
                    }
                }

                if (targets.Count > 0)
                {
                    int randomInt = Random.Range(0, targets.Count);

                    return targets[randomInt];
                } 
            }

            return null;
        }

        public static E_DamageTypes ReplaceRandomDamageType(E_DamageTypes effectType)
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

        #endregion

        #region Icon Constructs

        [System.Serializable]
        public struct IconToolTip
        {
            public string title;
            public string replaceText;
            public string description;
            public int imageID;
        }

        public struct SpellIconConstruct
        {
            public int value;
            public E_DamageTypes effectType;
            public int hitCount;

            //Scaling
            public int discardScaling;
            public int cleanseScaling;

            public E_SpellTargetType target; // replace with images later
        }

        public struct StatusIconConstruct
        {
            public StatusEffects effect;
            public float chance;
            public Object effectIcon;
            public int duration;

            public E_SpellTargetType target; // replace with images later
        }

        public struct ExecuteIconConstruct
        {
            public float threshold;
            public E_SpellTargetType target; // replace with images later
        }

        #endregion
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
                    if (sound.intensityThreshold >= intensity)
                    {
                        return sound.sound;
                    }
                }
                return sounds[sounds.Length - 1].sound;
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