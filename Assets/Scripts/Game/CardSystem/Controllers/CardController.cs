using System;
using System.Collections.Generic;
using Config;
using Game.CardSystem.Managers;
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
        private IDisposable _withdrawDisposable;

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
            MessageBroker.Default.Receive<(PlayerButtonType, string)>().Subscribe(ReceiveButtonActionWithParameters);
            
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
                    SortConsecutive();
                    break;
                case PlayerButtonType.SAME_NUMBER_SORT:
                    SortSameNumbers();
                    break;
                case PlayerButtonType.SMARTSORT:
                    break;
            }
        }

        private void ReceiveButtonActionWithParameters((PlayerButtonType,string) parameter)
        {
            if (parameter.Item2.Equals(GameConfig.TEST_DRAW_COMMAND))
            {
                WithdrawCards(true);
            }
        }

        private void WithdrawCards(bool testDeck = false)
        {
            _cardManager.ResetCards();
            _withdrawDisposable?.Dispose();
            int counter = 0;
            _withdrawDisposable = Observable.Interval(TimeSpan.FromSeconds(GameConfig.WITHDRAW_SECONDS)).Subscribe(_ =>
            {
                counter++;
                if (counter > GameConfig.PLAYER_DECK_COUNT)
                    _withdrawDisposable.Dispose();
                else
                {
                    if(!testDeck)
                        _cardManager.AddCard();
                    else
                        _cardManager.AddTestCard();
                }
            });
        }

        private void SortConsecutive()
        {
            _sortingManager.ConsecutiveSort();
        }
        
        private void SortSameNumbers()
        {
            var sameNumbers = _sortingManager.SameNumberSort();
            if (sameNumbers != null)
            {
                int index = 1;
                sameNumbers.ForEach(x => _cardCurveManager.SwapCards(x.Index, index++));
            }
        }
        
        #endregion

    }
}
