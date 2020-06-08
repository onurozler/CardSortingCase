using System.Collections.Generic;
using Game.CardSystem.Base;
using Game.CardSystem.Managers;
using Game.CardSystem.Model;
using Game.Config;
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

        private static int _testCounter;
        
        [Inject]
        private void OnInstaller(CardManager cardManager,AssetManager assetManager)
        {
            _cardManager = cardManager;
            _assetManager = assetManager;
            _testCounter = 0;
            
            _cardManager.OnCardAdded += WitdrawCardFromDeck;
            _cardManager.OnCardAddedTest += WithdrawTestCards;
            _cardManager.OnCardDeleted += PutCardToDeck;
            
            GenerateCardDatas();
        }

        public DeckManager()
        {
            _cardDatas = new List<CardData>(GameConfig.DECK_COUNT);
        }

        public void GenerateCardDatas()
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

        public List<CardData> GetDeck()
        {
            return _cardDatas;
        }
        
        private void WithdrawTestCards(CardBase cardBase)
        {
            var cardData = GameConfig.TEST_CARD_DATAS[_testCounter++];
            cardBase.Initialize(cardData, _assetManager.GetCardIcon(cardData.CardType),
                _assetManager.GetPortraitIcon(cardData.CardValue.Portrait));
        }

        private void WitdrawCardFromDeck(CardBase cardBase)
        {
            var cardData = _cardDatas.GetRandomElementFromList();
            _cardDatas.Remove(cardData);
            cardBase.Initialize(cardData, _assetManager.GetCardIcon(cardData.CardType),
                    _assetManager.GetPortraitIcon(cardData.CardValue.Portrait));
        }

        private void PutCardToDeck(CardBase cardBase)
        {
            _cardDatas.Add(cardBase.CardData);
            _testCounter = 0;
        }
        
    }
}
