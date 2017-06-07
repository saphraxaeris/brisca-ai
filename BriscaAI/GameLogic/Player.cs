using System;
using System.Collections.Generic;
using System.Text;

namespace BriscaAI.GameLogic
{
    public abstract class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> WonCards { get; set; }

        protected Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            WonCards = new List<Card>();
        }

        public abstract Card PlayCard(int timeout, Table table);
        public abstract void RecieveCard(Card card);
        public abstract bool WillMulligan(int timeout);
    }
}
