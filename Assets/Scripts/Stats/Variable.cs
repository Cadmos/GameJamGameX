using System;
using UnityEngine;

namespace FGJ24.Stats
{
    [Serializable]
    public class VariableBase
    {
        
    }
    
    [Serializable]
    public class Variable<T> : Variable
    {
        
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set => _value = value;
        }
        
        public Variable(T value)
        {
            _value = value;
        }

        public static implicit operator Variable<T>(T s) => new (s);
        public static implicit operator T(Variable<T> s) => s._value;
        
        public override string ToString() => $"{_value}";

        public override VariableType VariableConversion => VariableTypeForThis();

        private VariableType VariableTypeForThis()
        {
            if (typeof(T) == typeof(int))
            {
                return VariableType.Int;
            }
            else if (typeof(T) == typeof(string))
            {
                return VariableType.String;
            }
            else if (typeof(T) == typeof(float))
            {
                return VariableType.Float;
            }
            else if (typeof(T) == typeof(bool))
            {
                return VariableType.Bool;
            }

            return VariableType.Int;
        } 
    }
}
