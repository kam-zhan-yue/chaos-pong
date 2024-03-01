using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModeSelectPopup : Popup
{
    [SerializeField] private RectTransform singlePlayerSelection;
    [SerializeField] private RectTransform twoPlayerSelection;
    [SerializeField] private RectTransform selectHolder;

    private PlayerControls _playerControls;
    private bool _singlePlayer = false;
    private UniTaskCompletionSource<bool> _flow = new();

    protected override void InitPopup()
    {
        _playerControls = new PlayerControls();
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        _playerControls.UI.Navigate.performed += Navigate;
        _playerControls.UI.Select.performed += Select;
        _playerControls.Enable();
        SelectSinglePlayer();
    }

    public async UniTask<bool> GetFlow()
    {
        _flow = new UniTaskCompletionSource<bool>();
        return await _flow.Task;
    }

    private void Navigate(InputAction.CallbackContext callbackContext)
    {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        float horizontal = movementInput.x;
        if (horizontal > 0)
        {
            SelectTwoPlayer();
        }
        else if (horizontal < 0)
        {
            SelectSinglePlayer();
        }
    }

    private void SelectSinglePlayer()
    {
        _singlePlayer = true;
        selectHolder.DOMove(singlePlayerSelection.transform.position, 0.15f).SetEase(Ease.InOutExpo);
    }

    private void SelectTwoPlayer()
    {
        _singlePlayer = false;
        selectHolder.DOMove(twoPlayerSelection.transform.position, 0.15f).SetEase(Ease.InOutExpo);
    }

    private void Select(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Select");
        _flow.TrySetResult(_singlePlayer);
    }

    public override void HidePopup()
    {
        base.HidePopup();
        _playerControls.Dispose();
    }
}
