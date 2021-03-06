﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BriscaAI.GameLogic;
namespace BriscaAI.Agents
{
    public class HumanAgent : Player
    {
        public HumanAgent(string name) : base(name) { }

        private int valueSum(List<Card> played)
        {
            int vals = 0;
            foreach (var i in played)
            {
                vals += i.Value;
            }
            return vals;
        }

        public override void addPointsWon(List<Card> cardsWon)
        {
            PointsWon += valueSum(cardsWon);
        }

        public override Card PlayCard(int timeout, Table table)
        {
            Console.WriteLine($"Trumph card is: {table.Deck.TrumphCard}");

            Console.WriteLine("Cards Played up to now:");
            Helper.PrintCards(table.PlayedCards);

            Console.WriteLine($"\nPlay a card using # next to card: (Timeout in {(int)timeout/1000})");
            Helper.PrintCards(Hand);
            try
            {
                var play = "";
                Card card = null;
                while (string.IsNullOrEmpty(play))
                {
                    play = Reader.ReadLine();
                    if (play == "1")
                        card = Hand[0];
                    else if(play == "2")
                        card = Hand[1];
                    else if (play == "3")
                        card = Hand[2];
                    else
                        play = null;
                }
                Hand.Remove(card);
                Console.WriteLine($"Played: {card.ToString()}");
                return card;
            }
            catch (Exception e)
            {
                Console.WriteLine(Name + " has taken to long to play a card.");
                var card = Hand[0];
                Hand.Remove(card);
                Console.WriteLine($"Played: {card.ToString()}");
                return card;
            }
        }

        public override void RecieveCard(Card card)
        {
            if (Hand.Count == 3)
                throw new Exception();
            Hand.Add(card);
        }

       
    }
}
