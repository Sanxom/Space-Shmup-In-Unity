using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/*Helper class for input management. Only have exactly one permanently active object
in your scene at any time holding an instance of this - or create a singleton if not possible otherwise*/
public class InputExtensions : MonoBehaviour
{
    // Subscribe to this event
    public static event Action<ControlScheme> OnInputSchemeChanged;

    public static ControlScheme CurrentControlScheme { get; private set; }

    public InputActionAsset inputActions;
    public EventSystem eventSystem;
    public InputUser user;

    private GameObject previousSelectedGO;

    private void Awake()
    {
        previousSelectedGO = eventSystem.currentSelectedGameObject;
    }
    private void Start() => StartAutoControlSchemeSwitching();
    private void OnDestroy() => StopAutoControlSchemeSwitching();

    private void StartAutoControlSchemeSwitching()
    {
        user = InputUser.CreateUserWithoutPairedDevices();
        user.AssociateActionsWithUser(inputActions.actionMaps[0]); // need to be there at least one actionmap defined in InputActionAsset, otherwise rises exception during paring process
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += InputUser_onUnpairedDeviceUsed;
        user.UnpairDevices();
    }

    private void StopAutoControlSchemeSwitching()
    {
        InputUser.onUnpairedDeviceUsed -= InputUser_onUnpairedDeviceUsed;
        if (InputUser.listenForUnpairedDeviceActivity > 0)
            --InputUser.listenForUnpairedDeviceActivity;
        user.UnpairDevicesAndRemoveUser();
    }

    private void InputUser_onUnpairedDeviceUsed(InputControl ctrl, UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr)
    {
        var device = ctrl.device;

        //if ((CurrentControlScheme == ControlScheme.KeyboardMouse) &&
        //     ((device is Pointer) || (device is Keyboard)))
        //{
        //    InputUser.PerformPairingWithDevice(device, user);
        //    previousSelectedGO = eventSystem.currentSelectedGameObject;
        //    OnInputSchemeChanged?.Invoke(ControlScheme.KeyboardMouse);
        //    SetUserControlScheme(ControlScheme.KeyboardMouse);
        //    return;
        //}

        if (device is Gamepad)
        {
            eventSystem.SetSelectedGameObject(previousSelectedGO);
            OnInputSchemeChanged?.Invoke(ControlScheme.Gamepad);
            CurrentControlScheme = ControlScheme.Gamepad;
            SetUserControlScheme(ControlScheme.Gamepad);
        }
        else if (device is Keyboard)
        {
            eventSystem.SetSelectedGameObject(previousSelectedGO);
            OnInputSchemeChanged?.Invoke(ControlScheme.KeyboardMouse);
            CurrentControlScheme = ControlScheme.KeyboardMouse;
            SetUserControlScheme(ControlScheme.KeyboardMouse);
        }
        else if (device is Pointer)
        {
            previousSelectedGO = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(null);
            OnInputSchemeChanged?.Invoke(ControlScheme.KeyboardMouse);
            CurrentControlScheme = ControlScheme.KeyboardMouse;
            SetUserControlScheme(ControlScheme.KeyboardMouse);
        }
        else return;

        user.UnpairDevices();
        InputUser.PerformPairingWithDevice(device, user);
    }

    public void SetUserControlScheme(ControlScheme scheme)
    {
        //user.ActivateControlScheme(scheme.ToString());
        user.ActivateControlScheme(inputActions.controlSchemes[(int)scheme]); // this should be faster and not vulnerable to scheme string names
    }

    public enum ControlScheme
    {
        KeyboardMouse = 0, Gamepad = 1 // just need to be same indexes as defined in inputActionAsset
    }
}