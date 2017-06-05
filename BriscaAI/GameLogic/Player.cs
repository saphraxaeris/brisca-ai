using System;
using System.Collections.Generic;
using System.Text;

namespace BriscaAI.GameLogic
{
    public abstract class Player
    {
        protected string Name { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> CollectedCards { get; set; }

        protected Player(string name)
        {
            Name = name;
        }

    }
}
