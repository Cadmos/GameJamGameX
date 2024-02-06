using System;
using FGJ24.Stats;
using UnityEngine;

namespace FGJ24.Stats
{
    [Serializable]
    public enum VariableType
    {
        Int,
        Float,
        String,
        Bool,
    }
    
    [Serializable]
    public abstract class Variable
    {
        public virtual VariableType VariableConversion => VariableType.Int;
    }
    
    [Serializable]
    public class IntVariable : Variable<int>, IVariable
    {
        [SerializeField] private IntVar defaultValue;
        public IntVariable(int value) : base(value)
        {
            
        }

        public void SetDefaultValue(IntVar value)
        {
            defaultValue = value;
        }
    }
    
    [Serializable]
    public class FloatVariable : Variable<float>, IVariable
    {
        [SerializeField] private FloatVar defaultValue;
        public FloatVariable(float value) : base(value)
        {
            
        }

        public void SetDefaultValue(FloatVar value)
        {
            defaultValue = value;
        }
    }
    
    [Serializable]
    public class StringVariable : Variable<string>, IVariable
    {
        [SerializeField] private StringVar defaultValue;
        public StringVariable(string value) : base(value)
        {
            
        }

        public void SetDefaultValue(StringVar value)
        {
            defaultValue = value;
        }
    }
    
    [Serializable]
    public class BoolVariable : Variable<bool>, IVariable
    {
        [SerializeField] private BoolVar defaultValue;
        public BoolVariable(bool value) : base(value)
        {
            
        }

        public void SetDefaultValue(BoolVar value)
        {
            defaultValue = value;
        }
    }
}
