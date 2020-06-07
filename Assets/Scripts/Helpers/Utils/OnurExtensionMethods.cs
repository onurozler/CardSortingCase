using System;
using System.Collections.Generic;
using System.Linq;
using Game.CardSystem.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class OnurExtensionMethods
    {
        public static void SafeInvoke(this Action source)
        {
            if (source != null) source.Invoke();
        }

        public static void SafeInvoke<T>(this Action<T> source, T value)
        {
            if (source != null) source.Invoke(value);
        }
        
        public static void SafeInvoke<T1, T2>(this Action<T1, T2> source, T1 firstValue, T2 secondValue)
        {
            if (source != null) source.Invoke(firstValue, secondValue);
        }
        
        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> source, T1 firstValue, T2 secondValue, T3 thirdValue)
        {
            if (source != null) source.Invoke(firstValue, secondValue, thirdValue);
        }

        public static List<T> Clone<T>(this List<T> listToClone)  
        {  
            List<T> cloneList = new List<T>();
            foreach (var item in listToClone)
            {
                cloneList.Add(item);
            }
            return cloneList;
        }
        
        public static T GetRandomElementFromList<T>(this List<T> list)
        {
            int rnd = Random.Range(0, list.Count);
            return list[rnd];
        }

        public static IEnumerable<IEnumerable<int>> ConsecutiveSequences(this IEnumerable<int> input, int minLength = 3)
        {
            int order = 0;
            var inorder = new SortedSet<int>(input);
            return from item in new[] { new { order = 0, val = inorder.First() } }
                    .Concat(
                        inorder.Zip(inorder.Skip(1), (x, val) =>
                            new { order = x + 1 == val ? order : ++order, val }))
                group item.val by item.order into list
                where list.Count() >= minLength
                select list;
        }
        
        public static List<List<CardCurveValue>> ConsecutiveSequence(this List<CardCurveValue> input, int minLength = 3)
        {
            input = input.OrderBy(x => x.CurrentCard.CardData.CardValue.Value).ToList();
            List<List<CardCurveValue>> consecutiveList = new List<List<CardCurveValue>>();
            List<CardCurveValue> tempList = new List<CardCurveValue>();
            int consecutiveCounter = 0;
            int index = 0;
            while (input.Count > 0)
            {
                if (input[index+1].CurrentCard.CardData.CardValue.Value - input[index].CurrentCard.CardData.CardValue.Value == 1)
                {
                    consecutiveCounter++;
                    tempList.Add(input[index]);
                }
                else
                {
                    if (consecutiveCounter >= minLength - 1)
                    {
                        tempList.Add(input[index+1]);
                        consecutiveList.Add(tempList);
                        tempList.Clear();
                        index = 0;
                    }

                    consecutiveCounter = 0;
                }

                input.Remove(input[index]);
                index++;
            }

            return consecutiveList;
        }
    }
}
