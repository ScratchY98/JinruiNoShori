using UnityEngine;
using UnityEngine.InputSystem;

public class LoadData : MonoBehaviour
{
    [Header ("Input")]
    public PlayerInput playerInput;

    [Header("Other")]
    public bool isTitanSmoke;

    public static LoadData instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
        // Get the save of isTitanSmoke. We use a int for save a bool in PlayerPref. If isTitanSmokeInt = 1, we can, else we can't.
        int isTitanSmokeInt = PlayerPrefs.GetInt("isTitanSmoke", 1);

        isTitanSmoke = isTitanSmokeInt == 1 ? true : false;
    }
}
