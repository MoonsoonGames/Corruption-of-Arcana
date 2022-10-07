using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    List<KeyValuePair<Spell, Character>> spells = new List<KeyValuePair<Spell, Character>>();
    List<SpellBlock> spellBlocks = new List<SpellBlock>();
    public Object spellBlockPrefab;

    public void AddCard(Spell newCard, Character target)
    {
        spells.Add(new KeyValuePair<Spell, Character>(newCard, target));
        SortCards();
        CalculateTimeline();
    }

    public void RemoveCard(Spell newCard, Character target)
    {
        spells.Remove(new KeyValuePair<Spell, Character>(newCard, target));
        SortCards();
        CalculateTimeline();
    }

    void CalculateTimeline()
    {
        //Clear old blocks
        foreach (var item in spellBlocks)
        {
            Destroy(item.gameObject);
        }

        spellBlocks.Clear();

        //Spawn UI for cards
        foreach (var item in spells)
        {
            string text = item.Key.spellName + " on " + item.Value.characterName + " (" + item.Key.speed + ")";

            GameObject spellBlockObject = Instantiate(spellBlockPrefab) as GameObject;
            spellBlockObject.transform.SetParent(transform, false);

            SpellBlock spellBlock = spellBlockObject.GetComponent<SpellBlock>();
            spellBlock.text.text = text;
            spellBlock.image.color = item.Key.timelineColor;
            spellBlocks.Add(spellBlock);
        }
    }

    public void SortCards()
    {
        List<KeyValuePair<Spell, Character>> orderedList = new List<KeyValuePair<Spell, Character>>();

        foreach (var item in spells)
        {
            orderedList.Add(new KeyValuePair<Spell, Character>(item.Key, item.Value));
        }

        orderedList.Sort(SortBySpeed);

        spells.Clear();

        foreach (var item in orderedList)
        {
            spells.Add(new KeyValuePair<Spell, Character>(item.Key, item.Value));
            //Debug.Log(item.Key.spellName + item.Key.speed);
        }
    }

    static int SortBySpeed(KeyValuePair<Spell, Character> c1, KeyValuePair<Spell, Character> c2)
    {
        return c1.Key.speed.CompareTo(c2.Key.speed);
    }

    public void CastSpells()
    {
        //Loop through list and cast spell;
        foreach (var item in spells)
        {
            //Insert delay for each card
            Debug.Log("Played " + item.Key.spellName + " on " + item.Value.characterName + " at " + item.Key.speed);

            item.Key.CastSpell(item.Value, null);
        }
        spells.Clear();
        CalculateTimeline();
    }
}
