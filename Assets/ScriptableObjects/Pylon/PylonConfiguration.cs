using UnityEngine;

namespace FGJ24.ScriptableObjects.Pylon
{
    [CreateAssetMenu(menuName = "Curves/2D Curve")]
    public class PylonConfiguration : ScriptableObject
    {
        [SerializeField] private AnimationCurve triggerInterval;
        
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        
        [SerializeField] private float pulseTimeModifier;
        [SerializeField] private Vector2 pulseTimeModificationRange; 
        
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        
        public float PulseTimeModifier => pulseTimeModifier;
        public Vector2 PulseTimeModificationRange => pulseTimeModificationRange;
        
        public AnimationCurve GetAnimationCurve()
        {
            return triggerInterval;
        }
    }
}
