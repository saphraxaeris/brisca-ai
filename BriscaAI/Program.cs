using System;
using System.Collections.Generic;
using System.Threading;
using BriscaAI.Agents;
using BriscaAI.GameLogic;

namespace BriscaAI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var players = new List<Player>();
            players.Add(new ComputerAgent("AI 1", 30000));
            players.Add(new ComputerAgent("AI 2", 30000));
            var DM = new BriscaDM(players, Deck.DeckCapacity.Forty);
            DM.Timeout = 10000;
            DM.StartGame();
            Console.ReadKey();
        }
    }
}