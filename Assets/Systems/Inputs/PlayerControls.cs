using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }
    
    private PlayerInputActions _playerActions;
    
    private MoveData _moveData;
    private LookData _lookData;
    private FireData _fireData;
    private AimData _aimData;
    private InteractData _interactData;
    private JumpData _jumpData;
    private DashData _dashData;
    private StartData _startData;
    private PauseData _pauseData;
    
    private MenuDirectionData _menuDirectionData;
    private MenuInteractData _menuInteractData;
    
    
    struct MoveData
    {
        public bool MoveStarted;
        public bool MovePerformed;
        public bool MoveCanceled;
        public Vector2 MoveValue;
    }
    
    struct LookData
    {
        public bool LookStarted;
        public bool LookPerformed;
        public bool LookCanceled;
        public Vector2 LookValue;
    }
    
    struct FireData
    {
        public bool FireStarted;
        public bool FirePerformed;
        public bool FireCanceled;
    }
    
    struct AimData
    {
        public bool AimStarted;
        public bool AimPerformed;
        public bool AimCanceled;
    }
    
    struct InteractData
    {
        public bool InteractStarted;
        public bool InteractPerformed;
        public bool InteractCanceled;
    }
    
    struct JumpData
    {
        public bool JumpStarted;
        public bool JumpPerformed;
        public bool JumpCanceled;
    }
    
    struct DashData
    {
        public bool DashStarted;
        public bool DashPerformed;
        public bool DashCanceled;
    }
    
    struct StartData
    {
        public bool StartStarted;
        public bool StartPerformed;
        public bool StartCanceled;
    }
    
    struct PauseData
    {
        public bool PauseStarted;
        public bool PausePerformed;
        public bool PauseCanceled;
    }
    
    struct MenuDirectionData
    {
        public bool MenuDirectionStarted;
        public bool MenuDirectionPerformed;
        public bool MenuDirectionCanceled;
        public Vector2 MenuDirectionValue;
    }
    
    struct MenuInteractData
    {
        public bool MenuInteractStarted;
        public bool MenuInteractPerformed;
        public bool MenuInteractCanceled;
    }
    
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _playerActions = new PlayerInputActions();
        
        SubscribeControls();
    }
    
    private void SubscribeControls()
    {
        _playerActions.Player.Move.started += Move;
        _playerActions.Player.Move.performed += Move;
        _playerActions.Player.Move.canceled += Move;
        
        _playerActions.Player.Look.started += Look;
        _playerActions.Player.Look.performed += Look;
        _playerActions.Player.Look.canceled += Look;
        
        _playerActions.Player.Fire.started += Fire;
        _playerActions.Player.Fire.performed += Fire;
        _playerActions.Player.Fire.canceled += Fire;
        
        _playerActions.Player.Aim.started += Aim;
        _playerActions.Player.Aim.performed += Aim;
        _playerActions.Player.Aim.canceled += Aim;
        
        _playerActions.Player.Interact.started += Interact;
        _playerActions.Player.Interact.performed += Interact;
        _playerActions.Player.Interact.canceled += Interact;
                
        _playerActions.Player.Jump.started += Jump;
        _playerActions.Player.Jump.performed += Jump;
        _playerActions.Player.Jump.canceled += Jump;
        
        _playerActions.Player.Dash.started += Dash;
        _playerActions.Player.Dash.performed += Dash;
        _playerActions.Player.Dash.canceled += Dash;
        
        _playerActions.Player.Start.started += Start;
        _playerActions.Player.Start.performed += Start;
        _playerActions.Player.Start.canceled += Start;
        
        _playerActions.Player.Pause.started += Pause;
        _playerActions.Player.Pause.performed += Pause;
        _playerActions.Player.Pause.canceled += Pause;

        _playerActions.Menu.MenuDirection.started += MenuDirection;
        _playerActions.Menu.MenuDirection.performed += MenuDirection;
        _playerActions.Menu.MenuDirection.canceled += MenuDirection;
        
        _playerActions.Menu.MenuInteract.started += MenuInteract;
        _playerActions.Menu.MenuInteract.performed += MenuInteract;
        _playerActions.Menu.MenuInteract.canceled += MenuInteract;
    }

    private void UnsubscribeControls()
    {
        _playerActions.Player.Move.started -= Move;
        _playerActions.Player.Move.performed -= Move;
        _playerActions.Player.Move.canceled -= Move;
        
        _playerActions.Player.Look.started -= Look;
        _playerActions.Player.Look.performed -= Look;
        _playerActions.Player.Look.canceled -= Look;
        
        _playerActions.Player.Fire.started -= Fire;
        _playerActions.Player.Fire.performed -= Fire;
        _playerActions.Player.Fire.canceled -= Fire;
        
        _playerActions.Player.Aim.started -= Aim;
        _playerActions.Player.Aim.performed -= Aim;
        _playerActions.Player.Aim.canceled -= Aim;
        
        _playerActions.Player.Interact.started -= Interact;
        _playerActions.Player.Interact.performed -= Interact;
        _playerActions.Player.Interact.canceled -= Interact;
        
        _playerActions.Player.Jump.started -= Jump;
        _playerActions.Player.Jump.performed -= Jump;
        _playerActions.Player.Jump.canceled -= Jump;
        
        _playerActions.Player.Dash.started -= Dash;
        _playerActions.Player.Dash.performed -= Dash;
        _playerActions.Player.Dash.canceled -= Dash;
        
        _playerActions.Player.Start.started -= Start;
        _playerActions.Player.Start.performed -= Start;
        _playerActions.Player.Start.canceled -= Start;
        
        _playerActions.Player.Pause.started -= Pause;
        _playerActions.Player.Pause.performed -= Pause;
        _playerActions.Player.Pause.canceled -= Pause;
        
        _playerActions.Menu.MenuDirection.started -= MenuDirection;
        _playerActions.Menu.MenuDirection.performed -= MenuDirection;
        _playerActions.Menu.MenuDirection.canceled -= MenuDirection;
        
        _playerActions.Menu.MenuInteract.started -= MenuInteract;
        _playerActions.Menu.MenuInteract.performed -= MenuInteract;
        _playerActions.Menu.MenuInteract.canceled -= MenuInteract;
    }
    
    private void EnableControls()
    {
        _playerActions.Enable();
    }
    
    private void DisableControls()
    {
        _playerActions.Disable();
    }
    
    private void SwitchToPlayerActionMap()
    {
        //Disable all action maps
        if(_playerActions.Menu.enabled) 
            _playerActions.Menu.Disable();
        
        if(!_playerActions.Player.enabled) 
            _playerActions.Player.Enable();
    }
    
    private void SwitchToMenuActionMap()
    {
        //Disable all action maps
        if(_playerActions.Player.enabled) 
            _playerActions.Player.Disable();
        
        if(!_playerActions.Menu.enabled) 
            _playerActions.Menu.Enable();
    }
    
    private void Move(InputAction.CallbackContext ctx)
    {
        _moveData = new MoveData
        {
            MoveStarted = ctx.started,
            MovePerformed = ctx.performed,
            MoveCanceled = ctx.canceled,
            MoveValue = ctx.canceled ? Vector2.zero : ctx.ReadValue<Vector2>()
        };
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        _lookData = new LookData
        {
            LookStarted = ctx.started,
            LookPerformed = ctx.performed,
            LookCanceled = ctx.canceled,
            LookValue = ctx.canceled ? Vector2.zero : ctx.ReadValue<Vector2>()
        };

    }

    private void Fire(InputAction.CallbackContext ctx)
    {
        _fireData = new FireData
        {
            FireStarted = ctx.started,
            FirePerformed = ctx.performed,
            FireCanceled = ctx.canceled
        };
    }
    
    private void Aim(InputAction.CallbackContext ctx)
    {
        _aimData = new AimData
        {
            AimStarted = ctx.started,
            AimPerformed = ctx.performed,
            AimCanceled = ctx.canceled
        };
    }
    
    private void Interact(InputAction.CallbackContext ctx)
    {
        _interactData = new InteractData
        {
            InteractStarted = ctx.started,
            InteractPerformed = ctx.performed,
            InteractCanceled = ctx.canceled
        };
    }
    
    private void Jump(InputAction.CallbackContext ctx)
    {
        _jumpData = new JumpData
        {
            JumpStarted = ctx.started,
            JumpPerformed = ctx.performed,
            JumpCanceled = ctx.canceled
        };
    }
    
    private void Dash(InputAction.CallbackContext ctx)
    {
        _dashData = new DashData
        {
            DashStarted = ctx.started,
            DashPerformed = ctx.performed,
            DashCanceled = ctx.canceled
        };
    }
    
    private void Start(InputAction.CallbackContext ctx)
    {
        _startData = new StartData
        {
            StartStarted = ctx.started,
            StartPerformed = ctx.performed,
            StartCanceled = ctx.canceled
        };
    }
    
    private void Pause(InputAction.CallbackContext ctx)
    {
        _pauseData = new PauseData
        {
            PauseStarted = ctx.started,
            PausePerformed = ctx.performed,
            PauseCanceled = ctx.canceled
        };
    }
    
    private void MenuDirection(InputAction.CallbackContext ctx)
    {
        _menuDirectionData = new MenuDirectionData
        {
            MenuDirectionStarted = ctx.started,
            MenuDirectionPerformed = ctx.performed,
            MenuDirectionCanceled = ctx.canceled,
            MenuDirectionValue = ctx.canceled ? Vector2.zero : ctx.ReadValue<Vector2>()
        };
    }
    
    private void MenuInteract(InputAction.CallbackContext ctx)
    {
        _menuInteractData = new MenuInteractData
        {
            MenuInteractStarted = ctx.started,
            MenuInteractPerformed = ctx.performed,
            MenuInteractCanceled = ctx.canceled
        };
    }
    
    
}