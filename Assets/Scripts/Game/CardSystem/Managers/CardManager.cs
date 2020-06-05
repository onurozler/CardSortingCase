using System.Collections.Generic;
using Config;
using Game.CardSystem.Base;
using Game.CardSystem.Model;
using Game.Managers;
using Zenject;

namespace Game.CardSystem.Managers
{
    public class CardManager
    {
        private List<CardBase> _cardBases;

        private CardPoolManager _cardPoolManager;
        private AssetManager _assetManager;
        

        [Inject]
        private void OnInstaller(CardPoolManager cardPoolManager, AssetManager assetManager)
        {
            _cardPoolManager = cardPoolManager;
            _assetManager = assetManager;
        }
        
        public CardManager()
        {
            _cardBases = new List<CardBase>(GameConfig.PLAYER_DECK_COUNT);
        }

        public CardBase AddCard(CardData cardData)
        {
            var card = _cardPoolManager.Spawn();
            card.Initialize(cardData,_assetManager.GetCardIcon(cardData.CardType),
                _assetManager.GetPortraitIcon(cardData.CardValue.Portrait));
            return card;
        }

        public void DeleteCard()
        {
            
        }
    }
}
