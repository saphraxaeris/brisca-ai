using System;
using System.Collections.Generic;
using System.Text;

namespace BriscaAI.GameLogic
{
    public abstract class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Options { get; set; }
        public int PointsWon { get; set; }

        protected Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            PointsWon = 0;
            Options = new Deck(0).Cards;
        }

        public abstract Card PlayCard(int timeout, Table table);
        public abstract void RecieveCard(Card card);
        public abstract void addPointsWon(List<Card> cardsWon);
    }
}
