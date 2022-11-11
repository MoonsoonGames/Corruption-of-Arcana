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
    public class Timeline : MonoBehaviour
    {
        #region Setup

        public static Timeline instance;

        public float initialDelay = 2f;

        List<CombatHelperFunctions.SpellInstance> spells = new List<CombatHelperFunctions.SpellInstance>();
        List<SpellBlock> spellBlocks = new List<SpellBlock>();
        public Object spellBlockPrefab;
        public float spellDelayOffset = 0.5f;

        List<CombatHelperFunctions.StatusInstance> statuses = new List<CombatHelperFunctions.StatusInstance>();
        public float statusOffset = 0.3f;

        public Character player;
        public Deck2D hand;
        ArcanaManager arcanaManager; public ArcanaManager GetArcanaManager() { return arcanaManager; }
        int arcanaCount = 0;

        private void Start()
        {
            instance = this;
            arcanaManager = player.GetComponent<ArcanaManager>();
        }

        #endregion

        #region Changing Timeline

        #region Adding/Removing Spell Instances

        /// <summary>
        /// Adds spell instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void AddSpellInstance(CombatHelperFunctions.SpellInstance newSpellInstance)
        {
            spells.Add(newSpellInstance);
            CalculateTimeline();
        }

        /// <summary>
        /// Removes spell instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void RemoveSpellInstance(CombatHelperFunctions.SpellInstance newSpellInstance)
        {
            spells.Remove(newSpellInstance);
            CalculateTimeline();
        }

        /// <summary>
        /// Adds status instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public bool AddStatusInstance(CombatHelperFunctions.StatusInstance newStatusInstance)
        {
            bool apply = true;
            bool replace = false;
            CombatHelperFunctions.StatusInstance duplicate = new CombatHelperFunctions.StatusInstance();
            foreach (CombatHelperFunctions.StatusInstance status in statuses)
            {
                if (status.status == newStatusInstance.status && status.target == newStatusInstance.target)
                {
                    apply = false;

                    if (status.duration < newStatusInstance.duration)
                    {
                        replace = true;
                        duplicate = status;
                    }
                }
            }

            if (apply)
            {
                //Debug.Log(newStatusInstance.status.effectName + " has been added");
                statuses.Add(newStatusInstance);
            }
            else if (replace)
            {
                if (duplicate.duration < newStatusInstance.duration)
                {
                    Debug.Log("Should remove element: " + duplicate.status.effectName);
                    statuses.Remove(duplicate);
                    statuses.Add(newStatusInstance);
                }
            }

            return apply;
        }

        /// <summary>
        /// Removes status instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void RemoveStatusInstance(CombatHelperFunctions.StatusInstance newStatusInstance)
        {
            CombatHelperFunctions.StatusInstance remove = new CombatHelperFunctions.StatusInstance();
            foreach (CombatHelperFunctions.StatusInstance status in statuses)
            {
                if (status.status == newStatusInstance.status && status.target == newStatusInstance.target)
                {
                    remove = status;
                }
            }

            Debug.Log(remove.status.effectName + " has been removed");
            statuses.Remove(remove);
        }

        public void UpdateStatusDurations()
        {
            List<CombatHelperFunctions.StatusInstance> newStatusList = new List<CombatHelperFunctions.StatusInstance>(0);

            foreach (CombatHelperFunctions.StatusInstance status in statuses)
            {
                CombatHelperFunctions.StatusInstance instance = new CombatHelperFunctions.StatusInstance();
                instance.SetStatusInstance(status.status, status.target, status.duration - 1);
                newStatusList.Add(instance);
            }

            statuses = newStatusList;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Sorts the list of spells by their speed and spawns UI blocks on the timeline
        /// </summary>
        void CalculateTimeline()
        {
            if (spells.Count > 1)
                spells.Sort(SortBySpeed);
            arcanaCount = 0;

            //Clear old blocks that are no longer being cast
            foreach (var item in spellBlocks)
            {
                Destroy(item.gameObject);
            }

            spellBlocks.Clear();

            if (spells.Count > 0)
            {
                //Spawn UI for cards
                foreach (var item in spells)
                {
                    string text;

                    if (player != item.caster && player.enlightened == false)
                    {
                        text = "Enemy Spell";
                    }
                    else
                    {
                        text = item.caster.stats.characterName + " is casting " + item.spell.spellName + " on " + item.target.stats.characterName + " (" + item.spell.speed + ")";
                    }

                    //Creates spell block game object
                    GameObject spellBlockObject = Instantiate(spellBlockPrefab) as GameObject;
                    spellBlockObject.transform.SetParent(transform, false);

                    //Sets spell block values
                    //Sets spell block values
                    SpellBlock spellBlock = spellBlockObject.GetComponent<SpellBlock>();
                    spellBlock.text.text = text;
                    if (item.spell.overrideColor)
                        spellBlock.image.color = item.spell.timelineColor;
                    else
                        spellBlock.image.color = item.caster.stats.timelineColor;

                    //Adds spell block to layout group
                    spellBlocks.Add(spellBlock);

                    if (item.caster == player)
                    {
                        arcanaCount += item.spell.arcanaCost;
                    }
                }

                arcanaManager.CheckArcana(arcanaCount);

                SimulateSpellEffects();
            }
        }

        int SortBySpeed(CombatHelperFunctions.SpellInstance c1, CombatHelperFunctions.SpellInstance c2)
        {
            int result = c1.spell.speed.CompareTo(c2.spell.speed);
            if (result == 0)
            {
                //Sorting tied, prioritize the player
                if (c1.caster == player || c2.caster == player)
                {
                    result = (c1.caster == player ? -1 : 1);
                }
            }

            return result;
        }

        #endregion

        #region Simulating Spell Effects

        public void SimulateSpellEffects()
        {
            List<TeamManager> teamManagers = new List<TeamManager>();
            teamManagers.Add(CombatManager.instance.playerTeamManager);
            teamManagers.Add(CombatManager.instance.enemyTeamManager);

            foreach (TeamManager manager in teamManagers)
            {
                foreach (Character item in manager.team)
                {
                    item.ResetValues();
                }
            }

            int cardsDiscarded = hand.CurrentCardsLength();

            foreach (CombatHelperFunctions.SpellInstance item in spells)
            {
                item.spell.SimulateSpellValues(player, item.target, item.caster, item.empowered, item.weakened, cardsDiscarded);
            }
        }

        #endregion

        #endregion

        #region Playing Timeline

        /// <summary>
        /// Casts every spell on the timeline and then removes them
        /// </summary>
        /// <returns></returns>
        public float PlayTimeline()
        {
            float delay = CastSpells() + initialDelay;
            Invoke("ActivateStatuses", delay);
            delay += statuses.Count * statusOffset;
            delay += 0.5f;

            Invoke("EndTimeline", delay);

            return delay;
        }

        public bool discardCards = false;
        public List<Character> clearStatusChars = new List<Character>();

        void EndTimeline()
        {
            if (discardCards)
            {
                hand.RemoveAllCards(true);
                discardCards = false;
            }

            foreach (Character character in clearStatusChars)
            {
                Timeline.instance.RemoveStatusesOnCharacter(character);
            }
        }

        float CastSpells()
        {
            //Generates a delay for the entire set of spells being cast
            float i = 0;
            //Loop through list and cast spell;
            foreach (CombatHelperFunctions.SpellInstance item in spells)
            {
                //Use a coroutine to stagger spellcasting
                StartCoroutine(IDelaySpell(item, i + initialDelay));
                Vector2 spawnPosition = new Vector2(spellBlocks[0].transform.position.x, spellBlocks[0].transform.position.y);
                i += item.spell.QuerySpellCastTime(item.target, item.caster, spawnPosition) + spellDelayOffset;

                //Debug.Log("Spell " + item.spell.spellName + " has a delay of " + i);
            }

            return i;
        }

        public void StartSpellCoroutine(Spell spell, Character target, Character caster, Vector2 spawnPosition, bool empowered, bool weakened, Deck2D hand, int cardsInHand,
            CombatHelperFunctions.SpellModule module, int removedStatusCount, float time, float hitDelay,
            TeamManager targetTeamManager, List<Character> allCharacters)
        {
            StartCoroutine(spell.IDetermineTarget(target, caster, spawnPosition, empowered, weakened, hand, cardsInHand,
                module, removedStatusCount, time, hitDelay, targetTeamManager, allCharacters));
        }

        void ActivateStatuses()
        {
            //Debug.Log("Activate statuses: " + statuses.Count);
            UpdateStatusDurations();

            //Generates a delay for the entire set of spells being cast
            float delay = 0;

            foreach (CombatHelperFunctions.StatusInstance item in statuses)
            {
                StartCoroutine(IDelayStatus(item, delay + initialDelay));

                delay += statusOffset;
            }
        }

        public void HitStatuses(Character target, Character attacker)
        {
            foreach (CombatHelperFunctions.StatusInstance item in statuses)
            {
                if (item.target == target)
                {
                    item.status.HitEffect(target, attacker);
                }
            }
        }

        /// <summary>
        /// Delays the casting of a spell by its speed
        /// </summary>
        /// <param name="spellInstance"></param>
        /// <returns></returns>
        IEnumerator IDelaySpell(CombatHelperFunctions.SpellInstance spellInstance, float delay)
        {
            //Get location of first spell block
            Vector2 spawnPosition = new Vector2(spellBlocks[0].transform.position.x, spellBlocks[0].transform.position.y);

            //Get initial cards count
            int cardsDiscarded = hand.CurrentCardsLength();

            yield return new WaitForSeconds(delay);

            Character caster = spellInstance.caster;
            Character target = spellInstance.target;

            if (caster.stun)
            {
                Debug.Log("Stunned, skip spell");
                //Effect for fumbling spell
            }
            else if (caster.GetHealth().dying)
            {
                Debug.Log("Dead, skip spell");
                //Effect for fumbling spell
            }
            else if (caster.banish && caster != target)
            {
                Debug.Log("Caster is banished, skip spell");
            }
            else if (target.banish && caster != target)
            {
                Debug.Log("Target is banished, skip spell");
            }
            else
            {
                //Debug.Log(spellInstance.caster.characterName + " played " + spellInstance.spell.spellName + " on " + spellInstance.target.characterName + " at time " + spellInstance.spell.speed);
                spellInstance.spell.CastSpell(spellInstance.target, spellInstance.caster, spawnPosition, spellInstance.empowered, spellInstance.weakened, hand, cardsDiscarded);
            }

            if (spellInstance.spell.drawCard != null)
            {
                DeckManager.instance.AddToStart(spellInstance.spell.drawCard);
            }

            yield return new WaitForSeconds(spellInstance.spell.QuerySpellCastTime(spellInstance.target, spellInstance.caster, spawnPosition));

            SimulateSpellEffects();
            RemoveSpellInstance(spellInstance);
            CalculateTimeline();

            yield return new WaitForSeconds(1f);

            SimulateSpellEffects();
        }

        IEnumerator IDelayStatus(CombatHelperFunctions.StatusInstance statusInstance, float delay)
        {
            yield return new WaitForSeconds(delay);

            statusInstance.status.ActivateEffect(statusInstance.target);

            //Debug.Log("Activated " + statusInstance.status + " on " + statusInstance.target + " with " + statusInstance.duration + " turns remaining");

            if (statusInstance.duration <= 0)
            {
                //Debug.Log("Expired");
                statusInstance.status.Remove(statusInstance.target);
            }

            //RemoveStatusInstance(statusInstance);
            SimulateSpellEffects();
            CalculateTimeline();
        }

        public void ActivateTurnModifiers()
        {
            foreach(CombatHelperFunctions.StatusInstance status in statuses)
            {
                status.status.ActivateTurnModifiers(status.target);
            }
        }

        #endregion

        public void RemoveStatusesOnCharacter(Character target)
        {
            List<CombatHelperFunctions.StatusInstance> statusesToRemove = new List<CombatHelperFunctions.StatusInstance>();

            foreach (CombatHelperFunctions.StatusInstance status in statuses)
            {
                if (status.target == target)
                {
                    statusesToRemove.Add(status);
                }
            }

            foreach (CombatHelperFunctions.StatusInstance status in statusesToRemove)
            {
                status.status.Remove(status.target);
            }
        }

        public int StatusCount(Character target)
        {
            int statusCount = 0;

            foreach (CombatHelperFunctions.StatusInstance status in statuses)
            {
                if (status.target == target)
                {
                    statusCount++;
                }
            }

            return statusCount;
        }
    }
}