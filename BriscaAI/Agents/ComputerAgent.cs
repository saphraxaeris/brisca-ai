using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BriscaAI.GameLogic;

namespace BriscaAI.Agents
{
    public class ComputerAgent : Player
    {

        public ComputerAgent(string name) : base(name) { }
        
        public override Card PlayCard(int timeout, Table table)
        {
            var played = table.PlayedCards;
            double points = 0;
            Card toPlay = null;

            removeOptions(played);
            removeOptions(table.CardHistory);
            var trump = table.Deck.TrumpSuit;
            int i = getWinner(played,trump);
            var playedCard = false;
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromMilliseconds(timeout) && !playedCard)
            {
                    foreach (var j in Hand)
                    {
                        //Timeout occurs must break out of loop
                        if (s.Elapsed < TimeSpan.FromMilliseconds(timeout))
                           break;

                        double tempPoints = 0;

                        if(i >= 0)
                        {
                            if (played[i].Suit == j.Suit && j.CompareTo(played[i]) > 0) { tempPoints += (j.Value + valueSum(played)); }
                            else if (played[i].Suit != j.Suit && j.Suit == trump) { tempPoints += (j.Value + valueSum(played)); }

                            tempPoints += (avgPointsWon(Options, trump, j, played[i]) * (3 - played.Count));
                        }
                        else
                        {
                            tempPoints += (avgPointsWon(Options, trump, j, null) * (3 - played.Count));
                        }

                        if (tempPoints > points) { points = tempPoints; toPlay = j; }
                    }
                    if(toPlay != null)
                        playedCard = true;
                }
            }
            s.Stop();

            if (!playedCard)
            {
                //Timeout occured, play first card in hand
                toPlay = Hand[0];
            }
            Hand.Remove(toPlay);
            return toPlay;
        }

        private double avgPointsWon(List<Card> options, Card.Suits trump, Card j,Card prevWinner == null)
        {
            bool isTrump = (j.Suit == trump);
            double points = 0;
            foreach (var i in options)
            {
                if(prevWinner != null)
                {
                    if (i.Suit != prevWinner.Suit && i.Suit != trump) { points += i.Value; }
                    else if (j.Suit == i.Suit && j.CompareTo(i) > 0) { points += i.Value; }
                }
                else
                {
                    if (j.Suit == i.Suit && j.CompareTo(i) > 0) { points += i.Value; }
                }
            }

            return points / options.Count;
        }

        private int valueSum(List<Card> played)
        {
            int vals = 0;
            foreach (var i in played) {
                vals += i.Value;
            }
            return vals;
        }

        private int getWinner(List<Card> roundCards,Card.Suits lifeSuit)
        {
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

        private void removeOptions(List<Card> cards)
        {
            foreach (var i in cards)
            {
                foreach (var j in Options)
                {
                    if (i.CompareTo(j) == 0) {
                        Options.Remove(j);
                        break;
                    }
                }
            }
        }

        public override void RecieveCard(Card card)
        {
            if (Hand.Count == 2) { return; }
            Hand.Add(card);
        }

        public override void addPointsWon(List<Card> cardsWon)
        {
            PointsWon += valueSum(cardsWon);
        }
    }
}
