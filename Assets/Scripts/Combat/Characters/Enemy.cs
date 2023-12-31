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
    public class Enemy : Character
    {
        SpellCastingAI SpellCastingAI;
        public List<CombatHelperFunctions.AISpell> aISpells;

        EnemyManager enemyManager;

        public Object cardPrefab;

        public List<string> spellsThisTurn;
        public List<string> spellsLastTurn;

        public List<E_DamageTypes> effectsThisTurn;

        [ContextMenu("Setup References")]
        public override void SetupReferences()
        {
            base.SetupReferences();

            enemyManager = (EnemyManager)teamManager;
            SpellCastingAI = stats.ai;
            aISpells = new List<CombatHelperFunctions.AISpell>();

            foreach (CombatHelperFunctions.AISpell spell in stats.aISpells)
            {
                //Setup for the AI spells
                CombatHelperFunctions.AISpell newSpell = new CombatHelperFunctions.AISpell();

                newSpell.spell = spell.spell;
                newSpell.spawnAsCard = spell.spawnAsCard;
                newSpell.targetSelf = spell.targetSelf;
                newSpell.targetAllies = spell.targetAllies;
                newSpell.targetEnemies = spell.targetEnemies;
                newSpell.timeCooldown = spell.timeCooldown;
                newSpell.lastUsed = 99;

                aISpells.Add(newSpell);
            }
        }

        public override void StartTurn()
        {
            if (stun || banish || health.dying)
            {
                Debug.Log(stats.characterName + " has been stunned/banished, skipping turn");
            }
            else
            {
                spellsLastTurn = spellsThisTurn == null ? new List<string>() : spellsThisTurn;
                spellsThisTurn = new List<string>();
                effectsThisTurn.Clear();

                for (int i = 0; i < stats.actions; i++)
                {
                    //In future, determine target depending on spell so it can cast support spells on allies/self
                    CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
                    CombatHelperFunctions.SpellUtility spellUtility = PrepareSpell();

                    if (spellUtility.utility >= 0)
                    {
                        if (spellUtility.spell.spawnAsCard)
                        {
                            newSpellInstance.SetSpellInstance(spellUtility.spell.spell, spellUtility.target, this);
                        }
                        else
                        {
                            newSpellInstance.SetSpellInstance(spellUtility.spell.spell, spellUtility.target, this);
                        }

                        if (newSpellInstance.spell != null)
                            enemyManager.AddSpellInstance(newSpellInstance);
                    }
                    else
                    {
                        Debug.Log(stats.characterName + " is skipping their turn");
                    }

                    if (newSpellInstance.spell != null)
                    {
                        spellsThisTurn.Add(newSpellInstance.spell.spellName);

                        foreach (var item in newSpellInstance.spell.spellModules)
                        {
                            effectsThisTurn.Add(item.effectType);
                        }
                    }
                }
            }

            ResetCooldowns();

            base.StartTurn();
        }

        /// <summary>
        /// Determines the spell that the AI will cast this turn
        /// </summary>
        /// <returns>The spell the AI plans to cast</returns>
        public override CombatHelperFunctions.SpellUtility PrepareSpell()
        {
            CombatHelperFunctions.SpellUtility spell = SpellCastingAI.GetSpell(aISpells, this, enemyManager.team, enemyManager.opposingTeam);

            if (spell.utility >= 0)
            {
                Character target = spell.target;
                bool spawnCard = spell.spell.spawnAsCard;
                
                int index = 999;
                for (int i = 0; i < aISpells.Count; i++)
                {
                    //Determines which spell in the array is being cast
                    if (aISpells[i].spell == spell.spell.spell)
                    {
                        //Saves the spell index
                        index = i;
                    }
                }

                if (aISpells.Count > index)
                {
                    //Resets the cooldown of the spell used
                    CombatHelperFunctions.AISpell newSpell = new CombatHelperFunctions.AISpell();

                    newSpell.spell = aISpells[index].spell;
                    newSpell.spawnAsCard = aISpells[index].spawnAsCard;
                    newSpell.targetSelf = aISpells[index].targetSelf;
                    newSpell.targetAllies = aISpells[index].targetAllies;
                    newSpell.targetEnemies = aISpells[index].targetEnemies;
                    newSpell.timeCooldown = aISpells[index].timeCooldown;
                    newSpell.lastUsed = 0;

                    aISpells.RemoveAt(index);
                    aISpells.Add(newSpell);

                    spawnCard = newSpell.spawnAsCard;
                    //Debug.Log(spawnCard + " 2");
                }

                if (spawnCard)
                {
                    //Spawn enemy card on player decks
                    Deck2D deck = target.GetComponentInChildren<Deck2D>();

                    GameObject card = Instantiate(cardPrefab, deck.transform) as GameObject;
                    EnemyCard cardLogic = card.GetComponent<EnemyCard>();
                    cardLogic.Setup(spell.spell.spell);
                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    //Add the card to the array
                    deck.AddCard(cardDrag);

                    //Reset card scales
                    cardDrag.ScaleCard(1, false);
                }
            }

            return spell;
        }

        /// <summary>
        /// For every spell, add 1 to the last turn it was used
        /// </summary>
        void ResetCooldowns()
        {
            List<CombatHelperFunctions.AISpell> newList = new List<CombatHelperFunctions.AISpell>();

            foreach (CombatHelperFunctions.AISpell spell in aISpells)
            {
                CombatHelperFunctions.AISpell newSpell = new CombatHelperFunctions.AISpell();

                newSpell.spell = spell.spell;
                newSpell.spawnAsCard = spell.spawnAsCard;
                newSpell.targetSelf = spell.targetSelf;
                newSpell.targetAllies = spell.targetAllies;
                newSpell.targetEnemies = spell.targetEnemies;
                newSpell.timeCooldown = spell.timeCooldown;
                newSpell.lastUsed = spell.lastUsed + 1;

                newList.Add(newSpell);
            }

            aISpells.Clear();

            aISpells = newList;
        }

        protected override void Silence()
        {
            //prevent AI from casting spells
        }
    }
}
