namespace FGJ24.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        private PlayerCharacter _character;
        private PlayerController _controller;
        
        public PlayerMoveState(PlayerCharacter character, PlayerController controller)
        {
            _character = character;
            _controller = controller;
        }
        public override void EnterState(PlayerStateManager player)
        {
            
        }
        public override void UpdateState(PlayerStateManager player)
        {
            
        }
        public override void FixedUpdateState(PlayerStateManager player)
        {
            
        }
        public override void LateUpdateState(PlayerStateManager player)
        {
            
        }
    }
}