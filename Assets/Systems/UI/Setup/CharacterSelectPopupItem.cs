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
    public PlayerInfo PlayerInfo { get; private set; } = new();

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

    public void Select(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        Selected = true;
        Ready = false;
        presetController.SetPresetById("select");
        playerText.SetText($"P{playerInfo.id}");
        thumbnail.color = Color.white;
    }

    public void Deselect()
    {
        PlayerInfo = new();
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
