using UnityEngine;
using UnityEngine.UI;

public class SuccessBox : MonoBehaviour
{
    public string successName;
    public string successDescription;
    public int currentValue;
    public int valueToHave;
    public Text textToUpdate;
    public Text boxText;

    public void UpdateBoxText(int boxNumber)
    {
        boxText.text = boxNumber == 0 ? successName : successName + " | " + boxNumber;
    }
    public void UpdateUI()
    {
        textToUpdate.text =

        successName
        + "\r\n\r\n"
        + successDescription
        + "\r\n\r\n"
        + currentValue
        + "/"
        + valueToHave;
    }
}
