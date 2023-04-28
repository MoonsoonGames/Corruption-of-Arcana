using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Necropanda.SaveSystem.Serializables;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com & @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    #region Interfaces

    public interface IInteractable
    {
        void SetID(string newID);

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
        public static E_Scenes StringToSceneEnum(string sceneString)
        {
            //Debug.Log(sceneString);

            E_Scenes scene = (E_Scenes)System.Enum.Parse(typeof(E_Scenes), sceneString);
            return scene;
        }

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

        public static List<T> ArrayToList<T>(T[] array)
        {
            List<T> list = new List<T>();

            foreach (var item in array)
            {
                list.Add(item);
            }

            return list;
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

        public static bool AlmostEqualVector3(Vector3 a, Vector3 b, float threshold, Vector3 ignoreAxis)
        {
            bool x = AlmostEqualFloat(a.x, b.x, threshold) || ignoreAxis.x == 1;
            bool y = AlmostEqualFloat(a.y, b.y, threshold) || ignoreAxis.y == 1;
            bool z = AlmostEqualFloat(a.z, b.z, threshold) || ignoreAxis.z == 1;
            return x && y && z;
        }
        
        public static Vector3 ConvertSerializable(SerializableVector3 serializableVector3)
        {
            return new Vector3(serializableVector3.x, serializableVector3.y, serializableVector3.z);
        }

        public static Vector3 LerpVector3(Vector3 a, Vector3 b, float p)
        {
            float x = Mathf.Lerp(a.x, b.x, p);
            float y = Mathf.Lerp(a.y, b.y, p);
            float z = Mathf.Lerp(a.z, b.z, p);
            return new Vector3(x, y, z);
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
            public float valueScalingDamageTaken;
            public float valueScalingShieldCost;
            public int valueScalingPerDiscard;
            public int valueScalingPerStatus;
            public CharacterStats summon;
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

        [System.Serializable]
        public struct ProjectilePoint
        {
            public E_ProjectilePoints point;
            public Transform transform;
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

        [System.Serializable]
        public struct StatusUtility
        {
            public E_Statuses status;
            public float utility;
        }

        #endregion

        #endregion

        #region Status Effects

        public static bool ApplyEffect(Character target, StatusStruct status)
        {
            bool apply = false;

            if (target.GetHealth().GetShield() <= 0 || status.applyOverShield)
            {
                //Debug.Log("Apply success");
                apply = true;
            }
            else
            {

                //Debug.Log("Apply failed");
            }

            return apply;
        }

        [System.Serializable]
        public struct StatusStruct
        {
            public StatusEffects status;
            public int duration;
            public bool applyOverShield;
            public bool remove;
            public int valueSuccess;
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
            public Object effect;
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
            public bool replaceWithText;
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
            public bool applyOverShield;
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

    public static class QuestHelperFuncions
    {
        [System.Serializable]
        public struct QuestInstance
        {
            public Quest quest;
            public bool invert;
            public E_QuestStates[] states;

            public bool Available()
            {
                bool available = invert;

                foreach (E_QuestStates state in states)
                {
                    if (quest.state == state)
                    {
                        available = !invert;
                    }
                }

                return available;
            }
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
                foreach (var sound in sounds)
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