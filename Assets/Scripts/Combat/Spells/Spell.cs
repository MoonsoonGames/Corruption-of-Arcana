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
                List<Character> allCharacters = HelperFunctions.CombineLists(targetTeamManager.team, casterTeamManager.team);

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

        public void CastSpell(Character target, Character caster, Vector2 spawnPosition)
        {
            float time = 0;

            foreach (CombatHelperFunctions.SpellModule module in spellModules)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = HelperFunctions.CombineLists(targetTeamManager.team, casterTeamManager.team);

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
                            VFXManager.instance.AffectSelfDelay(this, caster, module, spawnPosition, moduleTime + hitDelay + time);
                            break;
                        case E_SpellTargetType.Target:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, target, module, spawnPosition, moduleTime + hitDelay);
                            break;
                        case E_SpellTargetType.Chain:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in targetTeamManager.team)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, spawnPosition, moduleTime + hitDelay + time);
                            }
                            break;
                        case E_SpellTargetType.Cleave:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in targetTeamManager.team)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, spawnPosition, moduleTime + hitDelay + time);
                            }
                            break;
                        case E_SpellTargetType.RandomTargetTeam:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], module, spawnPosition, moduleTime + hitDelay + time);
                            break;
                        case E_SpellTargetType.RandomAll:
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position);
                            VFXManager.instance.AffectTargetDelay(this, caster, allCharacters[Random.Range(0, allCharacters.Count)], module, spawnPosition, moduleTime + hitDelay + time);
                            break;
                        case E_SpellTargetType.All:
                            x = targetTeamManager.team.Count * multihitDelay;
                            moduleTime = VFXManager.instance.QueryTime(spawnPosition, target.transform.position) + x;
                            foreach (Character character in allCharacters)
                            {
                                VFXManager.instance.AffectTargetDelay(this, caster, character, module, spawnPosition, moduleTime + hitDelay + time);
                            }
                            break;
                    }

                    time += moduleDelay;
                }
            }
        }

        #region Affect Characters

        public void AffectSelf(Character caster, CombatHelperFunctions.SpellModule spell)
        {
            if (caster == null)
            {

                Debug.Log("Spell cast: " + spellName + " at " + caster.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                caster.GetHealth().ChangeHealth(spell.effectType, spell.value, caster);

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

        public void AffectTarget(Character caster, Character target, CombatHelperFunctions.SpellModule spell)
        {
            if (target != null)
            {

                Debug.Log("Spell cast: " + spellName + " at " + target.stats.characterName);
                //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
                E_DamageTypes realEffectType = CombatHelperFunctions.ReplaceRandom(spell.effectType);
                target.GetHealth().ChangeHealth(realEffectType, spell.value, caster);

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

                //Sound effects here
            }
        }

        #endregion

        #endregion
    }
}