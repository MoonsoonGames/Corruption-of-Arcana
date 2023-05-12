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
    public class Collectable : MonoBehaviour, IInteractable
    {
        public Object collectable;
        public SpriteRenderer sprite;
        public string ID;

        private void Start()
        {
            Setup();
        }

        [ContextMenu("Setup")]
        private void Setup()
        {
            if (collectable.GetType() == typeof(Weapon))
            {
                // Add to deck manager list
                Weapon weapon = (Weapon)collectable;
                name = "Collectable (" + weapon.weaponName + ")";
                sprite.sprite = weapon.image;
            }
            else if (collectable.GetType() == typeof(Spell))
            {
                Spell currentSpell = (Spell)collectable;
                name = "Collectable (" + currentSpell.spellName + ")";
                switch (currentSpell.cardType)
                {
                    case E_CardTypes.Cards:
                        sprite.sprite = currentSpell.cardImage;
                        break;
                    case E_CardTypes.Potions:
                        sprite.sprite = currentSpell.nameImage;
                        break;
                }
            }
            else if (collectable.GetType() == typeof(Curios_Object))
            {
                Curios_Object curio = (Curios_Object)collectable;
                name = "Collectable (" + curio.Name + ")";
                sprite.sprite = curio.Artwork;

                gameObject.SetActive(!curio.isCollected);
            }
        }

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
        {
            if (collectable != null)
            {
                if (collectable.GetType() == typeof(Weapon))
                {
                    // Add to deck manager list
                    DeckManager.instance.unlockedWeapons.Add((Weapon)collectable);
                }
                else if (collectable.GetType() == typeof(Spell))
                {
                    Spell currentSpell = (Spell)collectable;
                    name = "Collectable (" + currentSpell.spellName + ")";
                    switch (currentSpell.cardType)
                    {
                        case E_CardTypes.Cards:
                            // Add to deck manager list
                            DeckManager.instance.collection.Add(currentSpell);
                            break;
                        case E_CardTypes.Potions:
                            PotionManager.instance.ChangePotion(currentSpell.potionType, 1);
                            break;
                    }
                }
                else if (collectable.GetType() == typeof(Curios_Object))
                {
                    Curios_Object curio = (Curios_Object)collectable;
                    curio.isCollected = true;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
