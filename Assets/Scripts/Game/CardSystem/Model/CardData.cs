using System;

namespace Game.CardSystem.Model
{
    [Serializable]
    public class CardData
    {
        public CardType CardType;
        public CardValue CardValue;
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
