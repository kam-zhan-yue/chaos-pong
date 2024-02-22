//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Systems/Common/Input Actions/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""5bcb5400-5b7f-4e3c-893c-a3730118feff"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c0f89974-ec32-47c9-9f0d-c0f2101a9ba2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveSpecial"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7a8c53b1-ab3d-42a1-895a-65f01eebddb2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hit"",
                    ""type"": ""Button"",
                    ""id"": ""6c3dd1b1-2906-4c2f-8968-0f573ba79e82"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HitSpecial"",
                    ""type"": ""Button"",
                    ""id"": ""40325816-7d50-40a7-a784-b7c514757267"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AbilityPrimary"",
                    ""type"": ""Button"",
                    ""id"": ""0f5502b2-2698-49d2-8478-35794ba3c547"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AbilitySecondary"",
                    ""type"": ""Button"",
                    ""id"": ""5c64fed2-5ec4-4223-af31-160d4b6f03b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AbilitySpecial"",
                    ""type"": ""Button"",
                    ""id"": ""2ac233fb-e446-4361-80d0-a88118f47d4c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""2ba42b87-4698-40be-9260-9d49659fbae2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""025e026a-98ed-4f8e-bfb5-0752e40b5a20"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""21a2dd5d-2b76-4bff-8f52-351e8878704c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7992773c-29fe-473c-91e2-51da858237e0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""87b02861-cbbf-472f-845c-132b801b34de"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e6a7acdb-6713-4a7f-a4d8-1493fe360614"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""15d0d33b-9a92-4a5b-bcee-39708176f182"",
                    ""path"": ""<SwitchProControllerHID>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Switch"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""c5b21b73-bf5c-4f10-8d0b-c3b004e646ee"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSpecial"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""537e1d48-6bbc-48d5-b4c5-f574ea06a0c5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSpecial"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f50be9f1-f1b5-4f22-a54f-fa7cedbec052"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""71452ace-51c9-41f7-b85b-bfc51049efc9"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""754f1d58-61e9-4246-8420-a90008e54091"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0ee219f9-c317-4c06-8a95-b4b5a74f382d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bd64f10f-ceea-4475-bbad-573378e37b81"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilityPrimary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d00bbc52-f898-47da-aeea-6947072b7fa5"",
                    ""path"": ""<SwitchProControllerHID>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Switch"",
                    ""action"": ""AbilityPrimary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e69ee26-67fd-49ce-8146-a04919358521"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilitySecondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e52e077-10d1-4322-9033-ece34cd50bc8"",
                    ""path"": ""<SwitchProControllerHID>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Switch"",
                    ""action"": ""AbilitySecondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d245c24-224f-44ef-bcb4-49755c8b061e"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""AbilitySpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d662e20-ccad-4e01-8b17-1f9866de2fb3"",
                    ""path"": ""<SwitchProControllerHID>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Switch"",
                    ""action"": ""AbilitySpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""264f20e7-ef25-41b4-bb26-a9672c73462c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Hit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96ecace3-0378-4007-bfdf-120b86ebaef1"",
                    ""path"": ""<SwitchProControllerHID>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Switch"",
                    ""action"": ""Hit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de34d21f-e7ee-4d50-a9f5-cd656dd58f17"",
                    ""path"": ""<Keyboard>/rightAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""HitSpecial"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Switch"",
            ""bindingGroup"": ""Switch"",
            ""devices"": [
                {
                    ""devicePath"": ""<SwitchProControllerHID>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_MoveSpecial = m_Player.FindAction("MoveSpecial", throwIfNotFound: true);
        m_Player_Hit = m_Player.FindAction("Hit", throwIfNotFound: true);
        m_Player_HitSpecial = m_Player.FindAction("HitSpecial", throwIfNotFound: true);
        m_Player_AbilityPrimary = m_Player.FindAction("AbilityPrimary", throwIfNotFound: true);
        m_Player_AbilitySecondary = m_Player.FindAction("AbilitySecondary", throwIfNotFound: true);
        m_Player_AbilitySpecial = m_Player.FindAction("AbilitySpecial", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_MoveSpecial;
    private readonly InputAction m_Player_Hit;
    private readonly InputAction m_Player_HitSpecial;
    private readonly InputAction m_Player_AbilityPrimary;
    private readonly InputAction m_Player_AbilitySecondary;
    private readonly InputAction m_Player_AbilitySpecial;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @MoveSpecial => m_Wrapper.m_Player_MoveSpecial;
        public InputAction @Hit => m_Wrapper.m_Player_Hit;
        public InputAction @HitSpecial => m_Wrapper.m_Player_HitSpecial;
        public InputAction @AbilityPrimary => m_Wrapper.m_Player_AbilityPrimary;
        public InputAction @AbilitySecondary => m_Wrapper.m_Player_AbilitySecondary;
        public InputAction @AbilitySpecial => m_Wrapper.m_Player_AbilitySpecial;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @MoveSpecial.started += instance.OnMoveSpecial;
            @MoveSpecial.performed += instance.OnMoveSpecial;
            @MoveSpecial.canceled += instance.OnMoveSpecial;
            @Hit.started += instance.OnHit;
            @Hit.performed += instance.OnHit;
            @Hit.canceled += instance.OnHit;
            @HitSpecial.started += instance.OnHitSpecial;
            @HitSpecial.performed += instance.OnHitSpecial;
            @HitSpecial.canceled += instance.OnHitSpecial;
            @AbilityPrimary.started += instance.OnAbilityPrimary;
            @AbilityPrimary.performed += instance.OnAbilityPrimary;
            @AbilityPrimary.canceled += instance.OnAbilityPrimary;
            @AbilitySecondary.started += instance.OnAbilitySecondary;
            @AbilitySecondary.performed += instance.OnAbilitySecondary;
            @AbilitySecondary.canceled += instance.OnAbilitySecondary;
            @AbilitySpecial.started += instance.OnAbilitySpecial;
            @AbilitySpecial.performed += instance.OnAbilitySpecial;
            @AbilitySpecial.canceled += instance.OnAbilitySpecial;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @MoveSpecial.started -= instance.OnMoveSpecial;
            @MoveSpecial.performed -= instance.OnMoveSpecial;
            @MoveSpecial.canceled -= instance.OnMoveSpecial;
            @Hit.started -= instance.OnHit;
            @Hit.performed -= instance.OnHit;
            @Hit.canceled -= instance.OnHit;
            @HitSpecial.started -= instance.OnHitSpecial;
            @HitSpecial.performed -= instance.OnHitSpecial;
            @HitSpecial.canceled -= instance.OnHitSpecial;
            @AbilityPrimary.started -= instance.OnAbilityPrimary;
            @AbilityPrimary.performed -= instance.OnAbilityPrimary;
            @AbilityPrimary.canceled -= instance.OnAbilityPrimary;
            @AbilitySecondary.started -= instance.OnAbilitySecondary;
            @AbilitySecondary.performed -= instance.OnAbilitySecondary;
            @AbilitySecondary.canceled -= instance.OnAbilitySecondary;
            @AbilitySpecial.started -= instance.OnAbilitySpecial;
            @AbilitySpecial.performed -= instance.OnAbilitySpecial;
            @AbilitySpecial.canceled -= instance.OnAbilitySpecial;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_SwitchSchemeIndex = -1;
    public InputControlScheme SwitchScheme
    {
        get
        {
            if (m_SwitchSchemeIndex == -1) m_SwitchSchemeIndex = asset.FindControlSchemeIndex("Switch");
            return asset.controlSchemes[m_SwitchSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMoveSpecial(InputAction.CallbackContext context);
        void OnHit(InputAction.CallbackContext context);
        void OnHitSpecial(InputAction.CallbackContext context);
        void OnAbilityPrimary(InputAction.CallbackContext context);
        void OnAbilitySecondary(InputAction.CallbackContext context);
        void OnAbilitySpecial(InputAction.CallbackContext context);
    }
}
