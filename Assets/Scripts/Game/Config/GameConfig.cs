﻿using System.Collections.Generic;
using Game.CardSystem.Model;

namespace Game.Config
{
    public static class GameConfig
    {
        public static int INITIAL_POOLITEM_COUNT = 11;
        public static int PLAYER_DECK_COUNT = 11;
        public static int DECK_COUNT = 52;

        public static float CARD_SELECTION_THRESHOLD = 3f;
        public static float WITHDRAW_SECONDS = 0.2f;
        public static float ROTATION_START = 55;

        #region Card Options

        public static List<CardType> CARD_TYPES = new List<CardType>
        {
            CardType.Clubs,
            CardType.Diamonds,
            CardType.Hearts,
            CardType.Spades
        };
            
        public static List<CardValue> CARD_VALUES = new List<CardValue>
        {
            new CardValue("A",1),
            new CardValue("2",2),
            new CardValue("3",3),
            new CardValue("4",4),
            new CardValue("5",5),
            new CardValue("6",6),
            new CardValue("7",7),
            new CardValue("8",8),
            new CardValue("9",9),
            new CardValue("10",10),
            new CardValue("J",10, "Jack"),
            new CardValue("Q",10,"Queen"),
            new CardValue("K",10,"King"),
        };

        #endregion

        #region Test

        public static string TEST_DRAW_COMMAND = "TestDraw";

        // Example Card Datas from Document
        public static List<CardData> TEST_CARD_DATAS = new List<CardData>()
        {
            new CardData(CardType.Hearts,CARD_VALUES[0]),
            new CardData(CardType.Spades,CARD_VALUES[1]),
            new CardData(CardType.Diamonds,CARD_VALUES[4]),
            new CardData(CardType.Hearts,CARD_VALUES[3]),
            new CardData(CardType.Spades,CARD_VALUES[0]),
            new CardData(CardType.Diamonds,CARD_VALUES[2]),
            new CardData(CardType.Clubs,CARD_VALUES[3]),
            new CardData(CardType.Spades,CARD_VALUES[3]),
            new CardData(CardType.Diamonds,CARD_VALUES[0]),
            new CardData(CardType.Spades,CARD_VALUES[2]),
            new CardData(CardType.Diamonds,CARD_VALUES[3])
            
        };

        public static List<CardData> TEST_EXPECTED_CARD_DATAS_CONSECUTIVE = new List<CardData>
        {
            TEST_CARD_DATAS[4],
            TEST_CARD_DATAS[1],
            TEST_CARD_DATAS[9],
            TEST_CARD_DATAS[7],
            TEST_CARD_DATAS[5],
            TEST_CARD_DATAS[10],
            TEST_CARD_DATAS[2],
            TEST_CARD_DATAS[0],
            TEST_CARD_DATAS[3],
            TEST_CARD_DATAS[6],
            TEST_CARD_DATAS[8]
        };

        public static List<CardData> TEST_EXPECTED_CARD_DATAS_SAME = new List<CardData>()
        {
            TEST_CARD_DATAS[0],
            TEST_CARD_DATAS[4],
            TEST_CARD_DATAS[8],
            TEST_CARD_DATAS[3],
            TEST_CARD_DATAS[6],
            TEST_CARD_DATAS[7],
            TEST_CARD_DATAS[10],
            TEST_CARD_DATAS[1],
            TEST_CARD_DATAS[2],
            TEST_CARD_DATAS[5],
            TEST_CARD_DATAS[9]
        };

        public static List<CardData> TEST_EXPECTED_CARD_DATAS_SMART = new List<CardData>
        {
            TEST_CARD_DATAS[5],
            TEST_CARD_DATAS[10],
            TEST_CARD_DATAS[2],
            TEST_CARD_DATAS[4],
            TEST_CARD_DATAS[1],
            TEST_CARD_DATAS[9],
            TEST_CARD_DATAS[3],
            TEST_CARD_DATAS[6],
            TEST_CARD_DATAS[7],
            TEST_CARD_DATAS[0],
            TEST_CARD_DATAS[8]
        };

        #endregion


    }
}
