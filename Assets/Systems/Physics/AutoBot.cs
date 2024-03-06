using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using MEC;
using UnityEngine;

public class AutoBot : MonoBehaviour, IPaddle
{
    private const float COOLDOWN = 0.1f;
    [SerializeField] private TeamSide teamSide;
    [SerializeField] private float height;
    [SerializeField] private Pong pongPrefab;
    [SerializeField] private Transform serveTransform;

    private bool _cooldown = false;
    private Pong _pong;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_cooldown)
            return;
        if (other.gameObject.TryGetComponent(out Pong pong))
        {
            pong.Return(teamSide, height, HitType.Return);
            // Timing.RunCoroutine(StartCooldown().CancelWith(gameObject));
        }
    }

    private IEnumerator<float> StartCooldown()
    {
        _cooldown = true;
        yield return Timing.WaitForSeconds(COOLDOWN);
        _cooldown = false;
    }
    
    public void Init(TeamSide side)
    {
        teamSide = side;
    }

    public void SetStart()
    {
        _pong = Instantiate(pongPrefab, serveTransform.position, Quaternion.identity);
    }

    public void Toss()
    {
    }

    public void Serve()
    {
        IGameManager gameManager = ServiceLocator.Instance.Get<IGameManager>();
        gameManager?.Serve(teamSide);
        _pong.Serve(teamSide, height);
    }

    public void Return()
    {
    }

    public bool CanHit()
    {
        return true;
    }

    public void SetHitModifier(HitModifier modifier)
    {
        
    }
}
