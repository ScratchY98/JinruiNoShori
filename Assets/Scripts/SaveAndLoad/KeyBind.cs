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

    private enum KeyBinding
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

    public void ChangeLeftKey()
    {
        leftButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.Left;
    }

    public void ChangeRightKey()
    {
        rightButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.Right;
    }

    public void ChangeUpKey()
    {
        upButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.Up;
    }

    public void ChangeDownKey()
    {
        downButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.Down;
    }

    public void ChangeODMGearKey()
    {
        odmGearButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.ODMGear;
    }

    public void ChangeUseGasKey()
    {
        useGasButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.UseGas;
    }

    public void ChangeSprintKey()
    {
        sprintButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.Sprint;
    }

    public void ChangeAttackKey()
    {
        attackButtonLbl.text = "Awaiting Input";
        currentBinding = KeyBinding.Attack;
    }
}
