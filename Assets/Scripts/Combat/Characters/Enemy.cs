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
        List<CombatHelperFunctions.AISpell> aISpells;

        EnemyManager enemyManager;

        public Object cardPrefab;

        protected override void Start()
        {
            base.Start();
            enemyManager = (EnemyManager)teamManager;
            SpellCastingAI = stats.ai;
            aISpells = new List<CombatHelperFunctions.AISpell>();

            foreach (CombatHelperFunctions.AISpell spell in stats.aISpells)
            {
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

        public override CombatHelperFunctions.SpellUtility PrepareSpell()
        {
            CombatHelperFunctions.SpellUtility spell = SpellCastingAI.GetSpell(aISpells, this, enemyManager.team, enemyManager.opposingTeam);

            if (spell.utility >= 0)
            {
                Character target = spell.target;
                bool spawnCard = spell.spell.spawnAsCard;
                //Debug.Log(spawnCard + " 1");
                int index = 999;
                for (int i = 0; i < aISpells.Count; i++)
                {
                    if (aISpells[i].spell == spell.spell.spell)
                    {
                        //Debug.Log(aISpells[i].spell.spellName + " same spell, set cooldown");
                        index = i;
                    }
                    else
                    {
                        //Debug.Log("different spell, set cooldown");
                    }
                }

                if (aISpells.Count > index)
                {
                    //Debug.Log("Reset spell cooldown of " + aISpells[index].spell.spellName);
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
                    Debug.Log(spawnCard + " 2");
                }
                else
                {
                    //Debug.Log(index + " index");
                }

                if (spawnCard)
                {
                    //Debug.Log("Spawn Enemy Card");
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
                else
                {
                    //Debug.Log("Don't Spawn Enemy Card");
                }
            }

            return spell;
        }

        public override void StartTurn()
        {
            if (!stun)
            {
                //In future, determine target depending on spell so it can cast support spells on allies/self
                CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
                CombatHelperFunctions.SpellUtility spellUtility = PrepareSpell();

                if (spellUtility.utility >= 0)
                {
                    if (spellUtility.spell.spawnAsCard)
                    {
                        newSpellInstance.SetSpellInstance(spellUtility.spell.spell, empowerDeck, weakenDeck, spellUtility.target, this);
                    }
                    else
                    {
                        newSpellInstance.SetSpellInstance(spellUtility.spell.spell, false, false, spellUtility.target, this);
                    }

                    enemyManager.AddSpellInstance(newSpellInstance);
                }
                else
                {
                    Debug.Log(stats.characterName + " is skipping their turn");
                }
            }
            else
            {
                Debug.Log(stats.characterName + " has been stunned, skipping turn");
            }

            base.StartTurn();
        }

        protected override void Silence()
        {
            //prevent AI from casting spells
        }
    }
}