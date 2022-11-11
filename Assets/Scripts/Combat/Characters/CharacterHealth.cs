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
        public bool dying = false;
        protected int maxHealth; public int GetMaxHealth() { return maxHealth; }
        protected int cursedMaxHealth;
        protected int tempMaxHealth;
        protected int health; public int GetHealth() { return health; }
        //Hit sound modifier for health
        protected int shield;
        //Hit sound modifier for shield

        //Damage Resistances
        Dictionary<E_DamageTypes, float> currentDamageResistances;

        public Image healthIcon;
        public TextMeshProUGUI healthText;
        public Color shieldColor;
        public Color healthColor;
        public Color lowHealthColor;
        public float lowHealthThresholdPercentage;
        public GameObject curseOverlay;

        protected virtual void Start()
        {
            character = GetComponent<Character>();
            SetupHealth();
            SetupResistances();
        }

        protected virtual void SetupHealth()
        {
            maxHealth = character.stats.maxHealth;
            tempMaxHealth = maxHealth;
            health = maxHealth;
            cursedMaxHealth = (int)(maxHealth * 0.8);

            CheckCurseHealth();
        }

        protected virtual void SetupResistances()
        {
            currentDamageResistances = new Dictionary<E_DamageTypes, float>();

            for (int i = 0; i < character.stats.baseDamageResistancesModifier.Length; i++)
            {
                currentDamageResistances.Add(character.stats.baseDamageResistancesType[i], character.stats.baseDamageResistancesModifier[i]);
            }
        }

        #endregion

        #region Start Turn

        public void StartTurn()
        {
            //Decay shield
            //Debug.Log("Decay shield: " + shield + " --> " + shield / 2);
            shield = shield / 2;
            CheckCurseHealth();
        }

        #endregion

        #region Health

        public int ChangeHealth(E_DamageTypes type, int value, Character attacker)
        {
            int damageTaken = 0;

            //Resistance check
            int trueValue = (int)(value * CheckResistances(type));

            switch (type)
            {
                case (E_DamageTypes.Healing):
                    health = Mathf.Clamp(health + trueValue, 0, tempMaxHealth);
                    break;
                case (E_DamageTypes.Shield):
                    shield += trueValue;
                    break;
                case (E_DamageTypes.Perforation):
                    health = Mathf.Clamp(health - trueValue, 0, tempMaxHealth);
                    damageTaken = trueValue;
                    break;
                default:
                    int damageOverShield = (int)Mathf.Clamp(trueValue - shield, 0, Mathf.Infinity);
                    shield = (int)Mathf.Clamp(shield - trueValue, 0, Mathf.Infinity);
                    health = Mathf.Clamp(health - damageOverShield, 0, tempMaxHealth);
                    if (attacker != null)
                        Timeline.instance.HitStatuses(character, attacker);
                    damageTaken = trueValue;
                    break;
            }

            if (health <= 0)
            {
                //Debug.Log(health);
                dying = true;
            }

            PlaySound(type, trueValue);
            UpdateHealthUI();
            character.damageTakenThisTurn += damageTaken;
            return trueValue;
        }

        public float GetHealthPercentage() { return (float)health / (float)maxHealth; }

        public float GetHealthPercentageFromDamage(int damage) { return (float)(health - damage) / (float)maxHealth; }

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
                    healthText.text = health.ToString() + "/" + tempMaxHealth.ToString() + " + " + shield.ToString();
                }
            }
            else
            {
                if (healthIcon != null)
                {
                    //Debug.Log((float)((float)health / (float)maxHealth));
                    if ((float)((float)health / (float)tempMaxHealth) < lowHealthThresholdPercentage)
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
                    healthText.text = health.ToString() + "/" + tempMaxHealth.ToString();
                }
            }
        }

        public void CheckCurseHealth()
        {
            if (character.curse)
            {
                tempMaxHealth = cursedMaxHealth;
                health = Mathf.Clamp(health, 0, tempMaxHealth);
                curseOverlay.SetActive(true);
            }
            else
            {
                tempMaxHealth = maxHealth;
                curseOverlay.SetActive(false);
            }

            UpdateHealthUI();
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

        public EventReference defaultSoundEffectHealth;
        public EventReference defaultSoundEffectShield;
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

            if (CheckDamage(type))
            {
                //Play sound based on damage type
                //Modify sound based on whether target has shields or not (put sound modifiers in lines 24 and 26)
                if (shield > 0)
                {
                    //Target took damage to shield, dull sound
                    //Target took direct damage to health, intense sound
                    RuntimeManager.PlayOneShot(defaultSoundEffectShield);
                }
                else
                {
                    //Target took direct damage to health, intense sound
                    RuntimeManager.PlayOneShot(defaultSoundEffectHealth);
                }
            }
        }

        bool CheckDamage(E_DamageTypes type)
        {
            if (type == E_DamageTypes.Healing || type == E_DamageTypes.Shield)
            {
                return false;
            }

            return true;
        }

        public void PlayDeathSound()
        {
            //Play death sound
        }

        #endregion
    }
}