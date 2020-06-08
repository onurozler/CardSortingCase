using System.Collections.Generic;
using Game.CardSystem.Managers;
using System.Linq;
using Config;
using Game.CardSystem.Base;
using ModestTree;
using UnityEngine;
using Utils;
using Zenject;

namespace Game.SortingSystem
{
    public class SortingManager
    {
        private CardCurveManager _cardCurveManager;
        private CardManager _cardManager;
        
        [Inject]
        private void OnInstaller(CardCurveManager cardCurveManager,CardManager cardManager)
        {
            _cardCurveManager = cardCurveManager;
            _cardManager = cardManager;
        }
        
        public SortingManager()
        {
        }


        #region 123Sort/ConsecutiveSort

        public void ConsecutiveSort()
        {
            if (_cardCurveManager.HasNull())
                return;
            
            var cardBases = _cardManager.GetCards();
            
            List<List<CardBase>> cardCurveTypes = new List<List<CardBase>>();
            List<CardBase> nonConsecutives = new List<CardBase>();
            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                cardCurveTypes.Add(cardBases
                    .Where(x => x.CardData.CardType == cardType).ToList());
            }

            List<List<CardBase>> allConsecutives = new List<List<CardBase>>();
            foreach (var type in cardCurveTypes)
            {
                var consecutive = FindConsecutivesForTypes(type);
                if(consecutive != null)
                    allConsecutives.Add(consecutive);
            }
            var combinedConsecutives = 
                allConsecutives.OrderByDescending(x => x.Count).SelectMany(x=>x).ToList();
            nonConsecutives = cardBases.Where(x => !combinedConsecutives.Contains(x)).ToList();

            _cardCurveManager.UpdateCurves(combinedConsecutives.Concat(nonConsecutives).ToList());
        }

        private List<CardBase> FindConsecutivesForTypes(List<CardBase> cardCurveTypes, int minLength = 3)
        {
            if (cardCurveTypes.Count < minLength)
                return null;
            
            List<List<CardBase>> consecutivesForType = new List<List<CardBase>>();
            List<CardBase> tempList = new List<CardBase>();
            cardCurveTypes = cardCurveTypes.OrderBy(x => x.CardData.CardValue.Value).ToList();

            int consecutiveCounter = 0;
            for (int i = 0; i < cardCurveTypes.Count; i++)
            {
                if (i != cardCurveTypes.Count-1 && cardCurveTypes[i + 1].CardData.CardValue.Value - 
                    cardCurveTypes[i].CardData.CardValue.Value == 1)
                {
                    tempList.Add(cardCurveTypes[i]);
                    consecutiveCounter++;
                }
                else
                {
                    if (consecutiveCounter >= minLength - 1)
                    {
                        tempList.Add(cardCurveTypes[i]);
                        consecutivesForType.Add(tempList.Clone());
                        tempList.Clear();
                    }
                    
                    consecutiveCounter = 0;
                }
            }
            return consecutivesForType.SelectMany(x => x).ToList();
        }

        #endregion


        #region 777Sort/SameNumberSort

        public void SameNumberSort()
        {
            if (_cardCurveManager.HasNull())
                return;
            
            var cardBases = _cardManager.GetCards();
            var cardValues = cardBases.Select(x => x.CardData.CardValue.View).Distinct().ToList();
            List<List<CardBase>> sameNumbers = new List<List<CardBase>>();
            List<CardBase> nonSameNumbers = new List<CardBase>();
            foreach (var viewPattern in cardValues)
            {
                var curve = cardBases.Where(x => 
                    x.CardData.CardValue.View == viewPattern).Take(4).ToList();

                if (curve.Count >= 3)
                {
                    sameNumbers.Add(curve);
                }
            }

            var sameNumbersCombined = sameNumbers.SelectMany(x => x).ToList();
            nonSameNumbers = cardBases.Where(x => !sameNumbersCombined.Contains(x)).ToList();
            
            _cardCurveManager.UpdateCurves(sameNumbersCombined.Concat(nonSameNumbers).ToList());
        }

        #endregion
    }
}
