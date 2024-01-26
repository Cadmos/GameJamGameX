using UnityEngine;

namespace FGJ24.ScriptableObjects.Pylon
{
    [CreateAssetMenu(menuName = "Curves/2D Curve")]
    public class PylonConfiguration : ScriptableObject
    {
        [SerializeField] private AnimationCurve triggerInterval;
        
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        
        public AnimationCurve GetAnimationCurve()
        {
            return triggerInterval;
        }
    }
}
