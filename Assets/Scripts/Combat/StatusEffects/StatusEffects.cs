using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewStatusEffects", menuName = "Combat/Status Effects", order = 1)]
    public class StatusEffects : ScriptableObject
    {
        public GeneralCombat.StatusModule[] effectModules;

        public void Apply()
        {
            //Apply status effect on target, add to character list
        }

        public void Remove()
        {
            //Remove status effect on target, remove from character list
        }

        public void ActivateEffect(Character target, Character attacker)
        {
            //Apply effects when timeline ends
            foreach (GeneralCombat.StatusModule module in effectModules)
            {
                //May need additional checks to see if target is still valid in case they are killed by the multihit effect, speficially for the lists
                switch (module.target)
                {
                    case E_StatusTargetType.Self:
                        AffectTarget(target, module.effectType, module.value);
                        break;
                    case E_StatusTargetType.Team:
                        TeamManager targetTeamManager = target.GetManager();
                        foreach (Character character in targetTeamManager.team)
                        {
                            AffectTarget(character, module.effectType, module.value);
                        }
                        break;
                    case E_StatusTargetType.OpponentTeam:
                        TeamManager opponentTeamManager = CombatManager.instance.GetOpposingTeam(target.GetManager());
                        foreach (Character character in opponentTeamManager.team)
                        {
                            AffectTarget(character, module.effectType, module.value);
                        }
                        break;
                    case E_StatusTargetType.Reflect:
                        if (attacker != null)
                        {
                            AffectTarget(attacker, module.effectType, module.value);
                        }
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
        }

        void AffectTarget(Character target, E_DamageTypes effectType, int value)
        {
            //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            E_DamageTypes realEffectType = GeneralCombat.ReplaceRandom(effectType);
            target.GetHealth().ChangeHealth(realEffectType, value);

            //Sound effects here
        }
    }
}