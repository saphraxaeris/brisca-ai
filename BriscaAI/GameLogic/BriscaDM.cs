using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace BriscaAI.GameLogic
{
    public class BriscaDM
    {
        private readonly List<Player> _players;
        private readonly Table _table;
        private int _firstPlayerIndex;

        public int Timeout { get; set; }
        public bool Mulligan { get; set; }

        public BriscaDM(List<Player> players, Deck.DeckCapacity deckCapacity)
        {
            //Initialize players and table
            _players = players;
            _firstPlayerIndex = 0;
            _table = new Table(new Deck(deckCapacity));
        }

        public void StartGame()
        {
            //Set default timeout if not stated
            //Default: 10 seconds
            if (Timeout == 0)
                Timeout = 10000;

            //Deal hand for each player
            for (int i = 0; i < 3; i++)
            {
                foreach (var player in _players)
                {
                    player.RecieveCard(_table.Deck.DealCard());
                }
            }

            //Set trumph card
            _table.Deck.SetTrumphCard();


            //Start Rounds
            while (CountCards() > 0)
            {
                PlayRound();
            }

            Console.WriteLine("\nGame Ended!\n");

            ShowPlayerRanking();
        }

        private int CountCards()
        {
            //Count cards in each player's hand
            var playerCards = 0;
            foreach (var player in _players)
            {
                playerCards += player.Hand.Count;
            }

            //return playerCards + cards available in deck
            return _table.Deck.Cards.Count + playerCards;
        }

        private void PlayRound()
        {
            //Each player plays their card
            for (int i = 0; i < _players.Count; i++)
            {
                _table.PlayedCards.Add(_players[(i+ _firstPlayerIndex) % _players.Count].PlayCard(Timeout, _table));
            }

            //Select round winner and set as first player of next round
            _firstPlayerIndex = SelectRoundWinner(_table.PlayedCards);
            _players[_firstPlayerIndex].addPointsWon(_table.PlayedCards);
            _table.CardHistory.AddRange(_table.PlayedCards);

            //Reset Played Cards
            _table.PlayedCards.Clear();

            //Deal card to player in correct order
            for (int i = 0; i < _players.Count; i++)
            {
                _players[(i + _firstPlayerIndex) % _players.Count].RecieveCard(_table.Deck.DealCard());
            }
        }

        private int SelectRoundWinner(List<Card> roundCards)
        {
            var lifeSuit = _table.Deck.TrumphCard().Suit;
            var lifeCount = 0;

            //Count cards with same suit as trumph card
            foreach (var card in roundCards)
            {
                if (card.Suit == lifeSuit)
                    lifeCount++;
            }

            if (lifeCount == 1)
            {
                //No contest
                for (int i = 0; i < roundCards.Count; i++)
                {
                    if (roundCards[i].Suit == lifeSuit)
                    {
                        return i;
                    }
                }
            }
            else if (lifeCount == 0)
            {
                //First player suit becomes life suit
                lifeSuit = roundCards[0].Suit;
            }

            //Check which life card would win
            //Get eligible cards for winning
            var eligibleCards = new List<Card>();
            foreach (var card in roundCards)
            {
                if (card.Suit == lifeSuit)
                    eligibleCards.Add(card);
            }

            //Sort cards
            eligibleCards.Sort();

            //Card at position 0 wins
            //Find player that owns card
            for (int i = 0; i < roundCards.Count; i++)
            {
                if (roundCards[i] == eligibleCards[0])
                {
                    return i;
                }
            }

            return -1;
        }

        private void ShowPlayerRanking()
        {
            var ranked = new List<Player>();
            while (_players.Count > 0)
            {
                Player maxPlayer = null;
                int maxScore = 0;
                foreach (var player in _players)
                {
                    int score = player.PointsWon;
                    if (maxScore < score)
                    {
                        maxPlayer = player;
                        maxScore = score;
                    }
                }
                _players.Remove(maxPlayer);
                ranked.Add(maxPlayer);
            }

            Console.WriteLine("\n\nPlayer Rankning:\n");
            for (int i = 0; i < ranked.Count; i++)
            {
                Console.WriteLine($"#{i+1} {ranked[i].Name} - {ranked[i].PointsWon} points\n");
            }
        }

        private int GetPlayerScore(List<Card> cards)
        {
            var score = 0;
            foreach (var card in cards)
            {
                score += card.Value;
            }
            return score;
        }
    }
}
