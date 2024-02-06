using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace FGJ24.Stats
{
    public static class VariableHelpers
    {
        public static List<T> VarsOfType<T>(this List<Var> list) where T : Var
        {
            List<T> newList = new List<T>();
            
            foreach (var var in list)
            {
                if (var.GetType() == typeof(T))
                {
                    newList.Add((T) var);
                }
            }

            return newList;
        }
    }
}
