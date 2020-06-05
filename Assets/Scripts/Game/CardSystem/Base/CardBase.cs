using Game.CardSystem.Model;
using UnityEngine;

namespace Game.CardSystem.Base
{
    public class CardBase : MonoBehaviour
    {
        public TextMesh Number;
        public SpriteRenderer NumberIcon;
        public SpriteRenderer MainIcon;

        public CardData CardData;

        public void Initialize(CardData cardData)
        {
            CardData = cardData;
            
            Number.text = CardData.CardValue.View;
            if (CardData.CardValue.Portrait.Length > 0)
            {
                
            }
            
        }
    }
}
