using System.Collections.Generic;
using Config;
using Game.CardSystem.Base;
using Game.CardSystem.Managers;
using Game.CardSystem.Model;
using Game.Managers;
using Utils;
using Zenject;

namespace Game.DeckSystem.Managers
{
    public class DeckManager
    {
        private List<CardData> _cardDatas;

        private CardManager _cardManager;
        private AssetManager _assetManager;
        
        [Inject]
        private void OnInstaller(CardManager cardManager,AssetManager assetManager)
        {
            _cardManager = cardManager;
            _assetManager = assetManager;
            
            GenerateCardDatas();
        }
        
        public DeckManager()
        {
            _cardDatas = new List<CardData>(GameConfig.DECK_COUNT);
        }

        private void GenerateCardDatas()
        {
            _cardManager.OnCardAdded += WitdrawCardFromDeck;
            _cardManager.OnCardDeleted += PutCardToDeck;
            
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

        private void WitdrawCardFromDeck(CardBase cardBase)
        {
            var cardData = _cardDatas.GetRandomElementFromList();
            _cardDatas.Remove(cardData);
            cardBase.Initialize(cardData,_assetManager.GetCardIcon(cardData.CardType),
                _assetManager.GetPortraitIcon(cardData.CardValue.Portrait));
        }

        private void PutCardToDeck(CardBase cardBase)
        {
            _cardDatas.Add(cardBase.CardData);
        }
        
    }
}
