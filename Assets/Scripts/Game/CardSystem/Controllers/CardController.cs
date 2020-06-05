using Config;
using Game.CardSystem.Managers;
using Game.DeckSystem.Managers;
using Game.Managers;
using Game.View;
using NaughtyBezierCurves;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.CardSystem.Controllers
{
    public class CardController : MonoBehaviour
    {
        private BezierCurve3D _bezierCurve;

        private DeckManager _deckManager;
        private CardManager _cardManager;
        
        [Inject]
        private void OnInstaller(CardManager cardManager,DeckManager deckManager)
        {
            _cardManager = cardManager;
            _deckManager = deckManager;
            
            _bezierCurve = GetComponentInChildren<BezierCurve3D>();

            MessageBroker.Default.Receive<PlayerButtonType>().Subscribe(ReceiveButtonAction);
        }

        private void ReceiveButtonAction(PlayerButtonType buttonType)
        {
            if (buttonType == PlayerButtonType.WITDHDRAW)
                WithdrawCards();
        }

        private void WithdrawCards()
        {
            float rotationZ = 55; //DegreeDiff
            float begin = 1f / GameConfig.PLAYER_DECK_COUNT / 2;
            float zPosition = 0;
            
            for (int i = 0; i < GameConfig.PLAYER_DECK_COUNT; i++)
            {
                var card = _cardManager.AddCard(_deckManager.GetRandomCardData());
                Vector3 newPos = _bezierCurve.GetPoint(begin);
                newPos.z = zPosition;
                card.transform.position = newPos;
                card.transform.eulerAngles = new Vector3(0,0,rotationZ);
                
                zPosition += 0.2f;
                rotationZ -= GameConfig.PLAYER_DECK_COUNT;
                begin += 1f / GameConfig.PLAYER_DECK_COUNT;
            }
        }
    }
}
