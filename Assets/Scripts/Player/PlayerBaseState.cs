using UnityEngine;

namespace FGJ24.Player
{
    public abstract class PlayerBaseState
    {
        protected CharacterObject _character;
        protected PlayerController _controller;

        protected static readonly int StateEnum = Animator.StringToHash("StateEnum");
        protected static readonly int DashAnimationSpeed = Animator.StringToHash("DashAnimationSpeed");
        protected PlayerBaseState(CharacterObject character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        public abstract void EnterState(PlayerStateManager player);
        public abstract void UpdateState(PlayerStateManager player);
        public abstract void FixedUpdateState(PlayerStateManager player);
        public abstract void LateUpdateState(PlayerStateManager player);
        public abstract void ExitState(PlayerStateManager player);
    }
}