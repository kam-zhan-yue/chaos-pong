using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Kuroneko.UtilityDelivery;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPopupItem : MonoBehaviour
{
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private Image fillImage;

    private AbilitySignal _signal = new();

    private void Awake()
    {
        cooldownText.gameObject.SetActiveFast(false);
    }

    public void Init(AbilitySignal abilitySignal)
    {
        _signal = abilitySignal;
        _signal.interactive.Subscribe(OnInteractiveChanged);
        _signal.duration.Subscribe(OnDurationChanged);
        _signal.cooldown.Subscribe(OnCooldownChanged);
    }

    private void OnDurationChanged(float prev, float curr)
    {
        if (_signal.durationTime > 0f)
        {
            float value = curr / _signal.durationTime;
            SetFill(value);
        }
    }

    private void OnCooldownChanged(float prev, float curr)
    {
        bool hasCooldown = curr > 0f;
        cooldownText.gameObject.SetActiveFast(hasCooldown);
        cooldownText.SetText(curr.ToString("F2"));
        if(hasCooldown)
            SetFill(0f);
    }

    private void OnInteractiveChanged(bool prev, bool curr)
    {
        //If interactive, set fill to 1 and hide cooldown
        if (curr)
        {
            SetFill(1f);
            cooldownText.gameObject.SetActiveFast(false);
        }
        else if(_signal.duration.Value <= 0f)
        {
            SetFill(0f);
        }
    }

    private void SetFill(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
