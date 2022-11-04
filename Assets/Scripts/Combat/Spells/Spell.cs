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

        public float QuerySpellCastTime(Character target, Character caster, Vector2 spawnPosition)
        {
            float time = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

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

        public void CastSpell(Character target, Character caster, Vector2 spawnPosition, bool empowered, bool weakened, Deck2D hand)
        {
            bool discardCards = false;
            int cardsInHand = hand.CurrentCardsLength();
            float time = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                if (module.discardCards)
                    discardCards = true;

                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = HelperFunctions.CombineLists(CombatManager.instance.playerTeamManager.team, CombatManager.instance.enemyTeamManager.team);

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
                            VFXManager.instance.AffectSelfDelay(this, caster, module, cardsInHand, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            break;
                        case E_SpellTargetType.Target:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, target, module, cardsInHand, spawnPosition, moduleTime + hitDelay, empowered, weakened);
                            break;
                        case E_SpellTargetType.Chain:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in targetTeamManager.team)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, cardsInHand, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in targetTeamManager.team)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, cardsInHand, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.RandomTargetTeam:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], module, cardsInHand, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            break;
                        case E_SpellTargetType.RandomAll:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, allCharacters[Random.Range(0, allCharacters.Count)], module, cardsInHand, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
                            break;
                        case E_SpellTargetType.All:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in allCharacters)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, cardsInHand, spawnPosition, moduleTime + hitDelay + time, empowered, weakened);
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
        }

        #region Affect Characters

        public void AffectSelf(Character caster, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, bool empowered, bool weakened)
        {
            if (caster != null)
            {
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded);
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
            }
        }

        public void AffectTarget(Character caster, Character target, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, bool empowered, bool weakened)
        {
            if (target != null)
            {
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded);
                value = EmpowerWeakenValue(value, empowered, weakened);

                //Debug.Log("Spell cast: " + spellName + " at " + target.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
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
                    //Debug.Log("Kill " + target.characterName + " with " + name + " at: " + (target.GetHealth().GetHealthPercentage()));
                    target.GetHealth().ChangeHealth(E_DamageTypes.Perforation, 9999999, caster);
                }

                if (target.GetHealth().GetHealth() < 1)
                {
                    target.CheckOverlay();
                }

                //Sound effects here
            }
        }

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

        #region Simulate Spell

        public void SimulateSpellValues(Character target, Character caster, bool empowered, bool weakened, Deck2D hand)
        {
            int cardsInHand = hand.CurrentCardsLength();

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
                            Simulate(caster, caster, module, cardsInHand, empowered, weakened);
                            break;
                        case E_SpellTargetType.Target:
                            Simulate(caster, target, module, cardsInHand, empowered, weakened);
                            break;
                        case E_SpellTargetType.Chain:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, module, cardsInHand, empowered, weakened);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            foreach (Character character in targetTeamManager.team)
                            {
                                Simulate(caster, character, module, cardsInHand, empowered, weakened);
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
                                Simulate(caster, character, module, cardsInHand, empowered, weakened);
                            }
                            break;
                    }
                }
            }
        }

        public void Simulate(Character caster, Character target, CombatHelperFunctions.SpellModule spell, int cardsDiscarded, bool empowered, bool weakened)
        {
            int damage = 0, healing = 0, shield = 0;

            if (target != null)
            {
                int value = spell.value + (spell.valueScalingPerDiscard * cardsDiscarded);
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