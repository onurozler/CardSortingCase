using System.Collections.Generic;
using Config;
using Game.CardSystem.Model;
using UnityEngine;
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
            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                foreach (var cardVal in GameConfig.CARD_VALUES)
                {
                    CardData cardData = new CardData();
                    CardValue cardValue = new CardValue(cardVal.View,cardVal.Value,cardVal.Portrait);
                    cardData.CardType = cardType;
                    cardData.CardValue = cardValue;
                    _cardDatas.Add(cardData);
                }
            }
        }

        public CardData GetRandomCardData()
        {
            var card = _cardDatas.GetRandomElementFromList();
            _cardDatas.Remove(card);
            return card;
        }
    }
}
