using Cinemachine;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine.InputSystem;

public class Player : Character
{
    private IPaddle _paddle;
    private IMovement _movement;
    private IAbilityPrimary _abilityPrimary;
    private IAbilitySecondary _abilitySecondary;
    private IAbilitySpecial _abilitySpecial;

    private PlayerControls _playerControls;
    public PlayerSignal PlayerSignal { get; } = new();
    private bool _passive = false;

    protected override void Awake()
    {
        base.Awake();
        _movement = GetComponent<IMovement>();
        _paddle = GetComponentInChildren<IPaddle>();
        _abilityPrimary = GetComponentInChildren<IAbilityPrimary>();
        _abilitySecondary = GetComponentInChildren<IAbilitySecondary>();
        _abilitySpecial = GetComponentInChildren<IAbilitySpecial>();
    }
    
    public override void Init(PlayerInfo info)
    {
        base.Init(info);
        InitControls();
        _paddle.Init(PlayerInfo.teamSide);
        _abilityPrimary?.Init(PlayerInfo);
        _abilitySecondary?.Init(PlayerInfo);
        _abilitySpecial?.Init(PlayerInfo);
        Payload();
    }

    private void Update()
    {
        PlayerSignal.primarySignal.Update(_abilityPrimary);
        PlayerSignal.secondarySignal.Update(_abilitySecondary);
        PlayerSignal.specialSignal.Update(_abilitySpecial);
    }

    private void Payload()
    {
        PlayerPayload payload = new PlayerPayload
        {
            PlayerInfo = PlayerInfo
        };
        Messenger.Default.Publish(payload);
    }

    private void InitControls()
    {
        _playerControls = new PlayerControls();
        _playerControls.bindingMask = ChaosPongHelper.GetBindingMask(PlayerInfo.controlScheme);
        if (PlayerInfo.controlScheme == ControlScheme.KeyboardSpecial)
        {
            if(_movement != null)
                _playerControls.Player.MoveSpecial.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.HitSpecial.performed += Hit;
        }
        else
        {
            _playerControls.Player.Passive.performed += PassivePerformed;
            _playerControls.Player.Passive.canceled += PassiveCancelled;
            if(_movement != null)
                _playerControls.Player.Move.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.Hit.performed += Hit;
            _playerControls.Player.AbilityPrimary.performed += PrimaryAbility;
            _playerControls.Player.AbilitySecondary.performed += SecondaryAbility;
            _playerControls.Player.AbilitySpecial.performed += SpecialAbility;
        }
        _playerControls.Enable();
    }

    private void PassivePerformed(InputAction.CallbackContext callbackContext)
    {
        _passive = true;
    }

    private void PassiveCancelled(InputAction.CallbackContext callbackContext)
    {
        _passive = false;
    }

    private void PrimaryAbility(InputAction.CallbackContext callbackContext)
    {
        if(!_passive)
            _abilityPrimary?.Activate(callbackContext);
    }

    private void SecondaryAbility(InputAction.CallbackContext callbackContext)
    {
        if(!_passive)
            _abilitySecondary?.Activate(callbackContext);
    }

    private void SpecialAbility(InputAction.CallbackContext callbackContext)
    {
        if(!_passive)
            _abilitySpecial?.Activate(callbackContext);
    }

    private void Hit(InputAction.CallbackContext callbackContext)
     {
        switch (State)
        {
            //Do nothing
            case CharacterState.Idle:
                break;
            //Throw the pong into the air and wait for next input
            case CharacterState.Starting:
                //5th March - Put Serving Logic here until toss -> serve logic is finalised
                _movement.SetActive(true);
                _paddle.Serve();
                SetState(CharacterState.Returning);
                
                // _paddle.Toss();
                // SetState(CharacterState.Serving);
                break;
            //Serve the pong
            case CharacterState.Serving:
                _movement.SetActive(true);
                _paddle.Serve();
                SetState(CharacterState.Returning);
                break;
            //Return the pong
            case CharacterState.Returning:
                _paddle.Return();
                break;
        }
    }

    public override void SetStart()
    {
        _paddle.SetStart();
        SetState(CharacterState.Starting);
        _movement.SetActive(false);
    }
    
    private void OnDestroy()
    {
        _playerControls.Dispose();
    }
}
