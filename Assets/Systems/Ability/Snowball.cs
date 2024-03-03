using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snowball : Ability, IAbilityPrimary
{
    [SerializeField] private HitModifier hitModifier = new();
    private const float HEIGHT_OFFSET = 0.5f;
    [SerializeField] private SnowballProjectile projectilePrefab;
    [SerializeField] private Transform projectileSpawn;

    private Player _player;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }
    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }
    protected override bool Interactive()
    {
        return base.CanActivate();
    }

    protected override void Activate()
    {
        SnowballProjectile snowball = Instantiate(projectilePrefab, projectileSpawn.transform.position, Quaternion.identity);
        
        //Get the player's team side
        TeamSide playerSide = _player.TeamSide;
        //Get the opposing team
        TeamSide opposingSide = ChaosPongHelper.GetOppositeSide(playerSide);
        //Find a random player from the opposing team
        //Fire the pong at them at an extremely high speed with very little gravity
        IGameManager gameManager = ServiceLocator.Instance.Get<IGameManager>();
        
        switch (opposingSide)
        {
            case TeamSide.Blue:
                FireAtTeam(gameManager.GetBlueTeam(), snowball);
                break;
            case TeamSide.Red:
                FireAtTeam(gameManager.GetRedTeam(), snowball);
                break;
        }
    }

    private void FireAtTeam(Team team, SnowballProjectile snowball)
    {
        if (team.Characters.Count == 0)
            return;
        Character random = team.Characters.GetRandomElement();

        float height = projectileSpawn.transform.position.y + HEIGHT_OFFSET;
        snowball.Launch(random.transform.position, height, _player.TeamSide, hitModifier);
    }
    

    protected override void Deactivate()
    {
        
    }

}
