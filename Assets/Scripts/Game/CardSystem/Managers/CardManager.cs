using System;
using System.Collections.Generic;
using Config;
using Game.CardSystem.Base;
using Game.CardSystem.Model;
using Game.Managers;
using Utils;
using Zenject;

namespace Game.CardSystem.Managers
{
    public class CardManager
    {
        #region Events
        public event Action<CardBase> OnCardAdded;

        #endregion
        
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

        public void AddCard(CardData cardData)
        {
            var card = _cardPoolManager.Spawn();
            card.Initialize(cardData,_assetManager.GetCardIcon(cardData.CardType),
                _assetManager.GetPortraitIcon(cardData.CardValue.Portrait));
            
            OnCardAdded.SafeInvoke(card);
            
            _cardBases.Add(card);
        }

        public void DeleteCard()
        {
            
        }
    }
}
