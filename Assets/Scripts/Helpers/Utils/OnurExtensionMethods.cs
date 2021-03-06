﻿using System;
using System.Collections.Generic;
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
    }
}
