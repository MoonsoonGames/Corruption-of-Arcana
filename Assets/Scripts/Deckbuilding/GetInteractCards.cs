using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Interactable;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class GetInteractCards : MonoBehaviour
    {
        public GameObject content, interactContent;
        public Object cardPrefab;

        public List<Spell> requireSpells { get; private set; }
        public E_Operations operation = E_Operations.AND;
        public Interactable.Interactable interactable;

        public void LoadCards(List<Spell> spells, E_Operations operation, Interactable.Interactable interactable)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            requireSpells = spells;
            this.operation = operation;
            this.interactable = interactable;

            DeckManager.instance.LoadDeck();

            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < interactContent.transform.childCount; i++)
            {
                Destroy(interactContent.transform.GetChild(i).gameObject);
            }

            if (content != null)
            {
                List<Spell> allSpells = HelperFunctions.CombineLists<Spell>(DeckManager.instance.collection, DeckManager.instance.majorArcana);
                StartCoroutine(SetupDeck(content, allSpells, 0.05f));
            }
        }

        IEnumerator SetupDeck(GameObject content, List<Spell> collection, float delay)
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

            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }

            yield return new WaitForSeconds(delay);

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
            }
        }

        public void SuccessfulInteract()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            GameObject player = GameObject.FindObjectOfType<Necropanda.Player.PlayerController>().gameObject;
            interactable.CheckInteraction(player);

            gameObject.SetActive(false);
        }
    }
}
