using UnityEngine;

namespace FGJ24.CustomPhysics
{
    public class GravitySphere : GravitySource {

        [SerializeField] private float _gravity = 9.81f;

        [SerializeField, Min(0f)]
        private float _outerRadius = 10f;

        [SerializeField, Min(0f)]
        private float _outerFalloffRadius = 15f;

        [SerializeField, Min(0f)]
        private float _innerFalloffRadius = 1f;

        [SerializeField, Min(0f)]
        private float _innerRadius = 5f;

        private float _outerFalloffFactor, _innerFalloffFactor;
	
        void Awake () {
            OnValidate();
        }

        void OnValidate () {
            _innerFalloffRadius = Mathf.Max(_innerFalloffRadius, 0f);
            _innerRadius = Mathf.Max(_innerRadius, _innerFalloffRadius);
            _outerRadius = Mathf.Max(_outerRadius, _innerRadius);
            _outerFalloffRadius = Mathf.Max(_outerFalloffRadius, _outerRadius);
            
            _innerFalloffFactor = 1f / (_innerRadius - _innerFalloffRadius);
            _outerFalloffFactor = 1f / (_outerFalloffRadius - _outerRadius);
        }
        
        public override Vector3 GetGravity (Vector3 position) {
            
            Vector3 vector = transform.position - position;
            float distance = vector.magnitude;
            if (distance > _outerFalloffRadius || distance < _innerFalloffRadius) {
                return Vector3.zero;
            }
            float g = _gravity/distance;
            if (distance > _outerRadius) {
                g *= 1f - (distance - _outerRadius) * _outerFalloffFactor;
            }
            else if(distance < _innerRadius){
                g *= 1f - (_innerRadius - distance) * _innerFalloffFactor;
            }
            return g * vector;
        }
        
        void OnDrawGizmos () {
            Vector3 p = transform.position;
            if (_innerFalloffRadius > 0f && _innerFalloffRadius < _innerRadius) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, _innerFalloffRadius);
            }
            Gizmos.color = Color.yellow;
            if (_innerRadius > 0f && _innerRadius < _outerRadius) {
                Gizmos.DrawWireSphere(p, _innerRadius);
            }
            Gizmos.DrawWireSphere(p, _outerRadius);
            if (_outerFalloffRadius > _outerRadius) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, _outerFalloffRadius);
            }
        }
    }
}