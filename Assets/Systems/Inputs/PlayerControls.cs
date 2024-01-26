using FGJ24.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FGJ24.Player
{
    public class PlayerControls : MonoBehaviour
    {
        public static PlayerControls Instance { get; private set; }

        private PlayerInputActions _playerActions;

        public MoveData moveData;
        public LookData lookData;
        public FireData fireData;
        public AimData aimData;
        public InteractData interactData;
        public JumpData jumpData;
        public DashData dashData;
        public StartData startData;
        public PauseData pauseData;

        public MenuDirectionData menuDirectionData;
        public MenuInteractData menuInteractData;

        public struct MoveData
        {
            public bool moveStarted;
            public bool movePerformed;
            public bool moveCanceled;
            public Vector2 moveValue;
        }

        public struct LookData
        {
            public bool lookStarted;
            public bool lookPerformed;
            public bool lookCanceled;
            public Vector2 lookValue;
        }

        public struct FireData
        {
            public bool fireStarted;
            public bool firePerformed;
            public bool fireCanceled;
        }

        public struct AimData
        {
            public bool aimStarted;
            public bool aimPerformed;
            public bool aimCanceled;
        }

        public struct InteractData
        {
            public bool interactStarted;
            public bool interactPerformed;
            public bool interactCanceled;
        }

        public struct JumpData
        {
            public bool jumpStarted;
            public bool jumpPerformed;
            public bool jumpCanceled;
        }

        public struct DashData
        {
            public bool dashStarted;
            public bool dashPerformed;
            public bool dashCanceled;
        }

        public struct StartData
        {
            public bool startStarted;
            public bool startPerformed;
            public bool startCanceled;
        }

        public struct PauseData
        {
            public bool pauseStarted;
            public bool pausePerformed;
            public bool pauseCanceled;
        }

        public struct MenuDirectionData
        {
            public bool menuDirectionStarted;
            public bool menuDirectionPerformed;
            public bool menuDirectionCanceled;
            public Vector2 menuDirectionValue;
        }

        public struct MenuInteractData
        {
            public bool menuInteractStarted;
            public bool menuInteractPerformed;
            public bool menuInteractCanceled;
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

            _playerActions.Player.Enable();
            SubscribeControls();
        }

        public void SubscribeMenu(GameSceneUI ui)
        {
            _playerActions.Player.Start.started += ui.ToggleMenu;
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

            _playerActions.Player.Start.started += StartInput;
            _playerActions.Player.Start.performed += StartInput;
            _playerActions.Player.Start.canceled += StartInput;

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

            _playerActions.Player.Start.started -= StartInput;
            _playerActions.Player.Start.performed -= StartInput;
            _playerActions.Player.Start.canceled -= StartInput;

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
            if (_playerActions.Menu.enabled)
                _playerActions.Menu.Disable();

            if (!_playerActions.Player.enabled)
                _playerActions.Player.Enable();
        }

        private void SwitchToMenuActionMap()
        {
            //Disable all action maps
            if (_playerActions.Player.enabled)
                _playerActions.Player.Disable();

            if (!_playerActions.Menu.enabled)
                _playerActions.Menu.Enable();
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            moveData = new MoveData
            {
                moveStarted = ctx.started,
                movePerformed = ctx.performed,
                moveCanceled = ctx.canceled,
                moveValue = ctx.canceled ? Vector2.zero : ctx.ReadValue<Vector2>()
            };
        }

        private void Look(InputAction.CallbackContext ctx)
        {
            lookData = new LookData
            {
                lookStarted = ctx.started,
                lookPerformed = ctx.performed,
                lookCanceled = ctx.canceled,
                lookValue = ctx.canceled ? Vector2.zero : ctx.ReadValue<Vector2>()
            };

        }

        private void Fire(InputAction.CallbackContext ctx)
        {
            fireData = new FireData
            {
                fireStarted = ctx.started,
                firePerformed = ctx.performed,
                fireCanceled = ctx.canceled
            };
        }

        private void Aim(InputAction.CallbackContext ctx)
        {
            aimData = new AimData
            {
                aimStarted = ctx.started,
                aimPerformed = ctx.performed,
                aimCanceled = ctx.canceled
            };
        }

        private void Interact(InputAction.CallbackContext ctx)
        {
            interactData = new InteractData
            {
                interactStarted = ctx.started,
                interactPerformed = ctx.performed,
                interactCanceled = ctx.canceled
            };
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            jumpData = new JumpData
            {
                jumpStarted = ctx.started,
                jumpPerformed = ctx.performed,
                jumpCanceled = ctx.canceled
            };
        }

        private void Dash(InputAction.CallbackContext ctx)
        {
            dashData = new DashData
            {
                dashStarted = ctx.started,
                dashPerformed = ctx.performed,
                dashCanceled = ctx.canceled
            };
        }

        private void StartInput(InputAction.CallbackContext ctx)
        {
            startData = new StartData
            {
                startStarted = ctx.started,
                startPerformed = ctx.performed,
                startCanceled = ctx.canceled
            };
        }

        private void Pause(InputAction.CallbackContext ctx)
        {
            pauseData = new PauseData
            {
                pauseStarted = ctx.started,
                pausePerformed = ctx.performed,
                pauseCanceled = ctx.canceled
            };
        }

        private void MenuDirection(InputAction.CallbackContext ctx)
        {
            menuDirectionData = new MenuDirectionData
            {
                menuDirectionStarted = ctx.started,
                menuDirectionPerformed = ctx.performed,
                menuDirectionCanceled = ctx.canceled,
                menuDirectionValue = ctx.canceled ? Vector2.zero : ctx.ReadValue<Vector2>()
            };
        }

        private void MenuInteract(InputAction.CallbackContext ctx)
        {
            menuInteractData = new MenuInteractData
            {
                menuInteractStarted = ctx.started,
                menuInteractPerformed = ctx.performed,
                menuInteractCanceled = ctx.canceled
            };
        }


    }
}