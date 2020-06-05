
namespace Game.CardSystem.Model
{
    public class CardData
    {
        public CardType CardType;
        public CardValue CardValue;
    }

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
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }
}
