using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewSpell", menuName = "Combat/Spells", order = 0)]
    public class Spell : ScriptableObject
    {
        #region Setup

        #region Basic Info

        [Header("Basic Info")]
        public string spellName;
        [TextArea(3, 10)]
        public string flavourText; // Flavour text
        [TextArea(3, 10)]
        public string spellDescription; // Basic desciption of spell effect
        public Sprite cardImage;

        #endregion

        #region Timeline Colour

        [Header("Timeline Colour")]
        public bool overrideColor;
        public Color timelineColor = new Color(0, 0, 0, 255);

        #endregion

        #region Spell Logic

        [Header("Spell Logic")]
        public float speed;
        public int arcanaCost;
        public float multihitDelay = 0.1f;
        public float moduleDelay = 0f;

        public CombatHelperFunctions.SpellModule[] spellModules;

        #endregion

        #region FX

        [Header("FX")]
        //Visual effects for projectile
        public Object projectileObject;

        //Visual effects for hit effect

        //Sound effects for preparing the spell

        #endregion

        #endregion

        #region Spellcasting

        /// <summary>
        /// Checks the delay between casting the spell and the visual projectile(s) hitting the target
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="spawnPosition">The spawn position of the projectile</param>
        /// <returns>Time taken for the last projectile to hit the target</returns>
        public float QuerySpellCastTime(Character target, Character caster, Vector2 spawnPosition)
        {
            float time = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                float hitDelay = module.hitCount * multihitDelay;
                float moduleTime = 0;
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_SpellTargetType.Caster:
                        moduleTime = VFXManager.instance.QueryTime(spawnPosition, caster.transform.position) + hitDelay + time;
                        break;
                    default:
                        moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + hitDelay + time;
                        break;
                }

                time += moduleDelay + moduleTime;
            }

            return time;
        }

        #region Casting Spell

        /// <summary>
        /// Applies the effects of the spell on the caster and targets
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="spawnPosition">The spawn position of the projectile</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        /// <param name="hand">The hand from which this spell was cast</param>
        public void CastSpell(Character target, Character caster, Vector2 spawnPosition, bool empowered, bool weakened, Deck2D hand, int cardsInHand)
        {
            bool discardCards = false;
            bool removeStatuses = false;
            int removedStatusCount = Timeline.instance.StatusCount(target);
            float time = 0;

            TeamManager targetTeamManager = target.GetManager();
            List<Character> allCharacters = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                if (module.discardCards)
                    discardCards = true;

                if (module.removeStatuses)
                    removeStatuses = true;

                for (int i = 0; i < module.hitCount; i++)
                {
                    float hitDelay = i * multihitDelay;
                    float moduleTime = 0;
                    float x = 0;
                    //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                    switch (module.target)
                    {
                        case E_SpellTargetType.Caster:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, caster.transform.position);
                            VFXManager.instance.AffectSelfDelay(this, caster, module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            break;
                        case E_SpellTargetType.Target:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, target, module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay, empowered, weakened);
                            break;
                        case E_SpellTargetType.Chain:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in targetTeamManager.team)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in targetTeamManager.team)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.RandomTargetTeam:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            break;
                        case E_SpellTargetType.RandomAll:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, allCharacters[Random.Range(0, allCharacters.Count)], module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            break;
                        case E_SpellTargetType.All:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in allCharacters)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, cardsInHand, removedStatusCount, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            }
                            break;
                    }

                    time += moduleDelay;
                }
            }

            if (discardCards)
            {
                hand.RemoveAllCards();
            }

            if (removeStatuses)
            {
                Timeline.instance.RemoveStatusesOnCharacter(target);
            }
        }

        #region Affect Characters

        /// <summary>
        /// Applies the effect of an individual spell module on the caster
        /// </summary>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="spell">The individual spell module being applied</param>
        /// <param name="cardsDiscarded">The number of cards discarded</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        public void AffectSelf(Character caster, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, int removedStatusCount, bool empowered, bool weakened)
        {
            if (caster != null)
            {
                //Modifies the value if the spell is empowered or scales with how many cards are discarded
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded) + (spell.valueScalingPerStatus * removedStatusCount);
                value = EmpowerWeakenValue(value, empowered, weakened);

                //Debug.Log("Spell cast: " + spellName + " at " + caster.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                caster.GetHealth().ChangeHealth(spell.effectType, value, caster);

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    if (CombatHelperFunctions.ApplyChance(spell.statuses[i].chance))
                    {
                        //apply status i on target
                        spell.statuses[i].status.Apply(caster, spell.statuses[i].duration);
                    }
                }

                if (caster.GetHealth().GetHealth() < 1)
                {
                    caster.CheckOverlay();
                }
            }
        }

        /// <summary>
        /// Applies the effect of an individual spell module on the caster
        /// </summary>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="target">The character that the module is affecting</param>
        /// <param name="spell">The individual spell module being applied</param>
        /// <param name="cardsDiscarded">The number of cards discarded</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        public void AffectTarget(Character caster, Character target, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, int removedStatusCount, bool empowered, bool weakened)
        {
            if (target != null)
            {
                //Modifies the value if the spell is empowered or scales with how many cards are discarded
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded) + (spell.valueScalingPerStatus * removedStatusCount);
                value = EmpowerWeakenValue(value, empowered, weakened);

                E_DamageTypes realEffectType = CombatHelperFunctions.ReplaceRandom(spell.effectType);
                target.GetHealth().ChangeHealth(realEffectType, value, caster);

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    if (CombatHelperFunctions.ApplyChance(spell.statuses[i].chance))
                    {
                        //apply status i on target
                        spell.statuses[i].status.Apply(target, spell.statuses[i].duration);
                    }
                }

                if (target.GetHealth().GetHealthPercentage() < spell.executeThreshold)
                {
                    //Kill target if they are below the execute threshold
                    target.GetHealth().ChangeHealth(E_DamageTypes.Perforation, 9999999, caster);
                }

                if (target.GetHealth().GetHealth() < 1)
                {
                    target.CheckOverlay();
                }

                //Hit effects here
            }
        }

        /// <summary>
        /// Takes a value and modifies it depending on if the spell is empowered or weakened
        /// </summary>
        /// <param name="originalValue">The original value of the spell</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        /// <returns>The empowered or weakened value</returns>
        int EmpowerWeakenValue(int originalValue, bool empowered, bool weakened)
        {
            int value = originalValue;
            if (empowered && !weakened)
            {
                Debug.Log(spellName + " is empowered " + value + " to " + (int)(value * 1.5f));
                value = (int)(value * 1.5f);
            }
            else if (weakened && !empowered)
            {
                Debug.Log(spellName + " is weakened " + value + " to " + (int)(value * 0.5f));
                value = (int)(value * 0.5f);
            }
            return value;
        }

        #endregion

        #endregion

        #region Simulate Spell

        /// <summary>
        /// Simulates the effects of the spell on the caster and targets without applying them
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        /// <param name="hand">The hand from which this spell was cast</param>
        public void SimulateSpellValues(Character target, Character caster, bool empowered, bool weakened, int cardsInHand)
        {
            int removedStatusCount = Timeline.instance.StatusCount(target);
            Debug.Log("simulated found " + removedStatusCount + "statuses on " + target.stats.characterName);

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

                for (int i = 0; i < module.hitCount; i++)
                {
                    float x = 0;
                    //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                    switch (module.target)
                    {
                        case E_SpellTargetType.Caster:
                            Simulate(caster, caster, module, cardsInHand, removedStatusCount, empowered, weakened);
                            break;
                        case E_SpellTargetType.Target:
                            Simulate(caster, target, module, cardsInHand, removedStatusCount, empowered, weakened);
                            break;
                        case E_SpellTargetType.Chain:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, module, cardsInHand, removedStatusCount, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, module, cardsInHand, removedStatusCount, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.RandomTargetTeam:
                            //Simulate(caster, targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], module, cardsInHand, empowered, weakened);
                            break;
                        case E_SpellTargetType.RandomAll:
                            //Simulate(caster, allCharacters[Random.Range(0, allCharacters.Count)], module, cardsInHand, empowered, weakened);
                            break;
                        case E_SpellTargetType.All:
                            foreach (Character character in allCharacters)
                            {
                                Simulate(caster, character, module, cardsInHand, removedStatusCount, empowered, weakened);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the effect of an individual spell module
        /// </summary>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="spell">The individual spell module being checked</param>
        /// <param name="cardsDiscarded">The number of cards discarded</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        public void Simulate(Character caster, Character target, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, int statusesCleared, bool empowered, bool weakened)
        {
            int damage = 0, healing = 0, shield = 0;

            if (target != null)
            {
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded) + (spell.valueScalingPerStatus * statusesCleared);
                value = EmpowerWeakenValue(value, empowered, weakened);

                //Debug.Log("Spell cast: " + spellName + " at " + caster.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);

                switch (spell.effectType)
                {
                    case E_DamageTypes.Healing:
                        healing += value;
                        break;
                    case E_DamageTypes.Shield:
                        shield += value;
                        break;
                    case E_DamageTypes.Arcana:
                        break;
                    default:
                        damage += value;
                        break;
                }

                if (target.GetHealth().GetHealthPercentage() < spell.executeThreshold)
                {
                    //Debug.Log("Kill " + target.characterName + " with " + name + " at: " + (target.GetHealth().GetHealthPercentage()));
                    damage += 999999999;
                }
            }

            target.SimulateValues(damage, healing, shield);
        }

        #endregion

        #endregion
    }
}