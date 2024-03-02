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
    
    public bool TrySelect(PlayerUI playerUI, bool right, out CharacterConfig config)
    {
        int index = GetIndex(playerUI);
        if (index < 0)
        {
            return TrySelectRandom(playerUI, out config);
        }

        for (int i = 0; i < _popupItemList.Count; ++i)
        {
            int next = right ? (i + 1) % _popupItemList.Count : (i - 1 + _popupItemList.Count) % _popupItemList.Count;
            if (!_popupItemList[next].Selected)
            {
                _popupItemList[index].Deselect();
                _popupItemList[next].Select(playerUI);
                config = _popupItemList[next].Config;
                return true;
            }
        }

        config = null;
        return false;
    }

    public bool TrySelectRandom(PlayerUI playerUI, out CharacterConfig config)
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
        randomItem.Select(playerUI);
        config = randomItem.Config;
        return true;
    }

    private int GetIndex(PlayerUI playerUI)
    {
        for (int i = 0; i < _popupItemList.Count; ++i)
        {
            if (_popupItemList[i].PlayerUI.id == playerUI.id)
            {
                return i;
            }
        }

        return -1;
    }

    public void Deselect(PlayerUI playerUI)
    {
        int index = GetIndex(playerUI);
        if (index >= 0)
        {
            _popupItemList[index].Deselect();
        }
    }

    public void Select(PlayerUI playerUI)
    {
        int index = GetIndex(playerUI);
        if (index >= 0)
        {
            _popupItemList[index].Select(playerUI);
        }
    }

    public void ReadyUp(PlayerUI playerUI)
    {
        int index = GetIndex(playerUI);
        if (index >= 0)
        {
            _popupItemList[index].ReadyUp();
        }
    }
}