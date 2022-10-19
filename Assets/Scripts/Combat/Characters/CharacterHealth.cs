using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [RequireComponent(typeof(Character))]
    public class CharacterHealth : MonoBehaviour
    {
        #region Setup

        Character character;

        //Health Values
        public int maxHealth;
        protected int health; public int GetHealth() { return health; }
        //Hit sound modifier for health
        protected int shield;
        //Hit sound modifier for shield

        //Damage Resistances
        Dictionary<E_DamageTypes, float> baseDamageResistances;
        public E_DamageTypes[] baseDamageResistancesType;
        public float[] baseDamageResistancesModifier;
        Dictionary<E_DamageTypes, float> currentDamageResistances;

        public Image healthIcon;
        public TextMeshProUGUI healthText;
        public Color shieldColor;
        public Color healthColor;
        public Color lowHealthColor;
        public float lowHealthThresholdPercentage;

        protected virtual void Start()
        {
            character = GetComponent<Character>();
            SetupHealth();
            SetupResistances();
        }

        protected virtual void SetupHealth()
        {
            health = maxHealth;
            UpdateHealthUI();
        }

        protected virtual void SetupResistances()
        {
            baseDamageResistances = new Dictionary<E_DamageTypes, float>();

            for (int i = 0; i < baseDamageResistancesType.Length; i++)
            {
                baseDamageResistances.Add(baseDamageResistancesType[i], baseDamageResistancesModifier[i]);
            }

            currentDamageResistances = new Dictionary<E_DamageTypes, float>();

            foreach (E_DamageTypes type in baseDamageResistances.Keys)
            {
                currentDamageResistances.Add(type, baseDamageResistances[type]);
            }
        }

        #endregion

        #region Health

        public int ChangeHealth(E_DamageTypes type, int value, Character attacker)
        {
            //Resistance check
            int trueValue = (int)(value * CheckResistances(type));

            switch (type)
            {
                case (E_DamageTypes.Healing):
                    health = Mathf.Clamp(health + trueValue, 0, maxHealth);
                    break;
                case (E_DamageTypes.Shield):
                    shield += trueValue;
                    break;
                case (E_DamageTypes.Perforation):
                    health = Mathf.Clamp(health - trueValue, 0, maxHealth);
                    break;
                default:
                    int damageOverShield = (int)Mathf.Clamp(trueValue - shield, 0, Mathf.Infinity);
                    shield = (int)Mathf.Clamp(shield - trueValue, 0, Mathf.Infinity);
                    health = Mathf.Clamp(health - damageOverShield, 0, maxHealth);
                    if (attacker != null)
                        Timeline.instance.HitStatuses(character, attacker);
                    break;
            }

            if (health <= 0)
            {
                //Debug.Log(health);
            }

            PlaySound(type, trueValue);
            UpdateHealthUI();
            return trueValue;
        }

        public float GetHealthPercentage() { return (float)health / (float)maxHealth; }

        void UpdateHealthUI()
        {
            if (shield > 0)
            {
                if (healthIcon != null)
                {
                    healthIcon.color = shieldColor;
                }

                if (healthText != null)
                {
                    healthText.text = health.ToString() + "/" + maxHealth.ToString() + " + " + shield.ToString();
                }
            }
            else
            {
                if (healthIcon != null)
                {
                    //Debug.Log((float)((float)health / (float)maxHealth));
                    if ((float)((float)health / (float)maxHealth) < lowHealthThresholdPercentage)
                    {
                        healthIcon.color = lowHealthColor;
                    }
                    else
                    {
                        healthIcon.color = healthColor;
                    }
                }

                if (healthText != null)
                {
                    healthText.text = health.ToString() + "/" + maxHealth.ToString();
                }
            }
        }

        #endregion

        #region Resistances

        public bool ModifyResistanceModifier(E_DamageTypes damageType, float newValue)
        {
            if (currentDamageResistances.ContainsKey(damageType))
            {
                currentDamageResistances[damageType] += newValue;
                return true;
            }
            else
            {
                currentDamageResistances.Add(damageType, 1f + newValue);
                return false;
            }
        }

        public float CheckResistances(E_DamageTypes type)
        {
            //needs to check resistances
            if (currentDamageResistances.ContainsKey(type))
            {
                return currentDamageResistances[type];
            }
            else
            {
                //No resistance modifier, return 1
                return 1;
            }
        }

        #endregion

        #region Sound Effects

        public EventReference defualtSoundEffect;
        public Vector2Int damageScaling;
        public SoundEffects.SoundModule[] soundEffects;
        //FMOD.Studio.EventInstance fmodInstance;

        public void PlaySound(E_DamageTypes type, int value)
        {
            foreach(var item in soundEffects)
            {
                if (item.effectType == type)
                {
                    float remapValue = HelperFunctions.Remap(value, 0, 5, 0, 1);
                    RuntimeManager.PlayOneShot(item.GetSound(remapValue));
                }
            }

            //Play sound based on damage type
            //Modify sound based on whether target has shields or not (put sound modifiers in lines 24 and 26)
            if (shield > 0)
            {
                //Target took damage to shield, dull sound
            }
            else
            {
                //Target took direct damage to health, intense sound
            }
        }

        public void PlayDeathSound()
        {
            //Play death sound
        }

        #endregion
    }
}