using System;
using System.Collections.Generic;
using System.Linq;
using FGJ24.Pylons;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Player
{
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        [SerializeField] private List<Debuff> debuffs;

        [SerializeField] private float damageInterval;
        
        [SerializeField] private float healInterval;
        
        [SerializeField] private PlayerObject playerObject;
        private void Awake()
        {
            _playerData = GetComponent<PlayerData>();
        }

        private void Start()
        {
            InvokeRepeating("TakeDamage", 0.1f, damageInterval);
            InvokeRepeating("HealDebuffs", 0.1f, healInterval);
        }

        public void InflictDamageDebuff(Debuff debuff)
        {
            debuffs.Add(debuff);
        }

        private void HealDebuffs()
        {
            if (debuffs.Count < 1) return;
            
            var damageDebuffToHeal = debuffs.Find(d => d.DebuffType == DebuffType.DamageInfliction);
            debuffs.Remove(damageDebuffToHeal);
            
            var slowDebuffToHeal = debuffs.Find(d => d.DebuffType == DebuffType.Slow);
            debuffs.Remove(slowDebuffToHeal);
        }

        private void TakeDamage()
        {
            if (debuffs.Count < 1) return;
            
            var totalAmount = 0;

            debuffs.ForEach(d =>
            {
                if (d.DebuffType == DebuffType.DamageInfliction)
                {
                    totalAmount += ((DamageInfliction)d).DamageAmount;
                }
            });
            _playerData.TakeDamage(totalAmount);
        }

        public float GetMovementReduction()
        {
            var totalAmount = 0f;

            debuffs.ForEach(d =>
            {
                if (d.DebuffType == DebuffType.Slow)
                {
                    totalAmount += ((Slow)d).MovementReduction;
                }
            });
            
            if (totalAmount < 0.5f) totalAmount = 0.5f;
            if (totalAmount > 2.5f) totalAmount = 2.5f;
            
            return totalAmount;
        }
    }
}