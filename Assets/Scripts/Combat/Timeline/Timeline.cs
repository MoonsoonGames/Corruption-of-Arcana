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

        List<GeneralCombat.SpellInstance> spells = new List<GeneralCombat.SpellInstance>();
        List<SpellBlock> spellBlocks = new List<SpellBlock>();
        public Object spellBlockPrefab;

        List<GeneralCombat.StatusInstance> statuses = new List<GeneralCombat.StatusInstance>();
        public float statusOffset = 0.3f;

        public Character player;
        ArcanaManager arcanaManager;
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
        public void AddSpellInstance(GeneralCombat.SpellInstance newSpellInstance)
        {
            spells.Add(newSpellInstance);
            CalculateTimeline();
        }

        /// <summary>
        /// Removes spell instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void RemoveSpellInstance(GeneralCombat.SpellInstance newSpellInstance)
        {
            spells.Remove(newSpellInstance);
            CalculateTimeline();
        }

        /// <summary>
        /// Adds status instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public bool AddStatusInstance(GeneralCombat.StatusInstance newStatusInstance)
        {
            bool apply = true;
            GeneralCombat.StatusInstance duplicate = new GeneralCombat.StatusInstance();
            foreach (GeneralCombat.StatusInstance status in statuses)
            {
                if (status.status == newStatusInstance.status && status.target == newStatusInstance.target)
                {
                    apply = false;
                    duplicate = status;

                    if (status.duration < newStatusInstance.duration)
                    {
                        duplicate = status;
                    }
                }
            }

            if (apply)
            {
                Debug.Log(newStatusInstance.status.effectName + " has been added");
                statuses.Add(newStatusInstance);
            }
            else if (duplicate.duration < newStatusInstance.duration)
            {
                duplicate.duration = newStatusInstance.duration;
            }

            return apply;
        }

        /// <summary>
        /// Removes status instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void RemoveStatusInstance(GeneralCombat.StatusInstance newStatusInstance)
        {
            GeneralCombat.StatusInstance remove = new GeneralCombat.StatusInstance();
            foreach (GeneralCombat.StatusInstance status in statuses)
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
            List<GeneralCombat.StatusInstance> newStatusList = new List<GeneralCombat.StatusInstance>(0);

            foreach (GeneralCombat.StatusInstance status in statuses)
            {
                GeneralCombat.StatusInstance instance = new GeneralCombat.StatusInstance();
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
            spells.Sort(SortBySpeed);
            arcanaCount = 0;

            //Clear old blocks that are no longer being cast
            foreach (var item in spellBlocks)
            {
                Destroy(item.gameObject);
            }

            spellBlocks.Clear();

            //Spawn UI for cards
            foreach (var item in spells)
            {
                string text = item.caster.characterName + " is casting " + item.spell.spellName + " on " + item.target.characterName + " (" + item.spell.speed + ")";

                //Creates spell block game object
                GameObject spellBlockObject = Instantiate(spellBlockPrefab) as GameObject;
                spellBlockObject.transform.SetParent(transform, false);

                //Sets spell block values
                SpellBlock spellBlock = spellBlockObject.GetComponent<SpellBlock>();
                spellBlock.text.text = text;
                if (item.spell.overrideColor)
                    spellBlock.image.color = item.spell.timelineColor;
                else
                    spellBlock.image.color = item.caster.timelineColor;

                //Adds spell block to layout group
                spellBlocks.Add(spellBlock);

                if (item.caster == player)
                {
                    arcanaCount += item.spell.arcanaCost;
                }
            }

            arcanaManager.CheckArcana(arcanaCount);
        }

        static int SortBySpeed(GeneralCombat.SpellInstance c1, GeneralCombat.SpellInstance c2)
        {
            return c1.spell.speed.CompareTo(c2.spell.speed);
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
            float delay = CastSpells();
            Invoke("ActivateStatuses", delay);
            delay += statuses.Count * statusOffset;

            return delay;
        }

        float CastSpells()
        {
            //Generates a delay for the entire set of spells being cast
            float delay = 0;

            if (spells.Count > 0)
            {
                delay = spells[spells.Count - 1].spell.speed;
            }

            //Loop through list and cast spell;
            foreach (var item in spells)
            {
                //Use a coroutine to stagger spellcasting
                StartCoroutine(IDelaySpell(item));
            }

            return delay;
        }

        void ActivateStatuses()
        {
            //Debug.Log("Activate statuses: " + statuses.Count);
            UpdateStatusDurations();

            //Generates a delay for the entire set of spells being cast
            float delay = 0;

            foreach (GeneralCombat.StatusInstance item in statuses)
            {
                StartCoroutine(IDelayStatus(item, delay));

                delay += statusOffset;
            }
        }

        public void HitStatuses(Character target, Character attacker)
        {
            foreach (GeneralCombat.StatusInstance item in statuses)
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
        IEnumerator IDelaySpell(GeneralCombat.SpellInstance spellInstance)
        {
            yield return new WaitForSeconds(spellInstance.spell.speed);

            //Debug.Log(spellInstance.caster.characterName + " played " + spellInstance.spell.spellName + " on " + spellInstance.target.characterName + " at time " + spellInstance.spell.speed);

            spellInstance.spell.CastSpell(spellInstance.target, spellInstance.caster);

            RemoveSpellInstance(spellInstance);
            CalculateTimeline();
        }

        IEnumerator IDelayStatus(GeneralCombat.StatusInstance statusInstance, float delay)
        {
            yield return new WaitForSeconds(delay);

            statusInstance.status.ActivateEffect(statusInstance.target);

            //Debug.Log("Activated " + statusInstance.status + " on " + statusInstance.target + " with " + statusInstance.duration + " turns remaining");

            if (statusInstance.duration <= 0)
            {
                //Debug.Log("Expired");
                statuses.Remove(statusInstance);
            }

            //RemoveStatusInstance(statusInstance);
            CalculateTimeline();
        }

        #endregion
    }
}