using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BriscaAI.GameLogic;

namespace BriscaAI.Agents
{
    public class ComputerAgent : Player
    {
        private int _iterations = 10000;

        public ComputerAgent(string name, int? iterations = null) : base(name)
        {
            if (iterations != null)
                _iterations = iterations.Value;
        }

        //From https://www.dotnetperls.com/fisher-yates-shuffle
        static Random _random = new Random();
        static void Shuffle<T>(List<T> array)
        {
            int n = array.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public override Card PlayCard(int timeout, Table table)
        {
            Console.WriteLine(Name + "'s Hand:");
            Helper.PrintCards(Hand);
            var played = table.PlayedCards;
            double points = 0;
            Card toPlay = null;

            removeOptions(played);
            removeOptions(table.CardHistory);
            removeOptions(Hand);
            //Console.WriteLine(Options.Count+" Options left");
            var trump = table.Deck.TrumphSuit;
            int i = getWinner(played, trump);
            var playedCard = false;
            Stopwatch s = new Stopwatch();
            s.Start();
            //while (s.Elapsed < TimeSpan.FromMilliseconds(timeout) && !playedCard)
            //{
            foreach (var j in Hand)
            {
                if (j == null)
                    continue;
                //Timeout occurs must break out of loop
                double tempPoints = 0;
                List<Card> passerHand = new List<Card>();
                passerHand.AddRange(Hand);
                passerHand.Remove(j);
                Shuffle(Options);
                tempPoints += staticMonteCarlo(passerHand, played, Options, _iterations, table.Players, trump, j);


                if (tempPoints >= points) { points = tempPoints; toPlay = j; }
            }
            if (toPlay != null)
                playedCard = true;
            //}
            s.Stop();

            if (!playedCard)
            {
                //Timeout occured, play first card in hand
                Console.WriteLine(Name + " has taken to long to play a card.");
                toPlay = Hand[0];
            }
            Hand.Remove(toPlay);
            return toPlay;
        }

        private double staticMonteCarlo(List<Card> passerHand, List<Card> played, List<Card> options, int iterations, int players, Card.Suits trump, Card j)
        {

            double points = 0.0;
            for (int i = 0; i < iterations; i++)
            {
                List<Card> cPlayed = new List<Card>();
                cPlayed.AddRange(played);
                List<Card> tempOptions = new List<Card>(options);
                //Finish started round
                int index = cPlayed.Count;
                cPlayed.Add(j);
                while (cPlayed.Count < players) { cPlayed.Add(tempOptions[0]); tempOptions.RemoveAt(0); }
                int win = getWinner(cPlayed, trump);
                if (win == index) { points += (valueSum(cPlayed)); }
                cPlayed.Clear();


                if (tempOptions.Count > 0)
                {
                    List<Card>[] playersCards = new List<Card>[players];
                    for (int k = 0; k < players; k++)
                    {
                        playersCards[k] = new List<Card>();
                    }
                    playersCards[0].AddRange(passerHand);
                    int cap = (tempOptions.Count + passerHand.Count) / players;

                    for (int k = 0; k < players; k++)
                    {
                        while (playersCards[k].Count < cap) { playersCards[k].Add(tempOptions[0]); tempOptions.RemoveAt(0); }
                    }
                    win = 0;
                    while (playersCards[0].Count > 0)
                    {
                        for (int k = 0; k < players; k++)
                        {
                            int val = (win + k) % players;
                            int r = (int)(_random.NextDouble() * playersCards[val].Count);
                            cPlayed.Add(playersCards[val][r]);
                            playersCards[val].RemoveAt(r);
                        }
                        index = getWinner(cPlayed, trump);
                        if ((index + win) % players == 0) { points += (valueSum(cPlayed)); }
                        cPlayed.Clear();
                        win = index;
                    }
                }
            }


            return points / iterations;
        }

        private void removeTempOptions(List<Card> played, List<Card> tempOptions)
        {
            foreach (var i in played)
            {
                foreach (var j in tempOptions)
                {
                    if (i.CompareTo(j) == 0 && i.Suit == j.Suit)
                    {
                        tempOptions.Remove(j);
                        break;
                    }
                }
            }
        }

        private double avgPointsWon(List<Card> options, Card.Suits trump, Card j, Card prevWinner = null)
        {
            bool isTrump = (j.Suit == trump);
            double points = 0;
            foreach (var i in options)
            {
                if (prevWinner != null)
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
            foreach (var i in played)
            {
                vals += i.Value;
            }
            return vals;
        }

        private int getWinner(List<Card> roundCards, Card.Suits lifeSuit)
        {
            if (roundCards.Count == 0)
                return -1;

            var allNull = true;

            foreach (var card in roundCards)
            {
                if (card != null)
                    allNull = false;
            }

            if (allNull)
                return -1;

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
            else if (lifeCount > 1)
            {
                int win = -1;
                Card temp = null;
                for (int i = 0; i < roundCards.Count; i++)
                {
                    if (roundCards[i].Suit == lifeSuit)
                    {
                        if (win == -1) { win = i; temp = roundCards[i]; }
                        else if (roundCards[i].CompareTo(temp) > 0) { win = i; temp = roundCards[i]; }
                    }
                }

                return win;

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
                    if (i.CompareTo(j) == 0 && i.Suit == j.Suit)
                    {
                        Options.Remove(j);
                        break;
                    }
                }
            }
        }

        public override void RecieveCard(Card card)
        {
            if (Hand.Count == 3)
                throw new Exception();
            if (card != null)
                Hand.Add(card);
        }

        public override void addPointsWon(List<Card> cardsWon)
        {
            PointsWon += valueSum(cardsWon);
        }
    }
}
