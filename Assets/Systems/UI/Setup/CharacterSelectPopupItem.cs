using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterSelectPopupItem : MonoBehaviour
{
    [SerializeField] private PresetController presetController;
    [SerializeField] private Image thumbnail;
    [SerializeField] private Image outline;
    [SerializeField] private Image playerHolder;
    [SerializeField] private TMP_Text playerText;
    public bool Selected { get; private set; } = false;
    public bool Ready { get; private set; } = false;
    public CharacterConfig Config { get; private set; }
    public PlayerUI PlayerUI { get; private set; } = new PlayerUI(0, ControlScheme.Keyboard, CharacterType.Player);

    public void Init(CharacterConfig config)
    {
        Config = config;
        thumbnail.sprite = Config.thumbnail;
        outline.color = Config.outline;
        playerHolder.color = Config.outline;
        presetController.SetPresetById("reset");
        thumbnail.color = Color.white;
        Selected = false;
        Ready = false;
    }

    public void Select(PlayerUI playerUI)
    {
        PlayerUI = playerUI;
        Selected = true;
        Ready = false;
        presetController.SetPresetById("select");
        playerText.SetText($"P{playerUI.id}");
        thumbnail.color = Color.white;
    }

    public void Deselect()
    {
        PlayerUI = new PlayerUI(0, ControlScheme.Keyboard, CharacterType.Player);
        Selected = false;
        Ready = false;
        presetController.SetPresetById("deselect");
    }

    public void ReadyUp()
    {
        Ready = true;
        presetController.SetPresetById("ready");
    }
}
