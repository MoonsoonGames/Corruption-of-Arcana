using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class CombatManager : MonoBehaviour
    {
        public int currentTurn = 0;

        public Character player;
        public TeamManager playerTeamManager;
        public TeamManager enemyTeamManager;
        public Character redirectedCharacter;

        public GameObject victoryScreen;
        public Object rewardItem;
        public GameObject defeatScreen;

        public static CombatManager instance;

        public Image backdropImage;
        public GameObject loadingImage;

        protected void Start()
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            loadingImage.SetActive(true);

            Invoke("Setup", 0.1f);
        }

        protected void Setup()
        {
            loadingImage.SetActive(false);

            if (LoadCombatManager.instance.backdrop != null)
            {
                backdropImage.sprite = LoadCombatManager.instance.backdrop;
                LoadCombatManager.instance.backdrop = null;
            }

            DeckManager.instance.SetupDecks();
        }

        public void CharacterDied(Character character)
        {
            if (LoadCombatManager.instance.enemiesEndCombat.Contains(character.stats))
            {
                LoadCombatManager.instance.enemiesEndCombat.Remove(character.stats);
                if (LoadCombatManager.instance.enemiesEndCombat.Count <= 0)
                {
                    foreach(var enemy in enemyTeamManager.team)
                    {
                        enemy.GetHealth().ChangeHealth(E_DamageTypes.Physical, 99999999, null);
                    }
                    ShowEndScreen(true);
                }
            }

            if (redirectedCharacter == character)
            {
                redirectedCharacter = null;
            }

            //Debug.Log("Character Killed");
            if (playerTeamManager.team.Contains(character))
            {
                //Debug.Log("Character Killed on player team");
                playerTeamManager.Remove(character);
                if (playerTeamManager.team.Count == 0)
                {
                    ShowEndScreen(false);
                }
            }
            else
            {
                //Debug.Log("Character Killed on enemy team");
                enemyTeamManager.Remove(character);
                killedEnemies.Add(character.stats);
                if (enemyTeamManager.team.Count + LoadCombatManager.instance.enemies.Count == 0)
                {
                    ShowEndScreen(true);
                }
            }
        }

        protected void ShowEndScreen(bool victory)
        {
            victoryScreen.SetActive(victory);
            defeatScreen.SetActive(!victory);

            if (victory)
                GiveRewards();
        }

        List<CharacterStats> killedEnemies = new List<CharacterStats>();

        void GiveRewards()
        {
            GridLayoutGroup grid = victoryScreen.GetComponentInChildren<GridLayoutGroup>();

            foreach(CharacterStats stats in killedEnemies)
            {
                List<Object> enemyRewards = stats.GiveRewards();

                if (rewardItem != null)
                {
                    foreach(Object item in enemyRewards)
                    {
                        GameObject rewardObj = Instantiate(rewardItem, grid.transform) as GameObject;

                        rewardObj.GetComponent<RewardItem>().Setup(item);
                    }
                }
            }
        }

        public TeamManager GetCharacterTeam(TeamManager teamManager)
        {
            TeamManager outTeam = null;
            if (teamManager == playerTeamManager)
                outTeam = enemyTeamManager;
            else if (teamManager == enemyTeamManager)
                outTeam = playerTeamManager;
            return outTeam;
        }

        public TeamManager GetOpposingTeam(TeamManager teamManager)
        {
            TeamManager outTeam = null;
            if (teamManager == playerTeamManager)
                outTeam = enemyTeamManager;
            else if (teamManager == enemyTeamManager)
                outTeam = playerTeamManager;
            return outTeam;
        }

        public List<Character> GetAllCharacters()
        {
            List<Character> list = new List<Character>();

            foreach (var item in playerTeamManager.team)
            {
                list.Add(item);
            }

            foreach (var item in enemyTeamManager.team)
            {
                list.Add(item);
            }

            return list;
        }

        public Deck2D playerHandDeck; //Hand that player cards are drawn into
        public Deck2D potionDeck; //Hand that potion cards are drawn into

        protected Deck2D[] decks;

        public virtual bool CanEndTurn()
        {
            return true;
        }

        public virtual float EndTurn(float endTurnDelay)
        {
            decks = GameObject.FindObjectsOfType<Deck2D>();
            foreach (Deck2D deck in decks)
            {
                if (deck != playerHandDeck)
                {
                    deck.RemoveAllCards(false);
                }
            }

            float delay = Timeline.instance.PlayTimeline() + endTurnDelay;

            currentTurn++;
            Invoke("StartNextTurn", delay);
            return delay;
        }

        public virtual void StartNextTurn()
        {
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

            Timeline.instance.ActivateTurnModifiers();

            playerTeamManager.StartTurn(); enemyTeamManager.StartTurn();
        }

        public GameObject cardPrefab; //Prefab of the parent card type
        public Spell[] potions;

        protected void RedrawPotions()
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

        #region Sound Effects

        public EventReference cardShuffle;

        protected void PlayShuffleSound()
        {
            RuntimeManager.PlayOneShot(cardShuffle);
        }

        #endregion
    }
}