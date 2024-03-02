using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterSelectPopup : Popup
{
    [BoxGroup("Scriptable Objects"), SerializeField]
    private CharacterDatabase characterDatabase;

    [SerializeField] private RectTransform popupItemHolder;
    [SerializeField] private CharacterSelectPopupItem samplePopupItem;
    private readonly List<CharacterSelectPopupItem> _popupItemList = new();
    
    protected override void InitPopup()
    {
        
    }

    public override void ShowPopup()
    {
        TryInstantiate();
        PopulateData();
    }

    private void TryInstantiate()
    {
        int numToSpawn = characterDatabase.characters.Length - _popupItemList.Count;
        if (numToSpawn > 0)
        {
            for (int i = 0; i < numToSpawn; ++i)
            {
                samplePopupItem.gameObject.SetActiveFast(true);
                CharacterSelectPopupItem popupItem = Instantiate(samplePopupItem, popupItemHolder);
                _popupItemList.Add(popupItem);
            }
        }
        samplePopupItem.gameObject.SetActiveFast(false);
    }

    private void PopulateData()
    {
        for (int i = 0; i < characterDatabase.characters.Length; ++i)
        {
            if (i < _popupItemList.Count)
            {
                _popupItemList[i].gameObject.SetActiveFast(true);
                _popupItemList[i].Init(characterDatabase.characters[i]);
            }
        }
    }
    
    public bool TrySelect(PlayerInfo playerInfo, bool right, out CharacterConfig config)
    {
        int index = GetIndex(playerInfo);
        if (index < 0)
        {
            return TrySelectRandom(playerInfo, out config);
        }

        for (int i = 0; i < _popupItemList.Count; ++i)
        {
            int next = right ? (i + 1) % _popupItemList.Count : (i - 1 + _popupItemList.Count) % _popupItemList.Count;
            if (!_popupItemList[next].Selected)
            {
                _popupItemList[index].Deselect();
                _popupItemList[next].Select(playerInfo);
                config = _popupItemList[next].Config;
                return true;
            }
        }

        config = null;
        return false;
    }

    public bool TrySelectRandom(PlayerInfo playerInfo, out CharacterConfig config)
    {
        List<CharacterSelectPopupItem> unselectedCharacters = new();
        for (int i = 0; i < _popupItemList.Count; ++i)
        {
            if (!_popupItemList[i].Selected)
                unselectedCharacters.Add(_popupItemList[i]);
        }

        if (unselectedCharacters.Count == 0)
        {
            config = null;
            return false;
        }
        CharacterSelectPopupItem randomItem = unselectedCharacters.GetRandomElement();
        randomItem.Select(playerInfo);
        config = randomItem.Config;
        return true;
    }

    private int GetIndex(PlayerInfo playerInfo)
    {
        for (int i = 0; i < _popupItemList.Count; ++i)
        {
            if (_popupItemList[i].PlayerInfo.identifier == playerInfo.identifier)
            {
                return i;
            }
        }

        return -1;
    }

    public void Deselect(PlayerInfo playerInfo)
    {
        int index = GetIndex(playerInfo);
        if (index >= 0)
        {
            _popupItemList[index].Deselect();
        }
    }

    public void Select(PlayerInfo playerInfo)
    {
        int index = GetIndex(playerInfo);
        if (index >= 0)
        {
            _popupItemList[index].Select(playerInfo);
        }
    }

    public void ReadyUp(PlayerInfo playerInfo)
    {
        int index = GetIndex(playerInfo);
        if (index >= 0)
        {
            _popupItemList[index].ReadyUp();
        }
    }
}