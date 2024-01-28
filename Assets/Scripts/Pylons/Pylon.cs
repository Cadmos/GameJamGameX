using System;
using FGJ24.ScriptableObjects.Pylon;
using UnityEngine;
using UnityEngine.Serialization;

namespace FGJ24.Pylons
{
    public class Pylon : MonoBehaviour
    {
        [SerializeField] private PylonConfiguration pylonConfiguration;

        [SerializeField] private float currentValue;

        [SerializeField] private float time;

        [SerializeField] private bool growing = true;

        [SerializeField] private float timeModifier;
        private void Start()
        {
            time = pylonConfiguration.MinDistance;
        }

        public void Update()
        {
            CalculateModifierForTrigger();
        }

        private void CalculateModifierForTrigger()
        {
            time += Time.deltaTime * (growing ? -1 : 1) + timeModifier;
            
            if (time > pylonConfiguration.MaxDistance || time < pylonConfiguration.MinDistance)
            {
                growing = !growing;
            }
            
            currentValue = pylonConfiguration.GetAnimationCurve().Evaluate(time / pylonConfiguration.MaxDistance);
        }

        public float CurrentValue => currentValue;
    }
}
