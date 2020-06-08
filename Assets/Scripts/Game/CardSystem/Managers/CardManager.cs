using System;
using Game.CardSystem.Base;
using System.Collections.Generic;
using Game.Config;
using Utils;
using Zenject;

namespace Game.CardSystem.Managers
{
    public class CardManager
    {
        #region Events
        public event Action<CardBase> OnCardAdded;
        public event Action<CardBase> OnCardAddedTest;
        public event Action<CardBase> OnCardDeleted;

        #endregion
        
        private List<CardBase> _cardBases;

        private CardPoolManager _cardPoolManager;
        

        [Inject]
        private void OnInstaller(CardPoolManager cardPoolManager)
        {
            _cardPoolManager = cardPoolManager;
        }
        
        public CardManager()
        {
            _cardBases = new List<CardBase>(GameConfig.PLAYER_DECK_COUNT);
        }

        public void AddCard()
        {
            var card = _cardPoolManager.Spawn();
            OnCardAdded.SafeInvoke(card);
            _cardBases.Add(card);
        }

        public void AddTestCard()
        {
            var card = _cardPoolManager.Spawn();
            OnCardAddedTest.SafeInvoke(card);
            _cardBases.Add(card);
        }

        public void ResetCards()
        {
            foreach (var cardBase in _cardBases)
            {
                _cardPoolManager.Despawn(cardBase);
                OnCardDeleted.SafeInvoke(cardBase);
            }
            
            _cardBases.Clear();
        }
        
        
        public List<CardBase> GetCards()
        {
            return _cardBases;
        }
    }
}
