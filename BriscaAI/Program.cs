using System;
using BriscaAI.GameLogic;

namespace BriscaAI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var Deck = new Deck(GameLogic.Deck.DeckCapacity.FortyEight);
            Console.WriteLine("Hello World!");
        }
    }
}