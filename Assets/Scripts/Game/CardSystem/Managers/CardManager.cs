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
        

        [Inject]
        private void OnInstaller(CardPoolManager cardPoolManager)
        {
            _cardPoolManager = cardPoolManager;
        }
        
        public CardManager()
        {
            _cardBases = new List<CardBase>(GameConfig.PLAYER_DECK_COUNT);
        }

        public CardBase AddCard(CardData cardData)
        {
            var card = _cardPoolManager.Spawn();
            card.Initialize(cardData);
            return card;
        }

        public void DeleteCard()
        {
            
        }
    }
}
