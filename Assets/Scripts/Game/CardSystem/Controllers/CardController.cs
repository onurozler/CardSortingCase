using System;
using Config;
using Game.CardSystem.Managers;
using Game.DeckSystem.Managers;
using Game.SortingSystem;
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

        private CardManager _cardManager;
        private CardCurveManager _cardCurveManager;
        private SortingManager _sortingManager;

        #endregion

        #region Controllers

        private CardInputController _cardInputController;

        #endregion
        
        
        [Inject]
        private void OnInstaller(CardManager cardManager,CardCurveManager cardCurveManager,SortingManager sortingManager,
            CardInputController cardInputController)
        {
            _cardManager = cardManager;
            _cardCurveManager = cardCurveManager;
            _cardInputController = cardInputController;
            _sortingManager = sortingManager;
            
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
            switch (buttonType)
            {
                case PlayerButtonType.WITDHDRAW:
                    WithdrawCards();
                    break;
                case PlayerButtonType.CONSECUTIVE_SORT:
                    _sortingManager.ConsecutiveSort();
                    break;
                case PlayerButtonType.SAME_NUMBER_SORT:
                    break;
                case PlayerButtonType.SMARTSORT:
                    break;
            }
        }

        private void WithdrawCards()
        {
            _cardManager.ResetCards();
            
            for (int i = 0; i < GameConfig.PLAYER_DECK_COUNT; i++)
            {
                _cardManager.AddCard();
            }
        }
        
        
        #endregion

    }
}
