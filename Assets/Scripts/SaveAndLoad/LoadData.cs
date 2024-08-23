using UnityEngine;

public class LoadData : MonoBehaviour
{
    [Header ("Input")]
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
    public KeyCode ODMGear;
    public KeyCode useGas;
    public KeyCode sprint;
    public KeyCode attack;

    [Header("Other")]
    public bool isTitanSmoke;

    public static LoadData instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Get Input from PlayerPrefs.
        string leftStr = PlayerPrefs.GetString("HorizontalLeftKey", "Q");
        string rightStr = PlayerPrefs.GetString("HorizontalRightKey", "D");
        string upStr = PlayerPrefs.GetString("VerticalUpKey", "Z");
        string downStr = PlayerPrefs.GetString("VerticalDownKey", "S");
        string ODMGearStr = PlayerPrefs.GetString("ODMGearKey", "Mouse0");
        string useGasStr = PlayerPrefs.GetString("UseGasKey", "Mouse1");
        string sprintStr = PlayerPrefs.GetString("SprintKey", "LeftShift");
        string attackStr = PlayerPrefs.GetString("AttackKey", "Mouse2");

        // Set Inputs's variables.
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), leftStr);
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), rightStr);
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), upStr);
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), downStr);
        ODMGear = (KeyCode)System.Enum.Parse(typeof(KeyCode), ODMGearStr);
        useGas = (KeyCode)System.Enum.Parse(typeof(KeyCode), useGasStr);
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), sprintStr);
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), attackStr);

        // Get the save of isTitanSmoke. We use a int for save a bool in PlayerPref. If isTitanSmokeInt = 1, we can, else we can't.
        int isTitanSmokeInt = PlayerPrefs.GetInt("isTitanSmoke", 1);

        isTitanSmoke = isTitanSmokeInt == 1 ? true : false;
    }



    public float GetHorizontalInput()
    {
        float input = 0f;

        if (Input.GetKey(left))
            input -= 1f;

        if (Input.GetKey(right))
            input += 1f;

        return input;
    }

    public float GetVerticalInput()
    {
        float input = 0f;

        if (Input.GetKey(up))
            input += 1f;

        if (Input.GetKey(down))
            input -= 1f;

        return input;
    }
}
