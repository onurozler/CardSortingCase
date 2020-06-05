using Config;
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

        #region Managers

        private DeckManager _deckManager;
        private CardManager _cardManager;
        private CardCurveManager _cardCurveManager;

        #endregion

        #region Controllers

        private CardInputController _cardInputController;

        #endregion
        
        
        [Inject]
        private void OnInstaller(CardManager cardManager,DeckManager deckManager,CardCurveManager cardCurveManager,
            CardInputController cardInputController)
        {
            _cardManager = cardManager;
            _deckManager = deckManager;
            _cardCurveManager = cardCurveManager;
            _cardInputController = cardInputController;
            
            _cardCurve = GetComponentInChildren<BezierCurve3D>();
        }

        public void Initialize()
        {
            MessageBroker.Default.Receive<PlayerButtonType>().Subscribe(ReceiveButtonAction);

            _cardCurveManager.InitializeCurveValues(_cardCurve);
            _cardInputController.Initialize();
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
