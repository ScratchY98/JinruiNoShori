using HSVPicker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAvatar : MonoBehaviour
{
    [SerializeField] private Material[] mats;
    [SerializeField] private Color[] normalMatsColor;
    [SerializeField] private ColorPicker picker;
    [SerializeField] private Dropdown bodyPartsDropdown;
    private int lastDropdownIndex;

    void Start()
    {
        List<string> options = new List<string>();
        foreach (Material mat in mats) { options.Add(mat.name); }

        bodyPartsDropdown.ClearOptions();
        bodyPartsDropdown.AddOptions(options);
        bodyPartsDropdown.value = 0;
        bodyPartsDropdown.RefreshShownValue();

        picker.AssignColor(mats[0].color);
        picker.onValueChanged.AddListener(color =>
        {
            mats[0].color = color;
        });
        mats[0].color = picker.CurrentColor;

        RefreshPicker();
    }

    void ChangeMat(int dropdownIndex)
    {
        lastDropdownIndex = dropdownIndex;
        SaveAllColor();

    }

    void RefreshPicker()
    {
        picker.onValueChanged.RemoveAllListeners();
        picker.AssignColor(mats[lastDropdownIndex].color);
        picker.onValueChanged.AddListener(color =>
        {
            mats[lastDropdownIndex].color = color;
        });
        mats[lastDropdownIndex].color = picker.CurrentColor;
    }

    public void ResetColor()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            PlayerPrefs.SetFloat(mats[i].name + "R", normalMatsColor[i].r);
            PlayerPrefs.SetFloat(mats[i].name + "G", normalMatsColor[i].g);
            PlayerPrefs.SetFloat(mats[i].name + "B", normalMatsColor[i].b);

            mats[i].color = normalMatsColor[i];
            RefreshPicker();
        }
    }


    public void SaveAllColor()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            PlayerPrefs.SetFloat(mats[i].name + "R", mats[i].color.r);
            PlayerPrefs.SetFloat(mats[i].name + "G", mats[i].color.g);
            PlayerPrefs.SetFloat(mats[i].name + "B", mats[i].color.b);
        }
    }


    public void LoadAllColor()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            float R = PlayerPrefs.GetFloat(mats[i].name + "R", normalMatsColor[i].r);
            float G = PlayerPrefs.GetFloat(mats[i].name + "G", normalMatsColor[i].g);
            float B = PlayerPrefs.GetFloat(mats[i].name + "B", normalMatsColor[i].b);
            mats[i].color = new Color(R, G, B);
        }
    }
}
