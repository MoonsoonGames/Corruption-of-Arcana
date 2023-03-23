using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EndTurn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Deck2D playerHandDeck; //Hand that player cards are drawn into
        public Deck2D potionDeck; //Hand that potion cards are drawn into
        public Spell[] potions;
        Deck2D[] decks;
        public GameObject cardPrefab; //Prefab of the parent card type
        Timeline timeline;
        TeamManager[] teamManagers;

        Button endTurnButton;
        public Color buttonAvailable = new Color(0, 0, 0, 255);
        public Color buttonUnavailable = new Color(0, 0, 0, 255);

        bool waitingForStartTurn = false;
        bool waitingForSound = false;

        public float endTurndelay = 2f;

        ArcanaManager arcanaManager;
        DeckTab deckTab;

        private void Start()
        {
            timeline = GameObject.FindObjectOfType<Timeline>();
            teamManagers = GameObject.FindObjectsOfType<TeamManager>();
            endTurnButton = GetComponent<Button>();
            endTurnButton.image.color = buttonAvailable;

            arcanaManager = timeline.GetArcanaManager();
            deckTab = GetComponentInParent<DeckTab>();

            Invoke("EndTurnButton", 0.1f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (waitingForStartTurn == false)
                {
                    DisableButton();
                    Debug.Log("End turn success");
                    EndTurnButton();
                }
            }
        }

        public void EndTurnButton()
        {
            DisableButton();
            DragManager.instance.canDrag = false;
            PressSound();

            deckTab.SelectHand();

            decks = GameObject.FindObjectsOfType<Deck2D>();
            foreach (Deck2D deck in decks)
            {
                if (deck != playerHandDeck)
                {
                    deck.RemoveAllCards(false);
                }
            }

            float delay = timeline.PlayTimeline() + endTurndelay;

            SetUIEnabled(false);
            Invoke("StartNextTurn", delay);
        }

        public void StartNextTurn()
        {
            SetUIEnabled(true);
            DragManager.instance.canDrag = true;
            //Debug.Log("New Turn");

            //Only give cards if player's hand isn't full
            if (playerHandDeck.CurrentCardsLength() < playerHandDeck.maxCards)
            {
                //Get the difference between the current and max cards to determine how many need to be drawn in
                float difference = playerHandDeck.maxCards - playerHandDeck.CurrentCardsLength();

                for (int i = 0; i < difference; i++)
                {
                    GameObject card = Instantiate(cardPrefab, playerHandDeck.transform) as GameObject;
                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    //Add the card to the array
                    playerHandDeck.AddCard(cardDrag);

                    //Reset card scales
                    cardDrag.ScaleCard(1, false);
                }

                //Spawn sound effect for cards
                PlayShuffleSound();
            }

            RedrawPotions();

            timeline.ActivateTurnModifiers();

            foreach (TeamManager manager in teamManagers)
            {
                manager.StartTurn();
            }
            DisableButton();
            waitingForStartTurn = true;
            Invoke("EnableButton", 2f);

            arcanaManager.CheckArcana(0);
        }

        void RedrawPotions()
        {
            potionDeck.RemoveAllCards(false);

            for (int i = 0; i < 4; i++)
            {
                if (PotionManager.instance.PotionAvailable(potions[i].potionType, potions[i].potionCost))
                {
                    GameObject card = Instantiate(cardPrefab, potionDeck.transform) as GameObject;
                    DrawCard drawCard = card.GetComponent<DrawCard>();
                    drawCard.draw = false;
                    drawCard.Setup(potions[i]);

                    CardDrag2D cardDrag = card.GetComponent<CardDrag2D>();

                    //Add the card to the array
                    potionDeck.AddCard(cardDrag);

                    //Reset card scales
                    cardDrag.ScaleCard(1, false);
                }
            }
        }

        void DisableButton()
        {
            waitingForStartTurn = true;
            endTurnButton.image.color = buttonUnavailable;
            endTurnButton.interactable = false;
        }

        void EnableButton()
        {
            if (waitingForStartTurn)
            {
                endTurnButton.interactable = true;
                endTurnButton.image.color = buttonAvailable;
                waitingForStartTurn = false;

                if (waitingForSound)
                {
                    PlayHoverSound();
                }
            }
        }

        #region Sound Effects

        public StudioEventEmitter hoverEmitter;
        public StudioParameterTrigger pressTrigger;
        public EventReference cardShuffle;

        void PlayShuffleSound()
        {
            RuntimeManager.PlayOneShot(cardShuffle);
        }

        void PlayHoverSound()
        {
            hoverEmitter.Play();
        }

        void StopHoverSound()
        {
            hoverEmitter.Stop();
        }

        void PressSound()
        {
            pressTrigger.TriggerParameters();
        }

        #endregion

        #region Pointer Events

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (endTurnButton.interactable)
            {
                PlayHoverSound();
            }
            else
            {
                waitingForSound = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHoverSound();
            waitingForSound = false;
        }

        #endregion

        #region UI

        public List<GameObject> disableUIElements;
        Dictionary<GameObject, Vector3> disableUIElementsDictionary = new Dictionary<GameObject, Vector3>();

        void SetupDictionary()
        {
            Deck2D[] decks = GameObject.FindObjectsOfType<Deck2D>();

            ClearMissing();

            foreach (var deck in decks)
            {
                bool duplicate = false;
                foreach (var item in disableUIElements)
                {
                    if (item != null)
                    {
                        if (item.GetComponentInChildren<Deck2D>() == deck)
                        {
                            Debug.Log("Found copy");
                            duplicate = true;
                        }
                    }
                }

                if (duplicate == false)
                    disableUIElements.Add(deck.gameObject);
            }

            foreach(var item in disableUIElements)
            {
                if (!disableUIElementsDictionary.ContainsKey(item))
                    disableUIElementsDictionary.Add(item, item.transform.position);
            }
        }

        void ClearMissing()
        {
            List<GameObject> newDisableUIElements = new List<GameObject>();

            foreach (var item in disableUIElements)
            {
                if (item != null)
                {
                    newDisableUIElements.Add(item);
                }
            }

            disableUIElements = newDisableUIElements;
        }

        void SetUIEnabled(bool enable)
        {
            SetupDictionary();
            foreach (var item in disableUIElementsDictionary)
            {
                if (item.Key != null)
                    item.Key.transform.position = enable ? item.Value : new Vector3(0, -500000000, 0);
            }
        }

        #endregion
    }
}