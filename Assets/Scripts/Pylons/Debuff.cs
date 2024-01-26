using UnityEngine;

namespace FGJ24.Pylons
{
    public enum DebuffType
    {
        DamageInfliction,
        Slow
    }
    
    [System.Serializable]
    public class Debuff
    {
        public string Name;
        public DebuffType DebuffType;
        protected Debuff(string name, DebuffType debuffType)
        {
            Name = name;
            DebuffType = debuffType;
        }
    }

    [System.Serializable]
    public class DamageInfliction : Debuff
    {
        public int DamageAmount;

        public DamageInfliction(string name, int damageAmount) : base(name, DebuffType.DamageInfliction)
        {
            DamageAmount = damageAmount;
        }
    }

    [System.Serializable]
    public class Slow : Debuff
    {
        public float MovementReduction;
        public Slow(string name, float moveReduction) : base(name, DebuffType.Slow)
        {
            MovementReduction = moveReduction;
        }
    }
}
