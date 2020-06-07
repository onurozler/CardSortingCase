using System.Collections.Generic;
using System.Linq;
using Config;
using DG.Tweening;
using Game.CardSystem.Base;
using Game.CardSystem.Controllers;
using NaughtyBezierCurves;
using UniRx;
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
        }

        public CardCurveManager()
        {
            _availableValues = new List<CardCurveValue>(GameConfig.PLAYER_DECK_COUNT);
        }

        public void InitializeCurveValues(BezierCurve3D cardCurve)
        {
            _cardManager.OnCardAdded += AddCardOnCurve;
            _cardManager.OnCardDeleted += DeleteCardOnCurve;

            _cardInputController.OnCardsSwapped += SwapCards;
            
            float rotationZ = 55; 
            float begin = 1f / GameConfig.PLAYER_DECK_COUNT / 2;
            float zPosition = 0;
            
            for (int i = 0; i < GameConfig.PLAYER_DECK_COUNT; i++)
            {
                _availableValues.Add(new CardCurveValue(i+1,null,cardCurve.GetPoint(begin),
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

                //  Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(i =>
               // {
                    cardBase.transform.DOMove(cardCurve.Position, 0.5f);
                    cardBase.transform.DORotate(cardCurve.Rotation, 0.5f);
               // });

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

        public CardCurveValue GetCardFromCurve(Vector2 pos)
        {
            var first = _availableValues.OrderBy(x => Vector2.Distance(x.Position, pos)).First();
            return Vector2.Distance(first.Position,pos) < GameConfig.CARD_SELECTION_THRESHOLD ? first : null;
        }

        public List<CardCurveValue> GetCardCurveValues()
        {
            return _availableValues;
        }
    }
    
    
    public class CardCurveValue
    {
        public int Index;
        public CardBase CurrentCard;
        public Vector3 Position;
        public Vector3 Rotation;

        public CardCurveValue(int index,CardBase currentCard, Vector3 pos, Vector3 rotation)
        {
            Index = index;
            CurrentCard = currentCard;
            Position = pos;
            Rotation = rotation;
        }
    }
}
