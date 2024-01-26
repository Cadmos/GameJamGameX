using UnityEngine;
using Random = UnityEngine.Random;


namespace FGJ24.Pylons
{
    [RequireComponent(typeof(Collider))]
    public class SlowInflictor : MonoBehaviour
    {
        private Player.Player _playerToSlow;

        [SerializeField] private float movementReduction;
        [SerializeField] private float slowInterval;

        private void TryInflictDebuff()
        {
            if (_playerToSlow != null)
            {
                _playerToSlow.InflictDamageDebuff(new Slow("Slow", movementReduction));
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _playerToSlow = other.GetComponent<Player.Player>();
            InvokeRepeating("TryInflictDebuff", 0.1f, slowInterval);
        }

        private void OnTriggerExit(Collider other)
        {
            CancelInvoke("TryInflictDebuff");
            _playerToSlow = null;
        }
    }
}