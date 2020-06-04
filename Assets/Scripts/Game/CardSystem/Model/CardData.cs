
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

        public CardValue(string view, int value)
        {
            View = view;
            Value = value;
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
