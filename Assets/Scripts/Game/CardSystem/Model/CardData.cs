using System;

namespace Game.CardSystem.Model
{
    [Serializable]
    public class CardData : IEquatable<CardData>
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
        
        public bool Equals(CardData other)
        {
            if (other == null)
                return false;
            
            return this.CardType == other.CardType && this.CardValue.View.Equals(other.CardValue.View) &&
                   this.CardValue.Value == other.CardValue.Value;
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
