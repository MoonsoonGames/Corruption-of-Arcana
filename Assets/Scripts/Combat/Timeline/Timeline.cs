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

        public static Timeline instance = new Timeline();

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
        public void AddStatusInstance(GeneralCombat.StatusInstance newStatusInstance)
        {
            statuses.Add(newStatusInstance);
            CalculateTimeline();
        }

        /// <summary>
        /// Removes status instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void RemoveStatusInstance(GeneralCombat.StatusInstance newStatusInstance)
        {
            statuses.Remove(newStatusInstance);
            CalculateTimeline();
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

        float ActivateStatuses()
        {
            //Generates a delay for the entire set of spells being cast
            float delay = 0;

            for (int i = 0; i < spells.Count; i++)
            {
                StartCoroutine(IDelayStatus(statuses[i], i * statusOffset));
            }

            return delay;
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

            //Debug.Log(spellInstance.caster.characterName + " played " + spellInstance.spell.spellName + " on " + spellInstance.target.characterName + " at time " + spellInstance.spell.speed);

            statusInstance.status.ActivateEffect(statusInstance.target, null);

            RemoveStatusInstance(statusInstance);
            CalculateTimeline();
        }

        #endregion
    }
}