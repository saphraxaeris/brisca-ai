using System;
using System.Collections.Generic;
using System.Text;

namespace BriscaAI.GameLogic
{
    public class Card : IComparable
    {
        public enum Suits { Gold, Club, Sword, Cup }

        public int Value { get; set; }
        private int _number { get; set; }
        public int Number {
            get { return _number; }
            set
            {
                _number = value;
                switch (value)
                {
                    case (1):
                        Value = 11;
                        break;

                    case (3):
                        Value = 10;
                        break;

                    case (10):
                        Value = 2;
                        break;

                    case (11):
                        Value = 3;
                        break;

                    case (12):
                        Value = 4;
                        break;

                    default:
                        Value = 0;
                        break;
                }
            }
        }
        public Suits Suit { get; set; }

        public override string ToString()
        {
            var suit = "";
            switch (Suit)
            {
                case Suits.Gold:
                    suit = "Gold";
                    break;
                case Suits.Club:
                    suit = "Club";
                    break;
                case Suits.Sword:
                    suit = "Sword";
                    break;
                case Suits.Cup:
                    suit = "Cup";
                    break;
            }
            return $"{suit} - {Number}";
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;
            var otherCard = obj as Card;
            if (otherCard == null)
                return -1;
            if (Value < otherCard.Value)
                return 1;
            else if (Value > otherCard.Value)
                return -1;
            else
            {
                if(_number < otherCard.Number)
                    return 1;
                else if (_number > otherCard.Number)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
