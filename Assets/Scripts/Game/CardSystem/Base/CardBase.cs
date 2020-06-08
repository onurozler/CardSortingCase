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

        public void Initialize(CardData cardData, Sprite numberIcon, Sprite mainIcon)
        {
            CardData = cardData;
            
            if(numberIcon == null && mainIcon == null)
                return;
            
            Number.text = CardData.CardValue.View;
            Number.color = (int) CardData.CardType % 2 == 0 ? Color.red : Color.black;
            NumberIcon.sprite = numberIcon;
            MainIcon.sprite = mainIcon != null ? mainIcon : numberIcon;
        }
    }
}
