using System;
using FGJ24.Interactions;
using Ioni;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FGJ24.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;

        [SerializeField] private Slider slider;

        private void Start()
        {
            _currentHealth = _maxHealth;
            slider.value = _currentHealth;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;

            if (_currentHealth < 0)
            {
                slider.value = 0;
                WinSceneParams.Won = false;
                SceneManager.LoadSceneAsync("TittelScene");
            }
            
            slider.value = _currentHealth;
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;

            if (_currentHealth > _maxHealth)
            {
                slider.value = 1000; // Slider value is fixed atm
            }
            
            slider.value = _currentHealth;
        }
        
        public void AddMaxHealth(int amount)
        {
            _maxHealth += amount;
        }
    }
    
    public class IntStat
    {
        private int _value;

        public int Value => _value < 0 ? 0 : _value;
    } 
}
