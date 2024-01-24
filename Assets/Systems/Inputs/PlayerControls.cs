using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }
    
    private PlayerInputActions _playerActions;
    
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
        _playerActions.Player.Move.started += MoveStarted;
        _playerActions.Player.Move.performed += MovePerformed;
        _playerActions.Player.Move.canceled += MoveCanceled;
        
        _playerActions.Player.Look.started += LookStarted;
        _playerActions.Player.Look.performed += LookPerformed;
        _playerActions.Player.Look.canceled += LookCanceled;
        
        _playerActions.Player.Fire.started += FireStarted;
        _playerActions.Player.Fire.performed += FirePerformed;
        _playerActions.Player.Fire.canceled += FireCanceled;
        
        _playerActions.Player.Aim.started += AimStarted;
        _playerActions.Player.Aim.performed += AimPerformed;
        _playerActions.Player.Aim.canceled += AimCanceled;
        
        _playerActions.Player.Interact.started += InteractStarted;
        _playerActions.Player.Interact.performed += InteractPerformed;
        _playerActions.Player.Interact.canceled += InteractCanceled;
                
        _playerActions.Player.Jump.started += JumpStarted;
        _playerActions.Player.Jump.performed += JumpPerformed;
        _playerActions.Player.Jump.canceled += JumpCanceled;
        
        _playerActions.Player.Dash.started += DashStarted;
        _playerActions.Player.Dash.performed += DashPerformed;
        _playerActions.Player.Dash.canceled += DashCanceled;
        
        _playerActions.Player.Start.started += StartStarted;
        _playerActions.Player.Start.performed += StartPerformed;
        _playerActions.Player.Start.canceled += StartCanceled;
        
        _playerActions.Player.Pause.started += PauseStarted;
        _playerActions.Player.Pause.performed += PausePerformed;
        _playerActions.Player.Pause.canceled += PauseCanceled;

        _playerActions.Menu.MenuDirection.started += MenuUpStarted;
        _playerActions.Menu.MenuDirection.performed += MenuUpPerformed;
        _playerActions.Menu.MenuDirection.canceled += MenuUpCanceled;
        
        _playerActions.Menu.MenuInteract.started += MenuInteractStarted;
        _playerActions.Menu.MenuInteract.performed += MenuInteractPerformed;
        _playerActions.Menu.MenuInteract.canceled += MenuInteractCanceled;
    }

    private void UnsubscribeControls()
    {
        _playerActions.Player.Move.started -= MoveStarted;
        _playerActions.Player.Move.performed -= MovePerformed;
        _playerActions.Player.Move.canceled -= MoveCanceled;
        
        _playerActions.Player.Look.started -= LookStarted;
        _playerActions.Player.Look.performed -= LookPerformed;
        _playerActions.Player.Look.canceled -= LookCanceled;
        
        _playerActions.Player.Fire.started -= FireStarted;
        _playerActions.Player.Fire.performed -= FirePerformed;
        _playerActions.Player.Fire.canceled -= FireCanceled;
        
        _playerActions.Player.Aim.started -= AimStarted;
        _playerActions.Player.Aim.performed -= AimPerformed;
        _playerActions.Player.Aim.canceled -= AimCanceled;
        
        _playerActions.Player.Interact.started -= InteractStarted;
        _playerActions.Player.Interact.performed -= InteractPerformed;
        _playerActions.Player.Interact.canceled -= InteractCanceled;
        
        _playerActions.Player.Jump.started -= JumpStarted;
        _playerActions.Player.Jump.performed -= JumpPerformed;
        _playerActions.Player.Jump.canceled -= JumpCanceled;
        
        _playerActions.Player.Dash.started -= DashStarted;
        _playerActions.Player.Dash.performed -= DashPerformed;
        _playerActions.Player.Dash.canceled -= DashCanceled;
        
        _playerActions.Player.Start.started -= StartStarted;
        _playerActions.Player.Start.performed -= StartPerformed;
        _playerActions.Player.Start.canceled -= StartCanceled;
        
        _playerActions.Player.Pause.started -= PauseStarted;
        _playerActions.Player.Pause.performed -= PausePerformed;
        _playerActions.Player.Pause.canceled -= PauseCanceled;
        
        _playerActions.Menu.MenuDirection.started -= MenuUpStarted;
        _playerActions.Menu.MenuDirection.performed -= MenuUpPerformed;
        _playerActions.Menu.MenuDirection.canceled -= MenuUpCanceled;
        
        _playerActions.Menu.MenuInteract.started -= MenuInteractStarted;
        _playerActions.Menu.MenuInteract.performed -= MenuInteractPerformed;
        _playerActions.Menu.MenuInteract.canceled -= MenuInteractCanceled;
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
    
    private void MoveStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("MoveStarted");
    }
    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("MovePerformed");
    }
    private void MoveCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("MoveCanceled");
    }
    
    private void LookStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("LookStarted");
    }
    private void LookPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("LookPerformed");
    }
    private void LookCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("LookCanceled");
    }
    
    private void FireStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("FireStarted");
    }
    private void FirePerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("FirePerformed");
    }
    private void FireCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("FireCanceled");
    }
    
    private void AimStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("AimStarted");
    }
    private void AimPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("AimPerformed");
    }
    private void AimCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("AimCanceled");
    }
    
    private void InteractStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("InteractStarted");
    }
    private void InteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("InteractPerformed");
    }
    private void InteractCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("InteractCanceled");
    }
    
    private void JumpStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("JumpStarted");
    }
    private void JumpPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("JumpPerformed");
    }
    private void JumpCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("JumpCanceled");
    }
    
    private void DashStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("DashStarted");
    }
    private void DashPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("DashPerformed");
    }
    private void DashCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("DashCanceled");
    }
    
    private void StartStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("StartStarted");
    }
    private void StartPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("StartPerformed");
    }
    private void StartCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("StartCanceled");
    }
    
    private void PauseStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("PauseStarted");
    }
    private void PausePerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("PausePerformed");
    }
    private void PauseCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("PauseCanceled");
    }
    
    private void MenuUpStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("MenuUpStarted");
    }
    private void MenuUpPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("MenuUpPerformed");
    }
    private void MenuUpCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("MenuUpCanceled");
    }
    
    private void MenuInteractStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("MenuDownStarted");
    }
    private void MenuInteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("MenuDownPerformed");
    }
    private void MenuInteractCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("MenuDownCanceled");
    }
    
}