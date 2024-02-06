using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace FGJ24.Stats
{
    [Serializable]
    public class Stat<T>
    {
        
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set => _value = value;
        }
        
        public Stat(T value)
        {
            _value = value;
        }

        public static implicit operator Stat<T>(T s) => new Stat<T>(s);
        public static implicit operator T(Stat<T> s) => s._value;
        //public static explicit operator Stat<T>(T i) => new (i);
        
        public override string ToString() => $"{_value}";

    }

    public class LimitedStat<T> : Stat<T> where T : IComparable<T>
    {
        private T _min;
        private T _max;

        public T Min
        {
            get => _min;
            set => _min = value;
        }
        
        public T Max
        {
            get => _max;
            set => _max = value;
        }
        
        public LimitedStat(T value, T min, T max) : base(value)
        {
            _min = min;
            _max = max;
        }
        
        public T LimitedValue()
        {
            var comparer = Comparer<T>.Default; // need to set this ?
            var minCompare = Value.CompareTo(_min) <= 0 ? _min : Value;
            var maxCompare = minCompare.CompareTo(_max) >= 0 ? _max : Value;
            
            return maxCompare;
        }
    }
}
