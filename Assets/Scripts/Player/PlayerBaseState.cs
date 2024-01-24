namespace FGJ24.Player
{
    public abstract class PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        public abstract void EnterState(PlayerStateManager player);
        public abstract void UpdateState(PlayerStateManager player);
        public abstract void FixedUpdateState(PlayerStateManager player);
        public abstract void LateUpdateState(PlayerStateManager player);
    }
}