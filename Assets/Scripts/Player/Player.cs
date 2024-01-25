using UnityEngine;

namespace FGJ24.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerObject _playerObject;
    }
}