using Cysharp.Threading.Tasks;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Collider[] _colliders;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        ActivateColliders(false);
    }

    private void Hit()
    {
        ActivateAsync().Forget();
    }

    private async UniTaskVoid ActivateAsync()
    {
        ActivateColliders(true);
        
        await UniTask.WaitForEndOfFrame(this);
        
        ActivateColliders(false);
    }

    private void ActivateColliders(bool active)
    {
        for (int i = 0; i < _colliders.Length; ++i)
        {
            _colliders[i].enabled = active;
        }
    }
}
