using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using Necropanda.Utils.Console;
using Necropanda.Utils.Console.Commands;

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

        #region Variables

        Character character;

        #region Health Values

        [HideInInspector]
        public bool dying = false;
        protected int maxHealth; public int GetMaxHealth() { return maxHealth; }
        protected int cursedMaxHealth;
        protected int tempMaxHealth;
        protected int health; public int GetHealth() { return health; }
        protected int shield;

        //Damage Resistances
        Dictionary<E_DamageTypes, float> currentDamageResistances;

        #endregion

        #region UI

        [Header("UI")]
        public SliderValue healthSlider;
        public ShieldUI shieldUI;
        public Color shieldColor;
        public Color healthColor;
        public Color lowHealthColor;
        public float lowHealthThresholdPercentage;
        //public GameObject curseOverlay;
        UShake shake;
        UColorFlash colorFlash; public UColorFlash GetColorFlash() { return colorFlash; }
        public UColorFlash screenFlash;

        #endregion

        #endregion

        protected virtual void Start()
        {
            character = GetComponent<Character>();
            SetupHealth();
            SetupResistances();
            shake = GetComponentInChildren<UShake>();
            colorFlash = GetComponentInChildren<UColorFlash>();
        }

        /// <summary>
        /// Sets up the max health, cursed health and temp max health from the character stats
        /// </summary>
        protected virtual void SetupHealth()
        {
            maxHealth = character.stats.maxHealth;
            tempMaxHealth = maxHealth;
            health = maxHealth;
            cursedMaxHealth = (int)(maxHealth * 0.8);

            healthSlider.Setup(maxHealth);
            healthSlider.SetSliderValue(health);
            shieldUI.Setup(shield);

            CheckCurseHealth();
        }

        /// <summary>
        /// Sets up the current resistances from the base resistance values
        /// </summary>
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

        #region Health Calculations

        /// <summary>
        /// Apply an effect to the character that changes their health
        /// </summary>
        /// <param name="type">The effect type affecting the character</param>
        /// <param name="value">The value of the damage affecting the target</param>
        /// <param name="attacker">The attacker dealing the damage</param>
        /// <returns>The true value (affected by resistances and shield) of the effect</returns>
        public int ChangeHealth(E_DamageTypes type, int value, Character attacker)
        {
            #region Damage Calculations

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
                    ScreenFlash(type);
                    break;
                default:
                    int damageOverShield = (int)Mathf.Clamp(trueValue - shield, 0, Mathf.Infinity);
                    shield = (int)Mathf.Clamp(shield - trueValue, 0, Mathf.Infinity);
                    health = Mathf.Clamp(health - damageOverShield, 0, tempMaxHealth);
                    if (attacker != null)
                        Timeline.instance.HitStatuses(character, attacker);
                    damageTaken = trueValue;
                    if (damageOverShield > 0)
                        ScreenFlash(type);
                    break;
            }

            character.damageTakenThisTurn += damageTaken;

            #endregion

            #region Effects

            //FX
            PlaySound(type, trueValue);
            UpdateHealthUI();

            if (health <= 0)
            {
                Kill();
            }
            else
            {
                //Visual effects that rely on the character being alive
                ShakeCharacter(damageTaken);
                ColorFlash(type);
            }

            #endregion

            return trueValue;
        }

        /// <summary>
        /// Calculates the percentage of health the character has
        /// </summary>
        /// <returns>Character's health percentage</returns>
        public float GetHealthPercentage() { return (float)health / (float)maxHealth; }

        /// <summary>
        /// Calculates the percentage of health the character has based on how much damage they will take
        /// </summary>
        /// <param name="damage">The damage the target will take to thier health</param>
        /// <returns>Character's health percentage affected by the damage</returns>
        public float GetHealthPercentageFromDamage(int damage) { return (float)(health - damage) / (float)maxHealth; }

        #endregion

        #region UI

        /// <summary>
        /// Updates the UI for the health text and shield overlay
        /// </summary>
        void UpdateHealthUI()
        {
            if (healthSlider == null)
                return;

            healthSlider.SetSliderValue(health);
            shieldUI.SetShield(shield);

            if (shield > 0)
            {
                //Shield overlay
                healthSlider.standardFill.color = shieldColor;

                //Set shield value
                //healthText.text = health.ToString() + "/" + tempMaxHealth.ToString() + " + " + shield.ToString();
            }
            else
            {
                //Health overlay
                if ((float)((float)health / (float)tempMaxHealth) < lowHealthThresholdPercentage)
                {
                    healthSlider.standardFill.color = lowHealthColor;
                }
                else
                {
                    healthSlider.standardFill.color = healthColor;
                }

                //Set health value
                //healthText.text = health.ToString() + "/" + tempMaxHealth.ToString();
            }
        }

        /// <summary>
        /// Updates the UI for the curse overlay
        /// </summary>
        public void CheckCurseHealth()
        {
            if (character == null)
                return;

            if (character.curse)
            {
                //Activate the overlay, set the temp max health to the curse value and clamp the current health value to the new max
                tempMaxHealth = cursedMaxHealth;
                health = Mathf.Clamp(health, 0, tempMaxHealth);
                //curseOverlay.SetActive(true);
            }
            else
            {
                //Resets the overlay and max health
                tempMaxHealth = maxHealth;
                //curseOverlay.SetActive(false);
            }

            UpdateHealthUI();
        }

        #endregion

        #region Death

        public GameObject[] disableOnKill;

        void Kill()
        {
            // Get ref to the dev console
            DeveloperConsoleBehaviour behaviour = GameObject.FindGameObjectWithTag("Console").GetComponent<DeveloperConsoleBehaviour>();

            // Need to find a better way to do this
            ToggleGodMode tgm = (ToggleGodMode)behaviour.commands[6];

            if (tgm.GodMode == true)
            {
                return;
            }
            dying = true;
            KillFX();
            ActivateArt(false);
        }

        public void ActivateArt(bool activate)
        {
            foreach (var item in disableOnKill)
            {
                //Disable all art assets
                item.SetActive(activate);
            }
        }

        #endregion

        #endregion

        #region Resistances

        /// <summary>
        /// Change the resistance values to a new value
        /// </summary>
        /// <param name="damageType">The damage type being changed</param>
        /// <param name="valueModifier">The modifier to add or subtract from the current resistance value</param>
        /// <returns>True if the resistance was already contained and modified. False if the resistance was not contained and added.</returns>
        public bool ModifyResistanceModifier(E_DamageTypes damageType, float valueModifier)
        {
            if (currentDamageResistances.ContainsKey(damageType))
            {
                currentDamageResistances[damageType] += valueModifier;
                return true;
            }
            else
            {
                currentDamageResistances.Add(damageType, 1f + valueModifier);
                return false;
            }
        }

        /// <summary>
        /// Checks the resistance multiplier for an input damage type
        /// </summary>
        /// <param name="type">The damage type being checked</param>
        /// <returns>Returns the multiplier for the damage type</returns>
        public float CheckResistances(E_DamageTypes type)
        {
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

        #region Feedback

        #region Sound Effects

        [Header("Sound Effects")]
        public EventReference defaultSoundEffectHealth;
        public EventReference defaultSoundEffectShield;
        public Vector2Int damageScaling;
        public SoundEffects.SoundModule[] soundEffects;
        //FMOD.Studio.EventInstance fmodInstance;

        public void PlaySound(E_DamageTypes type, int value)
        {
            //Play sound from the damage type
            foreach (var item in soundEffects)
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

        #region Visual Effects

        void ShakeCharacter(int damage)
        {
            if (shake != null && damage > 0)
            {
                float intensity = HelperFunctions.Remap(damage, 0, 20, 7, 14);
                shake.CharacterShake(shake.baseDuration, intensity, 3);
            }
        }

        void ColorFlash(E_DamageTypes type)
        {
            if (colorFlash != null)
            {
                colorFlash.Flash(type);
            }

            //VFXManager.instance.ScreenShake();
        }

        void ScreenFlash(E_DamageTypes type)
        {
            if (screenFlash != null)
            {
                screenFlash.Flash(type);
            }
        }

        public Object killFX;

        void KillFX()
        {
            if (killFX == null) { return; }

            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            spawnPos.z = VFXManager.instance.transform.position.z;
            VFXManager.instance.SpawnImpact(killFX, spawnPos);
            VFXManager.instance.ScreenShake();
        }

        #endregion

        #endregion
    }
}