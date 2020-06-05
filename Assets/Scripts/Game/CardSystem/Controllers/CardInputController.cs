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
                .Where(_ => Input.GetMouseButton(0))
                .Select(_=> (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(SelectClosestCard);
        }

        private void SelectClosestCard(Vector2 mousePos)
        {
            var selectedCard = _cardCurveManager.GetCardFromCurve(mousePos);
            
            if (selectedCard != null)
            {
                if (Equals(selectedCard, _selectedCard))
                    return;
                
                _selectedCard = selectedCard;
                selectedCard.transform.DOMove(selectedCard.transform.position + 
                                              selectedCard.transform.up * 2f,0.5f);
            }
        }
    }
}
