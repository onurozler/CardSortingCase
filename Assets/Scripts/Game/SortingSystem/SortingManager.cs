using System.Collections.Generic;
using Game.CardSystem.Managers;
using System.Linq;
using Config;
using Game.CardSystem.Base;
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
            
            List<CardBase> nonConsecutives = new List<CardBase>();
            
            List<List<CardBase>> allConsecutives = FindConsecutivesForTypes();

            if(allConsecutives.Count <= 0)
                return;
            
            var combinedConsecutives = 
                allConsecutives.OrderByDescending(x => x.Count).SelectMany(x=>x).ToList();
            nonConsecutives = cardBases.Where(x => !combinedConsecutives.Contains(x)).ToList();

            _cardCurveManager.UpdateCurves(combinedConsecutives.Concat(nonConsecutives).ToList());
        }

        private List<List<CardBase>> FindConsecutivesForTypes(int minLength = 3)
        {
            var cardBases = _cardManager.GetCards();
            List<List<CardBase>> cardCurveTypes = new List<List<CardBase>>();

            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                cardCurveTypes.Add(cardBases
                    .Where(x => x.CardData.CardType == cardType).ToList());
            }
            
            if (cardCurveTypes.Count < minLength)
                return null;
            
            List<List<CardBase>> allConsecutives = new List<List<CardBase>>();

            foreach (var type in cardCurveTypes)
            {
                List<List<CardBase>> consecutivesForType = new List<List<CardBase>>();
                List<CardBase> tempList = new List<CardBase>();
                var typeOrdered = type.OrderBy(x => x.CardData.CardValue.Value).ToList();
            
                int consecutiveCounter = 0;
                for (int i = 0; i < typeOrdered.Count; i++)
                {
                    if (i != typeOrdered.Count-1 && typeOrdered[i + 1].CardData.CardValue.Value - 
                        typeOrdered[i].CardData.CardValue.Value == 1)
                    {
                        tempList.Add(typeOrdered[i]);
                        consecutiveCounter++;
                    }
                    else
                    {
                        if (consecutiveCounter >= minLength - 1)
                        {
                            tempList.Add(typeOrdered[i]);
                            consecutivesForType.Add(tempList.Clone());
                            tempList.Clear();
                        }
                    
                        consecutiveCounter = 0;
                    }
                }

                allConsecutives = allConsecutives.Concat(consecutivesForType).ToList();
            }

            return allConsecutives;
        }

        #endregion


        #region 777Sort/SameNumberSort

        public void SameNumberSort()
        {
            if (_cardCurveManager.HasNull())
                return;
            
            var cardBases = _cardManager.GetCards();
            
            List<List<CardBase>> sameNumbers = FindSameNumbers();
            List<CardBase> nonSameNumbers = new List<CardBase>();
            
            if(sameNumbers.Count <= 0)
                return;
            
            var sameNumbersCombined = sameNumbers.SelectMany(x => x).ToList();
            nonSameNumbers = cardBases.Where(x => !sameNumbersCombined.Contains(x)).ToList();
            
            _cardCurveManager.UpdateCurves(sameNumbersCombined.Concat(nonSameNumbers).ToList());
        }
        
        
        private List<List<CardBase>> FindSameNumbers()
        {
            var cardBases = _cardManager.GetCards();
            var cardValues = cardBases.Select(x => x.CardData.CardValue.View).Distinct().ToList();
            List<List<CardBase>> sameNumbers = new List<List<CardBase>>();
            foreach (var viewPattern in cardValues)
            {
                var curve = cardBases.Where(x => 
                    x.CardData.CardValue.View == viewPattern).Take(4).ToList();

                if (curve.Count >= 3)
                {
                    sameNumbers.Add(curve);
                }
            }
            return sameNumbers;
        }

        #endregion


        #region SmartSort

        public void SmartSort()
        {
            if (_cardCurveManager.HasNull())
                return;
            
            var consecutive = FindConsecutivesForTypes();
            var sameNumber = FindSameNumbers();

            var smartGroups=  GenerateCombinedGroups(consecutive,sameNumber);
            if(smartGroups.Count <= 0)
                return;

            var minSmart = smartGroups.OrderBy(x => x.SmartValue).First();
            _cardCurveManager.UpdateCurves(minSmart.CardBases.SelectMany(x=>x).ToList());

        }

        private List<SmartSortGroup> GenerateCombinedGroups(List<List<CardBase>> consecutive, List<List<CardBase>> sameNumber)
        {
            List<SmartSortGroup> smartSortGroups = new List<SmartSortGroup>();
            
            MatchBlocks(consecutive,sameNumber,smartSortGroups);
            MatchBlocks(sameNumber,consecutive,smartSortGroups);

            FindSmartValues(smartSortGroups);
            
            return smartSortGroups;
        }

        private void MatchBlocks(List<List<CardBase>> blockOne, List<List<CardBase>> blockTwo, List<SmartSortGroup> smartSortGroups)
        {
            foreach (var consecutiveBlock in blockOne)
            {
                var consClone = consecutiveBlock.Clone();
                SmartSortGroup smartSortGroup = new SmartSortGroup();
                foreach (var sameNumberBlock in blockTwo)
                {
                    var sameNumberClone = sameNumberBlock.Clone();
                    var sameValue = consClone.Intersect(sameNumberClone).ToList();
                    if (sameValue.Count == 1)
                    {
                        if (consClone.Count > sameNumberClone.Count)
                            consClone.Remove(sameValue[0]);
                        else
                            sameNumberClone.Remove(sameValue[0]);
                    }
                    smartSortGroup.CardBases.Add(sameNumberClone);
                }
                smartSortGroup.CardBases.Add(consClone);
                smartSortGroups.Add(smartSortGroup);
            }
        }
        
        private void FindSmartValues(List<SmartSortGroup> smartSortGroups)
        {
            foreach (var smartSortGroup in smartSortGroups)
            {
                int value = 0;
                var nonValues =_cardManager.NotContain(smartSortGroup.CardBases.SelectMany(x=>x).ToList());
                nonValues.ForEach(x=> value += x.CardData.CardValue.Value);
                smartSortGroup.CardBases.Add(nonValues);
                smartSortGroup.SmartValue = value;
            }
        }

        public class SmartSortGroup
        {
            public List<List<CardBase>> CardBases;
            public int SmartValue;

            public SmartSortGroup()
            {
                CardBases = new List<List<CardBase>>();
            }
        }
        
        #endregion
    }
}
