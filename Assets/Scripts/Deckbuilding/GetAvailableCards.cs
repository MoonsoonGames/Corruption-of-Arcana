using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class GetAvailableCards : MonoBehaviour
    {
        public GameObject collectionContent, equipContent;
        BuildDeck buildDeck;
        public Object cardPrefab;

        public void LoadCards()
        {
            DeckManager.instance.LoadDeck();

            buildDeck = GetComponent<BuildDeck>();

            if (collectionContent != null)
            {
                buildDeck.collectedSpells = new List<Spell>();
                StartCoroutine(SetupDeck(collectionContent, DeckManager.instance.collection, buildDeck.collectedSpells, 0.05f));
            }

            if (equipContent != null)
            {
                buildDeck.equippedSpells = new List<Spell>();
                StartCoroutine(SetupDeck(equipContent, DeckManager.instance.majorArcana, buildDeck.equippedSpells, 0.05f));
            }
        }

        IEnumerator SetupDeck(GameObject content, List<Spell> collection, List<Spell> buildDeckSpells, float delay)
        {
            Debug.Log("Open menu");
            if (content == null)
            {
                Debug.Log("Null content");
                yield break;
            }
            Deck2D collectionDeck = content.GetComponentInParent<Deck2D>();

            List<Spell> collectionCopy = new List<Spell>();

            foreach (var item in collection)
                collectionCopy.Add(item);

            buildDeckSpells.Clear();

            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }

            yield return new WaitForSecondsRealtime(delay);

            foreach (Spell spell in collectionCopy)
            {
                Debug.Log(spell.spellName + " should be in collection");
                GameObject card = Instantiate(cardPrefab, content.transform) as GameObject;
                CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
                DrawCard drawCard = card.GetComponent<DrawCard>();
                //Add the card to the array
                collectionDeck.AddCard(cardDrag);

                //Reset card scales
                cardDrag.ScaleCard(1, false);

                drawCard.draw = false;
                drawCard.Setup(spell);

                buildDeckSpells.Add(spell);
            }
        }
    }
}
