using System;
using System.Collections.Generic;
using System.Text;
using BriscaAI.GameLogic;

namespace BriscaAI.Agents
{
    public class ComputerAgent : Player
    {
        public ComputerAgent(string name) : base(name) { }

        public override Card PlayCard(int timeout, Table table)
        {
            throw new NotImplementedException();
        }

        public override void RecieveCard(Card card)
        {
            throw new NotImplementedException();
        }

        public override bool WillMulligan()
        {
            throw new NotImplementedException();
        }
    }
}
