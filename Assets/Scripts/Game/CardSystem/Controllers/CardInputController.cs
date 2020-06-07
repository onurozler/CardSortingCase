using System;
using DG.Tweening;
using Game.CardSystem.Base;
using Game.CardSystem.Managers;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.CardSystem.Controllers
{
    public class CardInputController
    {
        public event Action<CardBase, CardBase> OnCardsSwapped; 
        
        private Camera _camera;
        private CardCurveManager _cardCurveManager;
        private CardCurveValue _selectedCurve;

        private Tween _selectTween;
        private Sequence _swapSequence;
        
        [Inject]
        private void OnInstaller(CardCurveManager cardCurveManager,Camera camera)
        {
            _camera = camera;
            _cardCurveManager = cardCurveManager;
            _selectedCurve = null;
            _swapSequence = DOTween.Sequence();
        }

        public void Initialize()
        {
            var observableUpdate = Observable.EveryUpdate();
                        
            observableUpdate
                .Where(_ => Input.GetMouseButton(0))
                .Select(_=> (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(SelectClosestCard);

            observableUpdate
                .Where(_ => !Input.GetMouseButton(0))
                .Where( _=> _selectedCurve != null && !_swapSequence.IsActive())
                .Subscribe(_=> DeselectClosestCard());
        }

        private void SelectClosestCard(Vector2 mousePos)
        {
            var closeCurve = _cardCurveManager.GetCardFromCurve(mousePos);
            if(closeCurve == null)
                return;
            
            if (_selectedCurve == null)
            {
                _selectedCurve = closeCurve;
                if (_selectTween != null && _selectTween.IsActive())
                {
                    _selectTween.Complete();
                    _selectTween.Kill();
                }
                _selectTween = _selectedCurve.CurrentCard.transform.DOMove(_selectedCurve.CurrentCard.transform.position + 
                                                                           _selectedCurve.CurrentCard.transform.up * 2f,0.5f);
            }
            if (!Equals(closeCurve,_selectedCurve))
            {
                if(_swapSequence.IsActive())
                    return;

                SwapCards(closeCurve);
            }
        }

        private void SwapCards(CardCurveValue closeCurve)
        {
            _swapSequence.Kill();
            _swapSequence = DOTween.Sequence()
                .Insert(0, _selectedCurve.CurrentCard.transform.DOMove(closeCurve.Position, 0.5f))
                .Insert(0, _selectedCurve.CurrentCard.transform.DORotate(closeCurve.Rotation, 0.5f))
                .Insert(0, closeCurve.CurrentCard.transform.DOMove(_selectedCurve.Position, 0.5f))
                .Insert(0, closeCurve.CurrentCard.transform.DORotate(_selectedCurve.Rotation, 0.5f));

            OnCardsSwapped.SafeInvoke(_selectedCurve.CurrentCard,closeCurve.CurrentCard);
            _selectedCurve = closeCurve;
        }

        private void DeselectClosestCard()
        {
            if (_selectTween != null && _selectTween.IsActive())
            {
                _selectTween.Complete();
                _selectTween.Kill();
            }
            
            _selectTween =_selectedCurve.CurrentCard.transform.DOMove(_selectedCurve.Position,0.5f);
            _selectedCurve = null;
            
        }
    }
}
