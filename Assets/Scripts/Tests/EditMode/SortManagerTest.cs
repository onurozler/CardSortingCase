using System.Collections.Generic;
using System.Linq;
using Game.CardSystem.Base;
using Game.CardSystem.Model;
using Game.Config;
using Game.SortingSystem;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SortManagerTest
    {
        // 123/Consecutive Sorting Test
        [Test]
        public void Test_Consecutive_Sorting()
        {
            var testCards = CreateTestCards();
            SortingManager sortingManager = new SortingManager();
            List<CardData> expectedValues = GameConfig.TEST_EXPECTED_CARD_DATAS_CONSECUTIVE;
            
            var consecutives = sortingManager.FindConsecutivesForTypes(testCards).SelectMany(x=>x).ToList();
            
            Assert.AreEqual(expectedValues.Count,consecutives.Count);

            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.AreEqual(expectedValues[i].CardType,consecutives[i].CardData.CardType);
                Assert.AreEqual(expectedValues[i].CardValue.Value,consecutives[i].CardData.CardValue.Value);
            }
        }

        // 777/Same Number Sorting Test
        [Test]
        public void Test_SameNumber_Sorting()
        {
            var testCards = CreateTestCards();
            SortingManager sortingManager = new SortingManager();
            List<CardData> expectedValues = GameConfig.TEST_EXPECTED_CARD_DATAS_SAME;

            var sameNumber = sortingManager.FindSameNumbers(testCards).SelectMany(x=>x).ToList();
            
            Assert.AreEqual(expectedValues.Count,sameNumber.Count);
            
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.AreEqual(expectedValues[i].CardType,sameNumber[i].CardData.CardType);
                Assert.AreEqual(expectedValues[i].CardValue.View,sameNumber[i].CardData.CardValue.View);
            }
        }

        // Smart Sorting Test
        [Test]
        public void Test_Smart_Sorting()
        {
            var testCards = CreateTestCards();
            SortingManager sortingManager = new SortingManager();
            List<CardData> expectedValues = GameConfig.TEST_EXPECTED_CARD_DATAS_SMART;

            var smartGroup = sortingManager.FindSmartGroup(testCards).SelectMany(x=>x).ToList();

            Assert.AreEqual(expectedValues.Count,smartGroup.Count);
            
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.AreEqual(expectedValues[i].CardType,smartGroup[i].CardData.CardType);
                Assert.AreEqual(expectedValues[i].CardValue.View,smartGroup[i].CardData.CardValue.View);
            }
        }


        private List<CardBase> CreateTestCards()
        {
            List<CardBase> cardBases = new List<CardBase>();
            foreach (var cardData in GameConfig.TEST_CARD_DATAS )
            {
                var card = new GameObject("Card").AddComponent<CardBase>();
                card.Initialize(cardData,null,null);
                cardBases.Add(card);
            }

            return cardBases;
        }
    }
}
