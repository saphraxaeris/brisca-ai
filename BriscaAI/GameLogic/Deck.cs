using System.Collections.Generic;

namespace BriscaAI.GameLogic
{
    public class Deck
    {
        public enum DeckCapacity { Forty, FortyEight }
        public List<Card> Cards { get; set; }
        public Card.Suits TrumphSuit;
        public Card TrumphCard;
        public Deck(DeckCapacity size)
        {
            var capacity = (size == DeckCapacity.Forty) ? 40 : 48;

            Cards = new List<Card>();

            //Set Golds
            for (int i = 1; i < 13; i++)
            {
                if (capacity == 40)
                {
                    if (i == 7 || i == 8)
                        continue;
                }
                Cards.Add(new Card { Number = i, Suit = Card.Suits.Gold });
            }

            //Set Clubs
            for (int i = 1; i < 13; i++)
            {
                if (capacity == 40)
                {
                    if (i == 7 || i == 8)
                        continue;
                }
                Cards.Add(new Card { Number = i, Suit = Card.Suits.Club });
            }

            //Set Swords
            for (int i = 1; i < 13; i++)
            {
                if (capacity == 40)
                {
                    if (i == 7 || i == 8)
                        continue;
                }
                Cards.Add(new Card { Number = i, Suit = Card.Suits.Sword });
            }

            //Set Cups
            for (int i = 1; i < 13; i++)
            {
                if (capacity == 40)
                {
                    if (i == 7 || i == 8)
                        continue;
                }
                Cards.Add(new Card { Number = i, Suit = Card.Suits.Cup });
            }

            //Shuffle Deck
            Cards = Helper.Shuffle<Card>(Cards);
        }

        public Card DealCard()
        {
            if (Cards.Count == 0)
                return null;

            //Remove top card from deck
            var card = Cards[Cards.Count-1];
            Cards.Remove(card);
            return card;
        }

        public List<Card> Mulligan(List<Card> hand)
        {
            //Save trumph card
            var trumphCard = Cards[0];
            Cards.Remove(trumphCard);

            //Insert hand into deck
            foreach (var card in hand)
            {
                Cards.Add(card);
            }

            //Shuffle
            Cards = Helper.Shuffle(Cards);

            //Re-insert trumph card
            Cards.Insert(0, trumphCard);

            //Get new Hand
            var newHand = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                var card = DealCard();
                if(card != null)
                    newHand.Add(card);
            }

            return newHand;
        }

        public Card.Suits getTrumpSuit() {
            return TrumphSuit;
        }

        public void SetTrumphCard()
        {
            var card = Cards[Cards.Count - 1];
            Cards.Remove(card);
            Cards.Insert(0, card);
            TrumphSuit = card.Suit;
            TrumphCard = card;
        }

        public Card SwitchTrumphCard(Card card)
        {
            //Get and remove old trumph card
            var oldTrumphCard = Cards[0];
            Cards.Remove(oldTrumphCard);

            //Insert new Trumph Card
            Cards.Insert(0, card);

            return oldTrumphCard;
        }
    }
}
