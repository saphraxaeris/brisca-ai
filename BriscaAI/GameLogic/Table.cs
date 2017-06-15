using System;
using System.Collections.Generic;
using System.Text;

namespace BriscaAI.GameLogic
{
    public class Table
    {
        public List<Card> PlayedCards { get; set; }
        public List<Card> CardHistory { get; set; }
        public Deck Deck { get; set; }
        public int Players { get; set; }
        public Table(Deck deck, int players)
        {
            Deck = deck;
            PlayedCards = new List<Card>();
            CardHistory = new List<Card>();
            Players = players;
        }
    }
}
