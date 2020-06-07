using System;

namespace Game.CardSystem.Model
{
    [Serializable]
    public class CardData : IComparable<CardData>
    {
        public CardType CardType;
        public CardValue CardValue;

        public CardData()
        {
        }
        public CardData(CardType cardType, CardValue cardCurveValue)
        {
            CardType = cardType;
            CardValue = cardCurveValue;
        }

        public int CompareTo(CardData other)
        {
            if (this.CardType == other.CardType && this.CardValue.Value == other.CardValue.Value &&
                this.CardValue.View == other.CardValue.View)
                return 1;

            return 0;
        }
    }
    
    [Serializable]
    public class CardValue
    {
        public string View;
        public int Value;
        public string Portrait;

        public CardValue(string view, int value, string portrait = "")
        {
            View = view;
            Value = value;
            Portrait = portrait;
        }
    }
    
    public enum CardType
    {
        Clubs = 1,
        Diamonds = 2,
        Spades = 3,
        Hearts = 4

    }
}
