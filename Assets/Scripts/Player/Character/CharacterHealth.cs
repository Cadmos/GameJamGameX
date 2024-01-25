using System;
using UnityEngine;

namespace FGJ24.Player
{
    [Serializable]
    public class CharacterHealth
    {
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _gameOverHealth;
        
        public int GetCurrentHealth()
        {
            return _currentHealth;
        }
        
        public int GetMaxHealth()
        {
            return _maxHealth;
        }
        
        public int GetGameOverHealth()
        {
            return _gameOverHealth;
        }
    }
}