using System;
using System.Collections.Generic;
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

            if (Mulligan)
            {
                //Ask each player for mulligan
                foreach (var player in _players)
                {
                    if (player.WillMulligan())
                        player.Hand = _table.Deck.Mulligan(player.Hand);
                }
            }

            //Start Rounds
            while (CountCards() > 0)
            {
                PlayRound();
            }

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
            _players[_firstPlayerIndex].WonCards.AddRange(_table.PlayedCards);

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
            throw new NotImplementedException();
        }

        private void ShowPlayerRanking()
        {
            throw new NotImplementedException();
        }
    }
}
