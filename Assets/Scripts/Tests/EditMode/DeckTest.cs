using System.Collections.Generic;
using Game.CardSystem.Model;
using Game.Config;
using Game.DeckSystem.Managers;
using NUnit.Framework;

namespace Tests
{
    public class DeckTest
    {
        
        // Test - Deck is consist of 52 Cards
        [Test]
        public void Test_Deck_Count()
        {
            DeckManager deckManager = new DeckManager();
            deckManager.GenerateCardDatas();

            Assert.AreEqual(GameConfig.DECK_COUNT, deckManager.GetDeck().Count);
        }

        // Test - Deck has proper cards
        [Test]
        public void Test_Deck_Cards()
        {
            DeckManager deckManager = new DeckManager();
            deckManager.GenerateCardDatas();
            var deck = deckManager.GetDeck();
            
            List<CardData> expectedCards = new List<CardData>();
            
            foreach (var cardType in GameConfig.CARD_TYPES)
            {
                foreach (var cardVal in GameConfig.CARD_VALUES)
                {
                    CardData cardData = new CardData();
                    CardValue cardValue = new CardValue(cardVal.View,cardVal.Value,cardVal.Portrait);
                    cardData.CardType = cardType;
                    cardData.CardValue = cardValue;
                    expectedCards.Add(cardData);
                }
            }

            for (int i = 0; i < expectedCards.Count; i++)
            {
                Assert.AreEqual(expectedCards[i].CardValue.Value, deck[i].CardValue.Value);
            }
            
        }
    }
}
