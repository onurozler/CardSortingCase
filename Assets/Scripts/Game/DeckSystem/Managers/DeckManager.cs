using System.Collections.Generic;
using Config;
using Game.CardSystem.Model;
using Utils;

namespace Game.DeckSystem.Managers
{
    public class DeckManager
    {
        private List<CardData> _cardDatas;
        
        public DeckManager()
        {
            _cardDatas = new List<CardData>(GameConfig.DECK_COUNT);

            GenerateCardDatas();
        }

        private void GenerateCardDatas()
        {
            CardData cardData = new CardData();

            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                foreach (var cardVal in GameConfig.CARD_VALUES)
                {
                    CardValue cardValue = new CardValue(cardVal.View,cardVal.Value);
                    cardData.CardType = cardType;
                    cardData.CardValue = cardValue;
                    
                    _cardDatas.Add(cardData);
                }
            }
        }

        public void WithdrawCard()
        {
            var card = _cardDatas.GetRandomElementFromList();
            
        }
    }
}
