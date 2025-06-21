using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class ActualiseJSONSuccess : MonoBehaviour
{
    public static ActualiseJSONSuccess instance;
    [SerializeField] private Text actualiseText;
    [SerializeField] private float textDuringTime;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of ActualiseJSONSuccess in the scene.");
            return;
        }

        instance = this;
    }

    public void SaveSuccess(int newValue, string fieldName)
    {
        string path = Application.persistentDataPath + "/success.json";
        JSONSuccess data;

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            data = JsonUtility.FromJson<JSONSuccess>(json);
        }
        else data = new JSONSuccess();

        FieldInfo fieldInfo = typeof(JSONSuccess).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

        int currentValue = (int)fieldInfo.GetValue(data);

        if (newValue <= currentValue) return;

        fieldInfo.SetValue(data, newValue);

        string newJson = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, newJson);

        bool wantViewSuccesses = PlayerPrefs.GetInt("ViewSuccesses", 0) == 1 ? true : false;

        if (fieldName == "timePassed" || actualiseText.gameObject.activeInHierarchy || !wantViewSuccesses) return;

        string text = "<color=green>[SaveSuccess]</color>Best value of '<b>" + fieldName + "</b>' update : <color=yellow>" + currentValue + "</color> → <color=lime>" + newValue + "</color>";
        StartCoroutine(ActualiseText(text));
    }

    IEnumerator ActualiseText(string text)
    {
        actualiseText.text = text;

        actualiseText.gameObject.SetActive(true);

        yield return new WaitForSeconds(textDuringTime);

        actualiseText.gameObject.SetActive(false);
    }

}
