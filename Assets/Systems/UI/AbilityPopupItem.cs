using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPopupItem : MonoBehaviour
{
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private Image fillImage;

    public void SetFill(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
