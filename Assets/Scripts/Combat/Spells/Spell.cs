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
        public Sprite nameImage;
        [TextArea(3, 10)]
        public string flavourText; // Flavour text
        [TextArea(2, 10)]
        public string spellDescription; // Basic desciption of spell effect
        public Sprite cardImage;
        public Sprite background;
        public E_CardTypes cardType = E_CardTypes.Cards;

        public Spell previousTier;
        public Spell nextTier;

        #endregion

        #region Timeline Icon

        [Header("Timeline Colour")]
        public bool overrideColor;
        public Color timelineColor = new Color(0, 0, 0, 255);
        public Sprite timelineIcon;

        #endregion

        #region Spell Logic

        [Header("Spell Logic")]
        public float speed;
        public int loadoutCost;
        public int arcanaCost;
        public int potionCost;
        public float shieldCost;
        public E_PotionType potionType;

        [Header("Advanced Logic")]
        public bool discardAfterCasting = false;
        public bool discardAfterTurn = false;
        public Spell drawCard;
        public bool discardCards = false;
        public bool returnDiscardPile = false;
        public bool removeStatuses = false;

        [Header("Module Logic")]
        public float multihitDelay = 0.1f;
        public float moduleDelay = 0f;
        public CombatHelperFunctions.SpellModule[] spellModules;

        #endregion

        #region FX

        [Header("FX")]
        //Visual effects for projectile
        public Object castObject;
        public Object projectileObject;
        public float projectileSpeed = 0.6f;
        public E_ProjectilePoints[] projectilePoints;
        public Object impactObject;
        public Object projectileFXObject;
        public Color trailColor;
        public bool screenShake;
        public E_SpellTargetType idealTarget = E_SpellTargetType.Target;

        //Visual effects for hit effect

        //Sound effects for preparing the spell

        #endregion

        #endregion

        #region Spellcasting

        #region Cast Time

        /// <summary>
        /// Checks the delay between casting the spell and the visual projectile(s) hitting the target
        /// </summary>
        /// <param name="target">The initial target the spell was cast on</param>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="spawnPosition">The spawn position of the projectile</param>
        /// <returns>Time taken for the last projectile to hit the target</returns>
        public float QuerySpellCastTime(Character target, Character caster, float projectileSpeed)
        {
            float time = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                float hitDelay = module.hitCount * multihitDelay;
                float moduleTime = 0;

                bool playerTeam = caster.GetManager() == CombatManager.instance.playerTeamManager;
                Vector2[] points = VFXManager.instance.GetProjectilePoints(projectilePoints, caster, target);
                moduleTime = VFXManager.instance.QueryTime(points, projectileSpeed) + hitDelay + time;

                time += moduleDelay + moduleTime;
            }

            return time;
        }

        #endregion

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
        public void CastSpell(Character target, Character caster, Deck2D hand, int cardsInHand)
        {
            if (caster.CanCast() == false)
                return;

            List<Character> allCharacters = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);
            if (caster.confuse)
            {
                target = CombatHelperFunctions.ReplaceRandomTarget(allCharacters);
            }
            int removedStatusCount = Timeline.instance.StatusCount(target);
            float time = 0;

            TeamManager targetTeamManager = target.GetManager();

            int shieldRemoved = caster.GetHealth().GetShieldRemovedPercentage(1 - shieldCost);

            if (shieldCost > 0)
            {
                caster.GetHealth().SetShieldPercentage(1 - shieldCost);
            }

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                for (int i = 0; i < module.hitCount; i++)
                {
                    float hitDelay = i * this.multihitDelay;
                    //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists

                    Timeline.instance.StartSpellCoroutine(this, target, caster, hand, cardsInHand,
                    module, removedStatusCount, shieldRemoved, time, hitDelay, targetTeamManager, allCharacters);

                    time += moduleDelay;
                }
            }

            if (discardCards)
            {
                Timeline.instance.discardCards = true;
            }

            if (removeStatuses)
            {
                Timeline.instance.clearStatusChars.Add(target);
            }

            if (returnDiscardPile)
            {
                DeckManager.instance.DiscardPileToDeck(true, false);
            }

            if (potionCost > 0)
            {
                PotionManager.instance.ChangePotion(potionType, -potionCost);
            }
        }

        public IEnumerator IDetermineTarget(Character target, Character caster, Deck2D hand, int cardsInHand,
            CombatHelperFunctions.SpellModule module, int removedStatusCount, int shieldRemoved, float time, float hitDelay,
            TeamManager targetTeamManager, List<Character> allCharacters)
        {
            yield return new WaitForSeconds(hitDelay + time);

            Character randTarget;
            TeamManager opposingTeam = CombatManager.instance.GetOpposingTeam(caster.GetManager());
            E_DamageTypes trueEffectType = CombatHelperFunctions.ReplaceRandomDamageType(module.effectType);
            float delay;

            switch (module.target)
            {
                case E_SpellTargetType.Caster:
                    if (caster.GetHealth().dying == false)
                        VFXManager.instance.AffectSelfDelay(this, caster, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, 0f);
                    break;
                case E_SpellTargetType.Target:
                    if (target.GetHealth().dying == false)
                        VFXManager.instance.AffectTargetDelay(this, caster, target, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, 0f);
                    break;
                case E_SpellTargetType.Chain:
                    delay = targetTeamManager.team.Count * this.multihitDelay;
                    foreach (Character character in targetTeamManager.team)
                    {
                        if (character.GetHealth().dying == false)
                            VFXManager.instance.AffectTargetDelay(this, caster, character, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, delay);
                    }
                    break;
                case E_SpellTargetType.Cleave:
                    delay = targetTeamManager.team.Count * this.multihitDelay;
                    foreach (Character character in targetTeamManager.team)
                    {
                        if (character.GetHealth().dying == false)
                            VFXManager.instance.AffectTargetDelay(this, caster, character, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, delay);
                    }
                    break;
                case E_SpellTargetType.RandomEnemyTeam:
                    randTarget = CombatHelperFunctions.ReplaceRandomTarget(opposingTeam.team);
                    if (randTarget != null && randTarget.GetHealth().dying == false)
                        VFXManager.instance.AffectTargetDelay(this, caster, randTarget, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, 0f);
                    break;
                case E_SpellTargetType.RandomAll:
                    randTarget = CombatHelperFunctions.ReplaceRandomTarget(allCharacters);
                    if (randTarget != null && randTarget.GetHealth().dying == false)
                        VFXManager.instance.AffectTargetDelay(this, caster, randTarget, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, 0f);
                    break;
                case E_SpellTargetType.All:
                    delay = targetTeamManager.team.Count * this.multihitDelay;
                    foreach (Character character in allCharacters)
                    {
                        if (character.GetHealth().dying == false)
                            VFXManager.instance.AffectTargetDelay(this, caster, character, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, delay);
                    }
                    break;
                case E_SpellTargetType.AllEnemies:
                    delay = targetTeamManager.team.Count * this.multihitDelay;
                    foreach (Character character in opposingTeam.team)
                    {
                        if (character.GetHealth().dying == false)
                            VFXManager.instance.AffectTargetDelay(this, caster, character, module, trueEffectType, cardsInHand, removedStatusCount, shieldRemoved, delay);
                    }
                    break;
            }

            if (screenShake)
            {
                VFXManager.instance.ScreenShake();
            }
        }

        #endregion

        #region Affect Characters

        /// <summary>
        /// Applies the effect of an individual spell module on the caster
        /// </summary>
        /// <param name="caster">The character that cast the spell</param>
        /// <param name="spell">The individual spell module being applied</param>
        /// <param name="cardsDiscarded">The number of cards discarded</param>
        /// <param name="empowered">Whether the spell is empowered</param>
        /// <param name="weakened">Whether the spell is weakened</param>
        public void AffectSelf(Character caster, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatusCount, int shieldRemoved)
        {
            if (caster != null)
            {
                //Modifies the value if the spell is empowered or scales with how many cards are discarded
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded) + (spell.valueScalingPerStatus * removedStatusCount) + (int)(spell.valueScalingShieldCost * shieldRemoved) + (int)(spell.valueScalingDamageTaken * caster.GetDamageTakenThisTurn());
                value = EmpowerWeakenValue(caster.stats, value, caster.empowerDeck, caster.weakenDeck);

                if (spell.effectType == E_DamageTypes.Summon && spell.summon != null)
                {
                    if (spell.value > 0)
                    {
                        //bool playerTeam = caster.GetManager() == CombatManager.instance.playerTeamManager;
                        Vector2[] points = VFXManager.instance.GetProjectilePoints(projectilePoints, caster, caster);
                        for (int i = 0; i < spell.value; i++)
                            LoadCombatManager.instance.AddEnemy(spell.summon, points, projectileObject, projectileSpeed, impactObject, projectileFXObject, trailColor);
                    }
                }
                else
                {
                    //Debug.Log("Spell cast: " + spellName + " at " + caster.stats.characterName);
                    //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                    caster.GetHealth().ChangeHealth(effectType, value, caster);
                }

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    int statusValue = spell.statuses[i].valueSuccess;
                    statusValue = EmpowerWeakenValue(caster.stats, statusValue, caster.empowerDeck, caster.weakenDeck);

                    if (spell.statuses[i].remove)
                    {
                        if (Timeline.instance.CheckStatusAgainstTarget(spell.statuses[i].status, caster))
                        {
                            //remove status i on target
                            spell.statuses[i].status.Remove(caster);
                            caster.GetHealth().ChangeHealth(effectType, statusValue, caster);
                        }
                    }
                    else 
                    {
                        if (CombatHelperFunctions.ApplyEffect(caster, spell.statuses[i]))
                        {
                            //apply status i on target
                            spell.statuses[i].status.Apply(caster, spell.statuses[i].duration);
                            caster.GetHealth().ChangeHealth(effectType, statusValue, caster);
                        }
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
        public void AffectTarget(Character caster, Character target, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatusCount, int shieldRemoved)
        {
            if (target != null)
            {
                //Modifies the value if the spell is empowered or scales with how many cards are discarded
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded) + (spell.valueScalingPerStatus * removedStatusCount) + (int)(spell.valueScalingShieldCost * shieldRemoved) + (int)(spell.valueScalingDamageTaken * caster.GetDamageTakenThisTurn());
                value = EmpowerWeakenValue(caster.stats, value, target.empowerDeck, target.weakenDeck);

                if (spell.effectType == E_DamageTypes.Summon && spell.summon != null)
                {
                    if (spell.value > 0)
                    {
                        //bool playerTeam = caster.GetManager() == CombatManager.instance.playerTeamManager;
                        Vector2[] points = VFXManager.instance.GetProjectilePoints(projectilePoints, caster, caster);
                        for (int i = 0; i < spell.value; i++)
                            LoadCombatManager.instance.AddEnemy(spell.summon, points, projectileObject, projectileSpeed, impactObject, projectileFXObject, trailColor);
                    }
                }
                else
                {
                    target.GetHealth().ChangeHealth(effectType, value, caster);
                }

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    int statusValue = spell.statuses[i].valueSuccess;
                    statusValue = EmpowerWeakenValue(caster.stats, statusValue, target.empowerDeck, target.weakenDeck);

                    if (spell.statuses[i].remove)
                    {
                        if (Timeline.instance.CheckStatusAgainstTarget(spell.statuses[i].status, target))
                        {
                            //remove status i on target
                            spell.statuses[i].status.Remove(target);
                            target.GetHealth().ChangeHealth(effectType, statusValue, caster);
                        }
                    }
                    else
                    {
                        if (CombatHelperFunctions.ApplyEffect(target, spell.statuses[i]))
                        {
                            //apply status i on target
                            spell.statuses[i].status.Apply(target, spell.statuses[i].duration);
                            target.GetHealth().ChangeHealth(effectType, statusValue, caster);
                        }
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
        int EmpowerWeakenValue(CharacterStats caster, int originalValue, bool empowered, bool weakened)
        {
            float floatValue = originalValue;
            int value = originalValue;

            if (caster.usesArcana)
            {
                if (empowered && !weakened)
                {
                    Debug.Log(spellName + " is empowered " + floatValue + " to " + (floatValue * 1.5f));
                    floatValue = (floatValue * 1.5f);
                }
                else if (weakened && !empowered)
                {
                    Debug.Log(spellName + " is weakened " + floatValue + " to " + (floatValue * 0.5f));
                    floatValue = (floatValue * 0.5f);
                }
                value = (int)Mathf.Round(floatValue);
            }

            return value;
        }

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
        public void SimulateSpellValues(Character player, Character target, Character caster, int cardsInHand)
        {
            if (Timeline.instance.ShowSpells(caster) == false)
            {
                return;
            }

            int removedStatusCount = Timeline.instance.StatusCount(target);
            //Debug.Log("simulated found " + removedStatusCount + "statuses on " + target.stats.characterName);

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                if (module.effectType == E_DamageTypes.Summon)
                    break;

                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);
                int shieldRemoved = caster.GetHealth().GetShieldRemovedPercentage(1 - shieldCost);

                for (int i = 0; i < module.hitCount; i++)
                {
                    float x = 0;
                    //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                    switch (module.target)
                    {
                        case E_SpellTargetType.Caster:
                            Simulate(caster, caster, false, module, cardsInHand, removedStatusCount, shieldRemoved);
                            break;
                        case E_SpellTargetType.Target:
                            Simulate(caster, target, false, module, cardsInHand, removedStatusCount, shieldRemoved);
                            break;
                        case E_SpellTargetType.Chain:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, false, module, cardsInHand, removedStatusCount, shieldRemoved);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, false, module, cardsInHand, removedStatusCount, shieldRemoved);
                            }
                            break;
                        case E_SpellTargetType.RandomEnemyTeam:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, true, module, cardsInHand, removedStatusCount, shieldRemoved);
                            }
                            break;
                        case E_SpellTargetType.RandomAll:
                            foreach (Character character in allCharacters)
                            {
                                Simulate(caster, character, true, module, cardsInHand, removedStatusCount, shieldRemoved);
                            }
                            break;
                        case E_SpellTargetType.All:
                            foreach (Character character in allCharacters)
                            {
                                Simulate(caster, character, false, module, cardsInHand, removedStatusCount, shieldRemoved);
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
        public void Simulate(Character caster, Character target, bool rand, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, int statusesCleared, int shieldRemoved)
        {
            Vector2Int damage = new Vector2Int(0, 0);
            int shield = 0;

            if (target != null && target.GetHealth().dying == false)
            {
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded) + (spell.valueScalingPerStatus * statusesCleared) + (int)(spell.valueScalingShieldCost * shieldRemoved) + (int)(spell.valueScalingDamageTaken * caster.GetDamageTakenThisTurn());
                value = EmpowerWeakenValue(caster.stats, value, target.empowerDeck, target.weakenDeck);

                //Debug.Log("Spell cast: " + spellName + " at " + caster.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);

                switch (spell.effectType)
                {
                    case E_DamageTypes.Healing:
                        if (rand)
                        {
                            damage.x += value;
                        }
                        else
                        {
                            damage.x += value;
                            damage.y += value;
                        }
                        break;
                    case E_DamageTypes.Shield:
                        shield += value;
                        break;
                    case E_DamageTypes.Arcana:
                        break;
                    default:
                        if (rand)
                        {
                            damage.y -= value;
                        }
                        else
                        {
                            damage.x -= value;
                            damage.y -= value;

                            if (value > 0)
                                Timeline.instance.SimulateHitStatuses(target, caster);
                        }
                        break;
                }

                for (int i = 0; i < spell.statuses.Length; i++)
                {
                    int statusValue = spell.statuses[i].valueSuccess;
                    statusValue = EmpowerWeakenValue(caster.stats, statusValue, target.empowerDeck, target.weakenDeck);

                    if (spell.statuses[i].remove)
                    {
                        if (Timeline.instance.CheckStatusAgainstTarget(spell.statuses[i].status, target))
                            switch (spell.effectType)
                            {
                                case E_DamageTypes.Healing:
                                    if (rand)
                                    {
                                        damage.x += statusValue;
                                    }
                                    else
                                    {
                                        damage.x += statusValue;
                                        damage.y += statusValue;
                                    }
                                    break;
                                case E_DamageTypes.Shield:
                                    shield += statusValue;
                                    break;
                                case E_DamageTypes.Arcana:
                                    break;
                                default:
                                    if (rand)
                                    {
                                        damage.y -= statusValue;
                                    }
                                    else
                                    {
                                        damage.x -= statusValue;
                                        damage.y -= statusValue;

                                        if (statusValue > 0)
                                            Timeline.instance.SimulateHitStatuses(target, caster);
                                    }
                                    break;
                            }
                    }
                    else
                    {
                        if (CombatHelperFunctions.ApplyEffect(target, spell.statuses[i]))
                        {
                            //apply status i on target
                            spell.statuses[i].status.SimulateStatusValues(target);
                            switch (spell.effectType)
                            {
                                case E_DamageTypes.Healing:
                                    if (rand)
                                    {
                                        damage.x += statusValue;
                                    }
                                    else
                                    {
                                        damage.x += statusValue;
                                        damage.y += statusValue;
                                    }
                                    break;
                                case E_DamageTypes.Shield:
                                    shield += statusValue;
                                    break;
                                case E_DamageTypes.Arcana:
                                    break;
                                default:
                                    if (rand)
                                    {
                                        damage.y -= statusValue;
                                    }
                                    else
                                    {
                                        damage.x -= statusValue;
                                        damage.y -= statusValue;

                                        if (statusValue > 0)
                                            Timeline.instance.SimulateHitStatuses(target, caster);
                                    }
                                    break;
                            }
                        }
                    }
                }

                foreach (var item in spell.statuses)
                {
                    if (CombatHelperFunctions.ApplyEffect(target, item))
                    {
                        item.status.SimulateStatusValues(target);
                    }
                }
            }

            target.SimulateValues(damage, shield, spell.executeThreshold);
        }

        #endregion

        #endregion

        #region Construct Card Icons

        public List<CombatHelperFunctions.SpellIconConstruct> SpellIcons()
        {
            //Debug.Log(spellName + " is generating icons");
            List<CombatHelperFunctions.SpellIconConstruct> iconConstructs = new List<CombatHelperFunctions.SpellIconConstruct>();

            float highestExecute = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                if (module.value == 0)
                    break;

                CombatHelperFunctions.SpellIconConstruct moduleConstruct = new CombatHelperFunctions.SpellIconConstruct();

                moduleConstruct.value = module.value;
                moduleConstruct.effectType = module.effectType;
                moduleConstruct.hitCount = module.hitCount;
                moduleConstruct.discardScaling = module.valueScalingPerDiscard;
                moduleConstruct.cleanseScaling = module.valueScalingPerStatus;
                moduleConstruct.target = module.target;

                //Debug.Log("Module: " + moduleConstruct.value + " X " + moduleConstruct.hitCount + " " + moduleConstruct.effectType + " on " + moduleConstruct.target.ToString());

                iconConstructs.Add(moduleConstruct);
            }

            return iconConstructs;
        }

        public List<CombatHelperFunctions.StatusIconConstruct> EffectIcons()
        {
            //Debug.Log(spellName + " is generating icons");
            List<CombatHelperFunctions.StatusIconConstruct> iconConstructs = new List<CombatHelperFunctions.StatusIconConstruct>();

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                foreach (CombatHelperFunctions.StatusStruct status in module.statuses)
                {
                    CombatHelperFunctions.StatusIconConstruct effectConstruct = new CombatHelperFunctions.StatusIconConstruct();

                    effectConstruct.effect = status.status;
                    effectConstruct.applyOverShield = status.applyOverShield;
                    effectConstruct.effectIcon = status.status.effectIcon;
                    effectConstruct.duration = status.duration;
                    effectConstruct.target = module.target;

                    //Debug.Log("Module: " + moduleConstruct.value + " X " + moduleConstruct.hitCount + " " + moduleConstruct.effectType + " on " + moduleConstruct.target.ToString());

                    iconConstructs.Add(effectConstruct);
                }
            }

            return iconConstructs;
        }

        public CombatHelperFunctions.ExecuteIconConstruct ExecuteIcons()
        {
            //Debug.Log(spellName + " is generating icons");
            CombatHelperFunctions.ExecuteIconConstruct moduleConstruct = new CombatHelperFunctions.ExecuteIconConstruct();

            float highestExecute = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                if (module.executeThreshold > highestExecute)
                {
                    moduleConstruct.threshold = module.executeThreshold;
                    moduleConstruct.target = module.target;
                }
            }

            return moduleConstruct;
        }

        public Dictionary<CharacterStats, int> SummonIcons()
        {
            //Debug.Log(spellName + " is generating icons");

            Dictionary<CharacterStats, int> moduleDictionary = new Dictionary<CharacterStats, int>();

            foreach (var module in spellModules)
            {
                if (module.effectType == E_DamageTypes.Summon)
                {
                    moduleDictionary.Add(module.summon, module.value);
                }
            }

            return moduleDictionary;
        }

        #endregion

        #region Upgrading

        public Spell GetUpgrade()
        {
            if (nextTier == null)
                return this;
            else
                return nextTier;
        }

        [ContextMenu("SetupTiers")]
        public void SetupOtherTiers()
        {
            previousTier.nextTier = this;
            nextTier.previousTier = this;
        }

        #endregion
    }
}