using System;
using UnityEngine;

namespace FGJ24.Stats
{
    public class Stats
    {
        //private Stat<int> level = 1;
    }
    
    [Serializable]
    public class IntStat : Stat<int> {
        public IntStat(int value) : base(value)
        {
            
        }
    }
    

    public class IntAttribute
    {
        private int value;

        public IntAttribute(int s)
        {
            value = s;
        }
        
        public static implicit operator int(IntAttribute s) => s.value;
        public static explicit operator IntAttribute(int i) => new IntAttribute(i);
    }
}
