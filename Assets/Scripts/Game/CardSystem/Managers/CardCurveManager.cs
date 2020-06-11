using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.CardSystem.Base;
using Game.CardSystem.Controllers;
using Game.Config;
using NaughtyBezierCurves;
using UnityEngine;
using Zenject;

namespace Game.CardSystem.Managers
{
    public class CardCurveManager
    {
        private CardManager _cardManager;
        private CardInputController _cardInputController;
        
        private List<CardCurveValue> _availableValues;

        [Inject]
        private void OnInstaller(CardManager cardManager, CardInputController cardInputController)
        {
            _cardManager = cardManager;
            _cardInputController = cardInputController;
            
            _cardManager.OnCardAdded += AddCardOnCurve;
            _cardManager.OnCardAddedTest += AddCardOnCurve;
            _cardManager.OnCardDeleted += DeleteCardOnCurve;

            _cardInputController.OnCardsSwapped += SwapCards;
        }

        public CardCurveManager()
        {
            _availableValues = new List<CardCurveValue>(GameConfig.PLAYER_DECK_COUNT);
        }

        public void InitializeCurveValues(BezierCurve3D cardCurve)
        {
            float rotationZ = GameConfig.ROTATION_START; 
            float begin = 1f / GameConfig.PLAYER_DECK_COUNT / 2;
            float zPosition = 0;
            
            for (int i = 0; i < GameConfig.PLAYER_DECK_COUNT; i++)
            {
                _availableValues.Add(new CardCurveValue(null,cardCurve.GetPoint(begin),
                    new Vector3(0,0,rotationZ)));
                
                zPosition += 0.2f;
                rotationZ -= GameConfig.PLAYER_DECK_COUNT ;
                begin += 1f / GameConfig.PLAYER_DECK_COUNT ;
            }
        }

        private void AddCardOnCurve(CardBase cardBase)
        {
            var cardCurve = _availableValues.FirstOrDefault(x => x.CurrentCard == null);
            if (cardCurve != null)
            {
                cardCurve.CurrentCard = cardBase;
                cardBase.transform.DOMove(cardCurve.Position, 0.5f);
                cardBase.transform.DORotate(cardCurve.Rotation, 0.5f);
            }
        }
        
        private void DeleteCardOnCurve(CardBase cardBase)
        {
            var cardCurve = _availableValues.FirstOrDefault(x => x.CurrentCard == cardBase);
            if (cardCurve != null)
            {
                cardCurve.CurrentCard = null;
            }
        }
        
        private void SwapCards(CardBase card1, CardBase card2)
        {
            var curve1 = _availableValues.FirstOrDefault(x => x.CurrentCard == card1);
            var curve2 = _availableValues.FirstOrDefault(x => x.CurrentCard == card2);

            if (curve1 != null && curve2 != null)
            {
                curve1.CurrentCard = card2;
                curve2.CurrentCard = card1;
            }
        }
        
        public void UpdateCurves(List<CardBase> cardBases)
        {
            if(cardBases == null)
                return;
            
            for (int i = 0; i < cardBases.Count; i++)
            {
                _availableValues[i].CurrentCard = cardBases[i];
                DOTween.Sequence().
                    Insert(0,_availableValues[i].CurrentCard.transform.DOMove(_availableValues[i].Position, 0.5f)).
                    Insert(0,_availableValues[i].CurrentCard.transform.DORotate(_availableValues[i].Rotation, 0.5f));
            }
        }

        public CardCurveValue GetCardFromCurve(Vector2 pos)
        {
            var first = _availableValues.OrderBy(x => Vector2.Distance(x.Position, pos)).First();
            return Vector2.Distance(first.Position,pos) < GameConfig.CARD_SELECTION_THRESHOLD ? first : null;
        }

        public bool HasNull()
        {
            return _availableValues.Count(x => x.CurrentCard == null) > 0;
        }
    }
    
    
    public class CardCurveValue
    {
        public CardBase CurrentCard;
        public Vector3 Position;
        public Vector3 Rotation;

        public CardCurveValue(CardBase currentCard, Vector3 pos, Vector3 rotation)
        {
            CurrentCard = currentCard;
            Position = pos;
            Rotation = rotation;
        }
    }
}
