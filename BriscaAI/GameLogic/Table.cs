﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BriscaAI.GameLogic
{
    public class Table
    {
        public List<Card> PlayedCards { get; set; }
        public List<Card> CardHistory { get; set; }
        public Deck Deck { get; set; }

        public Table(Deck deck)
        {
            Deck = deck;
            PlayedCards = new List<Card>();
            CardHistory = new List<Card>();
        }


    }
}
