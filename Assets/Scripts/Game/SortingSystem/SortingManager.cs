using System.Collections.Generic;
using System.Linq;
using Game.CardSystem.Model;
using Game.Config;
using Utils;

namespace Game.SortingSystem
{
    public class SortingManager
    {
        public SortingManager()
        {
        }


        #region 123Sort/ConsecutiveSort

        public List<CardData> ConsecutiveSort(List<CardData> cardDatas)
        {
            List<CardData> nonConsecutives = new List<CardData>();
            
            List<List<CardData>> allConsecutives = FindConsecutivesForTypes(cardDatas);

            if(allConsecutives.Count <= 0)
                return null;
            
            var combinedConsecutives = 
                allConsecutives.OrderByDescending(x => x.Count).SelectMany(x=>x).ToList();
            
            foreach (var cardData in cardDatas)
            {
                if (!combinedConsecutives.Contains(cardData))
                {
                    nonConsecutives.Add(cardData);
                }
            }

            return combinedConsecutives.Concat(nonConsecutives).ToList();
        }

        private List<List<CardData>> FindConsecutivesForTypes(List<CardData> cardBases,int minLength = 3)
        {
            List<List<CardData>> cardCurveTypes = new List<List<CardData>>();

            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                cardCurveTypes.Add(cardBases
                    .Where(x => x.CardType == cardType).ToList());
            }
            
            if (cardCurveTypes.Count < minLength)
                return null;
            
            List<List<CardData>> allConsecutives = new List<List<CardData>>();

            foreach (var type in cardCurveTypes)
            {
                List<List<CardData>> consecutivesForType = new List<List<CardData>>();
                List<CardData> tempList = new List<CardData>();
                var typeOrdered = type.OrderBy(x => x.CardValue.Value).ToList();
            
                int consecutiveCounter = 0;
                for (int i = 0; i < typeOrdered.Count; i++)
                {
                    if (i != typeOrdered.Count-1 && typeOrdered[i + 1].CardValue.Value - 
                        typeOrdered[i].CardValue.Value == 1)
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

        public List<CardData> SameNumberSort(List<CardData> cardDatas)
        {
            var sameNumbers = FindSameNumbers(cardDatas);
            List<CardData> nonSameNumbers = new List<CardData>();
            
            if(sameNumbers.Count <= 0)
                return null;
            
            var sameNumbersCombined = sameNumbers.SelectMany(x => x).ToList();

            foreach (var cardData in cardDatas)
            {
                if (!sameNumbersCombined.Contains(cardData))
                {
                    nonSameNumbers.Add(cardData);
                }
            }
            return sameNumbersCombined.Concat(nonSameNumbers).ToList();
            //_cardCurveManager.UpdateCurves(sameNumbersCombined.Concat(nonSameNumbers).ToList());
        }
        
        
        private List<List<CardData>> FindSameNumbers(List<CardData> cardBases)
        {
            var cardValues = cardBases.Select(x => x.CardValue.View).Distinct().ToList();
            List<List<CardData>> sameNumbers = new List<List<CardData>>();
            foreach (var viewPattern in cardValues)
            {
                var curve = cardBases.Where(x => 
                    x.CardValue.View == viewPattern).Take(4).ToList();

                if (curve.Count >= 3)
                {
                    sameNumbers.Add(curve);
                }
            }
            return sameNumbers;
        }

        #endregion


        #region SmartSort

        public List<CardData> SmartSort(List<CardData> cardDatas)
        {
            var smartGroup = FindSmartGroup(cardDatas);

            List<CardData> combinedSmartGroup = null;
            if(smartGroup != null)
                combinedSmartGroup = smartGroup.SelectMany(x => x).ToList();

            return combinedSmartGroup;
        }

        private List<List<CardData>> FindSmartGroup(List<CardData> cardBases)
        {
            var consecutive = FindConsecutivesForTypes(cardBases);
            var sameNumber = FindSameNumbers(cardBases);

            var smartGroups=  GenerateCombinedGroups(consecutive,sameNumber,cardBases);
            if(smartGroups.Count <= 0)
                return null;

            var minSmart = smartGroups.OrderBy(x => x.SmartValue).First();
            return minSmart.CardBases.Select(x => x).ToList();
        }

        private List<SmartSortGroup> GenerateCombinedGroups(List<List<CardData>> consecutive, List<List<CardData>> sameNumber,
            List<CardData> cardBases)
        {
            List<SmartSortGroup> smartSortGroups = new List<SmartSortGroup>();
            
            MatchBlocks(consecutive,sameNumber,smartSortGroups);
            MatchBlocks(sameNumber,consecutive,smartSortGroups);

            FindSmartValues(smartSortGroups, cardBases);
            
            return smartSortGroups;
        }

        private void MatchBlocks(List<List<CardData>> blockOne, List<List<CardData>> blockTwo, List<SmartSortGroup> smartSortGroups)
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
        
        private void FindSmartValues(List<SmartSortGroup> smartSortGroups, List<CardData> cardBases)
        {
            foreach (var smartSortGroup in smartSortGroups)
            {
                int value = 0;
                var nonValues =FindNotGroupedCards(cardBases,smartSortGroup.CardBases.
                    SelectMany(x=>x).ToList());
                nonValues.ForEach(x=> value += x.CardValue.Value);
                smartSortGroup.CardBases.Add(nonValues);
                smartSortGroup.SmartValue = value;
            }
        }
        
        private List<CardData> FindNotGroupedCards(List<CardData> cards,List<CardData> cardBases)
        {
            List<CardData> notInList = new List<CardData>();
            foreach (var card in cards)
            {
                if (!cardBases.Contains(card))
                {
                    notInList.Add(card);
                }
            }

            return notInList;
        }

        public class SmartSortGroup
        {
            public List<List<CardData>> CardBases;
            public int SmartValue;

            public SmartSortGroup()
            {
                CardBases = new List<List<CardData>>();
            }
        }
        
        #endregion
        
    }
}
