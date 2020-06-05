using Config;
using Game.CardSystem.Base;
using Game.CardSystem.Managers;
using Game.DeckSystem.Managers;
using Game.View;
using NaughtyBezierCurves;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.CardSystem.Controllers
{
    public class CardController : MonoBehaviour
    {
        private BezierCurve3D _cardCurve;
        
        private DeckManager _deckManager;
        private CardManager _cardManager;
        private CardCurveManager _cardCurveManager;
        
        [Inject]
        private void OnInstaller(CardManager cardManager,DeckManager deckManager,CardCurveManager cardCurveManager)
        {
            _cardManager = cardManager;
            _deckManager = deckManager;
            _cardCurveManager = cardCurveManager;
            
            _cardCurve = GetComponentInChildren<BezierCurve3D>();
        }

        public void Initialize()
        {
            MessageBroker.Default.Receive<PlayerButtonType>().Subscribe(ReceiveButtonAction);

            _cardCurveManager.InitializeCurveValues(_cardCurve);
        }
        #region ButtonEvents
        
        private void ReceiveButtonAction(PlayerButtonType buttonType)
        {
            if (buttonType == PlayerButtonType.WITDHDRAW)
                WithdrawCards();
        }

        private void WithdrawCards()
        {
            for (int i = 0; i < GameConfig.PLAYER_DECK_COUNT; i++)
            {
                _cardManager.AddCard(_deckManager.GetRandomCardData());
            }
        }
        
        #endregion

    }
}
