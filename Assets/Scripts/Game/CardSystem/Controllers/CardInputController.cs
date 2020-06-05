using System;
using DG.Tweening;
using Game.CardSystem.Base;
using Game.CardSystem.Managers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.CardSystem.Controllers
{
    public class CardInputController
    {
        private Camera _camera;
        private CardCurveManager _cardCurveManager;
        private CardBase _selectedCard;

        private Tween _currentTween;
        
        [Inject]
        private void OnInstaller(CardCurveManager cardCurveManager,Camera camera)
        {
            _camera = camera;
            _cardCurveManager = cardCurveManager;
            _selectedCard = null;
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_=> (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(SelectClosestCard);

            Observable.EveryUpdate()
                .Where(_ => !Input.GetMouseButton(0))
                .Subscribe(_=> DeselectClosestCard());
        }

        private void SelectClosestCard(Vector2 mousePos)
        {
            var selectedCard = _cardCurveManager.GetCardFromCurve(mousePos);
            
            if (selectedCard != null)
            {
                if (Equals(selectedCard, _selectedCard))
                    return;
                
                _selectedCard = selectedCard;

                if (_currentTween != null && _currentTween.IsActive())
                {
                    _currentTween.Complete();
                    _currentTween.Kill();
                }

                _currentTween = selectedCard.transform.DOMove(selectedCard.transform.position + 
                                                              selectedCard.transform.up * 2f,0.5f);
            }
        }

        private void DeselectClosestCard()
        {
            if (_selectedCard != null)
            {
                if (_currentTween != null && _currentTween.IsActive())
                {
                    _currentTween.Complete();
                    _currentTween.Kill();
                }
                
                _currentTween =_selectedCard.transform.DOMove(_selectedCard.transform.position + 
                                                              _selectedCard.transform.up * -2f,0.5f);
                _selectedCard = null;
            }
        }
    }
}
