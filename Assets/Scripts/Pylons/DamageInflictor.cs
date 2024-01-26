using System;
using Ioni.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;


namespace FGJ24.Pylons
{
    [RequireComponent(typeof(Collider))]
    public class DamageInflictor : MonoBehaviour
    {
        private Player.Player playerToDamage;

        [SerializeField] private int minBurnDamage;
        [SerializeField] private int maxBurnDamage;
        [SerializeField] private float damageInterval;

        private void TryInflictDebuff()
        {
            if (playerToDamage != null)
            {
                playerToDamage.InflictDamageDebuff(new DamageInfliction("Burn!", Random.Range(minBurnDamage,maxBurnDamage)));
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            playerToDamage = other.GetComponent<Player.Player>();
            InvokeRepeating("TryInflictDebuff", 0.1f, damageInterval);
        }

        private void OnTriggerExit(Collider other)
        {
            CancelInvoke("TryInflictDebuff");
            playerToDamage = null;
        }
    }
}
