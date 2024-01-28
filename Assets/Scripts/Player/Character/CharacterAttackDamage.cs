using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterAttackDamage
    {
        [FormerlySerializedAs("_attackDamage")] [SerializeField] private int attackDamage = 10;
    }
}