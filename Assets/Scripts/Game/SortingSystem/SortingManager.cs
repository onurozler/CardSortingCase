using System.Collections.Generic;
using Game.CardSystem.Managers;
using System.Linq;
using Config;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.SortingSystem
{
    public class SortingManager
    {
        private CardCurveManager _cardCurveManager;
        
        [Inject]
        private void OnInstaller(CardCurveManager cardCurveManager)
        {
            _cardCurveManager = cardCurveManager;
        }
        
        public SortingManager()
        {
        }


        #region 123Sort/ConsecutiveSort

        public void ConsecutiveSort()
        {
            var cardCurveList = _cardCurveManager.GetCardCurveValues();
            
            List<List<CardCurveValue>> cardCurveTypes = new List<List<CardCurveValue>>();
            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                cardCurveTypes.Add(cardCurveList
                    .Where(x => x.CurrentCard.CardData.CardType == cardType).ToList());
            }

            /*
            var sa = cardCurveTypes[0].ConsecutiveSequence();

            foreach (var ass in sa)
            {
                foreach (var test in ass)
                {
                    Debug.Log(test);
                }
            }*/

            //List<CardCurveValue> nonConsecutives;
            //var consecutiveValues = FindConsecutiveAndNonConsecutiveValues(cardCurveTypes, out nonConsecutives);
        }

        
        private List<List<CardCurveValue>> FindConsecutiveAndNonConsecutiveValues
            (List<List<CardCurveValue>> cardCurveTypes, out List<CardCurveValue> nonConsecutives)
        {
            List<List<CardCurveValue>> allConsecutiveValues = new List<List<CardCurveValue>>();
            nonConsecutives = new List<CardCurveValue>();

            foreach (var cardType in cardCurveTypes)
            {
                var selectedType =cardType.Select(x => x.CurrentCard.CardData.CardValue.Value);
                var consecutiveValues = selectedType.ConsecutiveSequences().ToList();
                foreach (var consValue in consecutiveValues)
                {
                   var consCardCurveValue = 
                       cardType.Where(x => consValue.Contains(x.CurrentCard.CardData.CardValue.Value)).ToList();

                   allConsecutiveValues.Add(consCardCurveValue);
                }
            }

            return allConsecutiveValues;
        }

        #endregion


        #region 777Sort/SameNumberSort

        public List<CardCurveValue> SameNumberSort()
        {
            if (_cardCurveManager.HasNull())
                return null;
            
            var nonSameValue = _cardCurveManager.GetCardCurveValues().Clone();
            var cardValues = nonSameValue.Select(x => x.CurrentCard.CardData.CardValue.View).Distinct().ToList();
            List<List<CardCurveValue>> sameNumbers = new List<List<CardCurveValue>>();
            foreach (var viewPattern in cardValues)
            {
                var curve = nonSameValue.Where(x => 
                    x.CurrentCard.CardData.CardValue.View == viewPattern).Take(4).ToList();

                if (curve.Count >= 3)
                {
                    sameNumbers.Add(curve);
                    nonSameValue.RemoveAll(x => curve.Contains(x));
                }
            }
            
            return sameNumbers.SelectMany(x=>x).ToList();
        }

        #endregion
    }
}
