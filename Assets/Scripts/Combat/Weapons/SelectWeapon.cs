using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SelectWeapon : MonoBehaviour
    {
        public Weapon previewWeapon;

        public TextMeshProUGUI nameText, descriptionText;
        public Image image;

        public GameObject equipButton, equippedText;

        public Deck2D deck;

        public Object cardPrefab;

        private void OnEnable()
        {
            if (DeckManager.instance.weapon != null)
                PreviewWeapon(DeckManager.instance.weapon);
        }

        public void PreviewWeapon(Weapon weapon)
        {
            if (deck.CurrentCardsLength() > 0)
                deck.RemoveAllCards(false);

            previewWeapon = weapon;

            nameText.text = weapon.weaponName;
            descriptionText.text = weapon.description;
            image.sprite = previewWeapon.image;
            image.color = previewWeapon == null || previewWeapon.image == null ? new Color(0, 0, 0, 0) : new Color(255, 255, 255, 255);

            if (weapon.spells.Count <= 10)
            {
                foreach (var item in weapon.spells)
                {
                    if (item == null)
                        break;

                    Debug.Log(item.spellName);
                    GameObject card = Instantiate(cardPrefab, transform.GetChild(0).transform) as GameObject;
                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();
                    DrawCard drawCard = card.GetComponent<DrawCard>();

                    drawCard.draw = false;
                    drawCard.Setup(item);

                    deck.AddCard(cardDrag);

                    //Reset card scales
                    //cardDrag.transform.localScale *= 0.6f;
                    cardDrag.Setup(cardDrag.GetComponent<Card>());
                    cardDrag.ScaleCard(1, false);
                }
            }

            //enable equip button or equip text if it is currently equipped
            equippedText.SetActive(DeckManager.instance.weapon == weapon);
            equipButton.SetActive(DeckManager.instance.weapon != weapon);
        }

        public void EquipWeapon()
        {
            if (previewWeapon == null) { return; }

            previewWeapon.Equip();

            PreviewWeapon(previewWeapon);
        }
    }
}
