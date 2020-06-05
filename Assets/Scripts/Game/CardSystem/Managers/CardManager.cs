using System;
using System.Collections.Generic;
using Config;
using Game.CardSystem.Base;
using Utils;
using Zenject;

namespace Game.CardSystem.Managers
{
    public class CardManager
    {
        #region Events
        public event Action<CardBase> OnCardAdded;
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

        public void ResetCards()
        {
            foreach (var cardBase in _cardBases)
            {
                _cardPoolManager.Despawn(cardBase);
                OnCardDeleted.SafeInvoke(cardBase);
            }
            
            _cardBases.Clear();
        }
    }
}
