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
        
        [Inject]
        private void OnInstaller(CardCurveManager cardCurveManager,Camera camera)
        {
            _camera = camera;
            _cardCurveManager = cardCurveManager;
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
               
        }
    }
}
