using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.InputSystem;

public class Permafrost : Ability, IAbilitySecondary
{
    private const float FLOOR_HEIGHT = 0.01f;
    [SerializeField] private IcyFloor icyFloorPrefab;
    private Player _player;
    private IcyFloor _icyFloor;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }
    
    protected override bool Interactive()
    {
        return base.CanActivate() && _player.State == CharacterState.Returning;
    }

    protected override void Activate()
    {
        if (_icyFloor != null && _icyFloor.gameObject)
        {
            Destroy(_icyFloor.gameObject);
        }

        Vector3 position = transform.position;
        Vector3 floorPosition = new(position.x, FLOOR_HEIGHT, position.z);
        _icyFloor = Instantiate(icyFloorPrefab);
        _icyFloor.Init(_player.TeamSide, floorPosition);
    }

    protected override void Deactivate()
    {
        Destroy(_icyFloor.gameObject);
    }

    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }
}