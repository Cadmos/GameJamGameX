using Ioni;
using UnityEngine;

namespace FGJ24.Stats
{
    public class StatDebugObject : MonoBehaviour
    {
        [SerializeField] private IntVariable health;
        [SerializeField] private FloatVariable moveSpeed;
        [SerializeField] private StringVariable playerName;
        [SerializeField] private BoolVariable isFlying;

        [SerializeField] private IntVar healthVar;
        [SerializeField] private FloatVar moveSpeedVar;
        [SerializeField] private StringVar playerNameVar;
        [SerializeField] private BoolVar isFlyingVar;
    }
}
