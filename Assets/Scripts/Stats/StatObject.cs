using UnityEngine;

namespace FGJ24.Stats
{
    [CreateAssetMenu(menuName = "Stats/Stat")]
    public class StatObject : ScriptableObject
    {
        [SerializeField] private int _health;

        public Stat<int> Health
        {
            get => _health;
            set => _health = value;
        }
    }
}
