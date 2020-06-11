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
            SortingManager sortingManager = new SortingManager();
            List<CardData> expectedValues = GameConfig.TEST_EXPECTED_CARD_DATAS_CONSECUTIVE;

            var consecutives = sortingManager.ConsecutiveSort(GameConfig.TEST_CARD_DATAS);
            
            
            Assert.AreEqual(expectedValues.Count,consecutives.Count);

            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.AreEqual(expectedValues[i].CardType,consecutives[i].CardType);
                Assert.AreEqual(expectedValues[i].CardValue.Value,consecutives[i].CardValue.Value);
            }
        }

        // 777/Same Number Sorting Test
        [Test]
        public void Test_SameNumber_Sorting()
        {
            SortingManager sortingManager = new SortingManager();
            List<CardData> expectedValues = GameConfig.TEST_EXPECTED_CARD_DATAS_SAME;

            var sameNumber = sortingManager.SameNumberSort(GameConfig.TEST_CARD_DATAS);

            Assert.AreEqual(expectedValues.Count,sameNumber.Count);
            
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.AreEqual(expectedValues[i].CardType,sameNumber[i].CardType);
                Assert.AreEqual(expectedValues[i].CardValue.View,sameNumber[i].CardValue.View);
            }
        }

        // Smart Sorting Test
        [Test]
        public void Test_Smart_Sorting()
        {
            var testCards = GameConfig.TEST_CARD_DATAS;
            SortingManager sortingManager = new SortingManager();
            List<CardData> expectedValues = GameConfig.TEST_EXPECTED_CARD_DATAS_SMART;

            var smartGroup = sortingManager.SmartSort(GameConfig.TEST_CARD_DATAS);

            Assert.AreEqual(expectedValues.Count,smartGroup.Count);
            
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.AreEqual(expectedValues[i].CardType,smartGroup[i].CardType);
                Assert.AreEqual(expectedValues[i].CardValue.View,smartGroup[i].CardValue.View);
            }
        }

        
    }
}
