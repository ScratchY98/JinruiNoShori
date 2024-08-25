using UnityEngine;
using System;
using UnityEngine.UI;

public class KeyBind : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Text leftButtonLbl;
    [SerializeField] private Text rightButtonLbl;
    [SerializeField] private Text upButtonLbl;
    [SerializeField] private Text downButtonLbl;
    [SerializeField] private Text odmGearButtonLbl;
    [SerializeField] private Text useGasButtonLbl;
    [SerializeField] private Text sprintButtonLbl;
    [SerializeField] private Text attackButtonLbl;

    public enum KeyBinding
    {
        None,
        Left,
        Right,
        Up,
        Down,
        ODMGear,
        UseGas,
        Sprint,
        Attack
    }

    private KeyBinding currentBinding = KeyBinding.None;

    private void Start()
    {
        leftButtonLbl.text = PlayerPrefs.GetString("HorizontalLeftKey", "Q");
        rightButtonLbl.text = PlayerPrefs.GetString("HorizontalRightKey", "D");
        upButtonLbl.text = PlayerPrefs.GetString("VerticalUpKey", "Z");
        downButtonLbl.text = PlayerPrefs.GetString("VerticalDownKey", "S");
        odmGearButtonLbl.text = PlayerPrefs.GetString("ODMGearKey", "Mouse0");
        useGasButtonLbl.text = PlayerPrefs.GetString("UseGasKey", "Mouse1");
        sprintButtonLbl.text = PlayerPrefs.GetString("SprintKey", "LeftShift");
        attackButtonLbl.text = PlayerPrefs.GetString("AttackKey", "Mouse2");
    }

    private void Update()
    {
        if (currentBinding != KeyBinding.None)
        {
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keycode))
                {
                    string keyString = keycode.ToString();
                    switch (currentBinding)
                    {
                        case KeyBinding.Left:
                            leftButtonLbl.text = keyString;
                            PlayerPrefs.SetString("HorizontalLeftKey", keyString);
                            break;
                        case KeyBinding.Right:
                            rightButtonLbl.text = keyString;
                            PlayerPrefs.SetString("HorizontalRightKey", keyString);
                            break;
                        case KeyBinding.Up:
                            upButtonLbl.text = keyString;
                            PlayerPrefs.SetString("VerticalUpKey", keyString);
                            break;
                        case KeyBinding.Down:
                            downButtonLbl.text = keyString;
                            PlayerPrefs.SetString("VerticalDownKey", keyString);
                            break;
                        case KeyBinding.ODMGear:
                            odmGearButtonLbl.text = keyString;
                            PlayerPrefs.SetString("ODMGearKey", keyString);
                            break;
                        case KeyBinding.UseGas:
                            useGasButtonLbl.text = keyString;
                            PlayerPrefs.SetString("UseGasKey", keyString);
                            break;
                        case KeyBinding.Sprint:
                            sprintButtonLbl.text = keyString;
                            PlayerPrefs.SetString("SprintKey", keyString);
                            break;
                        case KeyBinding.Attack:
                            attackButtonLbl.text = keyString;
                            PlayerPrefs.SetString("AttackKey", keyString);
                            break;
                    }

                    PlayerPrefs.Save();
                    currentBinding = KeyBinding.None;
                    break;
                }
            }
        }
    }

    public void ChangeKeyButton(string key)
    {
        KeyBinding binding;
        if (Enum.TryParse(key, out binding))
        {
            ChangeKey(binding);
        }
        else
        {
            Debug.LogWarning("The key binding string is not valid.");
        }
    }

    public void ChangeKey(KeyBinding key)
    {
        switch (key)
        {
            case KeyBinding.Left:
                SetBindingAndLabel(leftButtonLbl, KeyBinding.Left);
                break;
            case KeyBinding.Right:
                SetBindingAndLabel(rightButtonLbl, KeyBinding.Right);
                break;
            case KeyBinding.Up:
                SetBindingAndLabel(upButtonLbl, KeyBinding.Up);
                break;
            case KeyBinding.Down:
                SetBindingAndLabel(downButtonLbl, KeyBinding.Down);
                break;
            case KeyBinding.ODMGear:
                SetBindingAndLabel(odmGearButtonLbl, KeyBinding.ODMGear);
                break;
            case KeyBinding.UseGas:
                SetBindingAndLabel(useGasButtonLbl, KeyBinding.UseGas);
                break;
            case KeyBinding.Sprint:
                SetBindingAndLabel(sprintButtonLbl, KeyBinding.Sprint);
                break;
            case KeyBinding.Attack:
                SetBindingAndLabel(attackButtonLbl, KeyBinding.Attack);
                break;
            default:
                Debug.Log("The key binding is not correct.");
                break;
        }
    }

    private void SetBindingAndLabel(Text label, KeyBinding binding)
    {
        label.text = "Awaiting Input";
        currentBinding = binding;
    }
}
