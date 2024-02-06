using FGJ24.Stats;
using Ioni.Extensions;
using UnityEngine;

namespace FGJ24.Stats
{
    public abstract class Var : ScriptableObject
    {
        public virtual VariableType GetType => VariableType.Int;
    }

    public interface IVar
    {
        public IVariable Variable { get; }
    } 
    
    [CreateAssetMenu(menuName = "Variables/IntVar", fileName = "New Int")]
    public class IntVar : Var, IVar
    {
        [SerializeField] private int defaultValue;
        
        public Stat<int> DefaultValue => defaultValue;
        public IVariable Variable => new IntVariable(defaultValue);

        public override VariableType GetType => VariableType.Int;

    }
    
    [CreateAssetMenu(menuName = "Variables/FloatVar", fileName = "New Float")]
    public class FloatVar : Var, IVar
    {
        [SerializeField] private float defaultValue;
        
        public Stat<float> DefaultValue => defaultValue;
        public IVariable Variable => new FloatVariable(defaultValue);

        public override VariableType GetType => VariableType.Float;
    }
    
    [CreateAssetMenu(menuName = "Variables/StringVar", fileName = "New String")]
    public class StringVar : Var, IVar
    {
        [SerializeField] private string defaultValue;
        
        public Stat<string> DefaultValue => defaultValue;
        public IVariable Variable => new StringVariable(defaultValue);

        public override VariableType GetType => VariableType.String;
    }
    
    [CreateAssetMenu(menuName = "Variables/BoolVar", fileName = "New Bool")]
    public class BoolVar : Var, IVar
    {
        [SerializeField] private bool defaultValue;
        
        public Stat<bool> DefaultValue => defaultValue;
        public IVariable Variable => new BoolVariable(defaultValue);

        public override VariableType GetType => VariableType.Bool;
    }
}
