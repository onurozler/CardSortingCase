using System.Collections.Generic;
using Game.CardSystem.Managers;
using System.Linq;
using Config;
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
            
            List<CardCurveValue> nonConsecutives;
            var consecutiveValues = FindConsecutiveAndNonConsecutiveValues(cardCurveTypes, out nonConsecutives);
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

    }
}
