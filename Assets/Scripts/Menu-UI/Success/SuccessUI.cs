using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Successes
{
    public string successesName;
    public string JSONVariableName;
    public string successesDescription;
    public Color boxColor;

    public int[] allSuccesses;
}

public class SuccessUI : MonoBehaviour
{
    [SerializeField] private Successes[] successes;
    [SerializeField] private GameObject successUIPrefab, lineUIPrefab;
    [SerializeField] private float lineYDiff;
    [SerializeField] private float YDistance;
    [SerializeField] private float xDistance;
    [SerializeField] private RectTransform orinalPos;
    [SerializeField] private RectTransform successesParent;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Color startLineColor;
    [SerializeField] private Color endLineColor;

    private float xPos;
    private float yPos;

    private string pathToJSON;

    private void Start()
    {
        xPos = orinalPos.position.x;
        yPos = orinalPos.position.y;


        pathToJSON = Application.persistentDataPath + "/success.json";

        JSONSuccess data = LoadJSONData();

        foreach (Successes successes in successes)
        {
            RectTransform currentSuccesses = new GameObject().AddComponent<RectTransform>();
            currentSuccesses.name = successes.successesName;
            currentSuccesses.position = orinalPos.position;
            currentSuccesses.SetParent(successesParent);
            bool isFirstSuccess = true;

            int boxNumber = 0;
            Color nextLineColor = startLineColor;

            foreach (int successIndex in successes.allSuccesses)
            {
                int value = LoadVariableFromJSON(successes.JSONVariableName);

                GameObject currentSuccessGO = Instantiate(successUIPrefab, new Vector3(xPos, yPos, orinalPos.position.z), orinalPos.rotation);
                currentSuccessGO.GetComponent<RectTransform>().SetParent(currentSuccesses);
                currentSuccessGO.GetComponent<Image>().color = successes.boxColor;
                SuccessBox successBox = currentSuccessGO.GetComponent<SuccessBox>();
                successBox.successName = successes.successesName;
                successBox.successDescription = successes.successesDescription;
                successBox.valueToHave = successIndex;
                successBox.textToUpdate = descriptionText;
                successBox.UpdateBoxText(boxNumber);
                successBox.currentValue = value;

                yPos += YDistance;

                if (!isFirstSuccess)
                {
                    Vector3 lineSpawnPoint = currentSuccessGO.transform.position;
                    lineSpawnPoint.y -= lineYDiff;
                    GameObject line = Instantiate(lineUIPrefab, lineSpawnPoint, currentSuccessGO.transform.rotation);
                    line.GetComponent<RectTransform>().SetParent(currentSuccesses);
                    line.GetComponent<Image>().color = nextLineColor;
                }

                if (isFirstSuccess) isFirstSuccess = false;

                boxNumber ++;

                nextLineColor = startLineColor;
                nextLineColor = value < successIndex ? startLineColor : endLineColor;
            }

            xPos += xDistance;
            yPos = orinalPos.position.y;

        }
    }

    public JSONSuccess LoadJSONData()
    {
        if (File.Exists(pathToJSON))
        {
            string json = File.ReadAllText(pathToJSON);
            return JsonUtility.FromJson<JSONSuccess>(json);
        }
        else
        {
            return new JSONSuccess();
        }
    }

    private int LoadVariableFromJSON(string variableName)
    {
        JSONSuccess data = LoadJSONData();

        FieldInfo info = typeof(JSONSuccess).GetField(variableName,
            BindingFlags.Public | BindingFlags.Instance);

        if (info != null)
        {
            object value = info.GetValue(data);
            int result = Convert.ToInt32(value);
            return result;
        }
        else
        {
            Debug.Log($"Champ '{variableName}' introuvable.");
            return -1;
        }
    }
}

