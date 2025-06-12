using System.Reflection;
using UnityEngine;

public class ActualiseJSONSuccess : MonoBehaviour
{
    public static ActualiseJSONSuccess instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of ActualiseJSONSuccess in the scene.");
            return;
        }

        instance = this;
    }

    public void SaveSuccess(float newValue, string fieldName)
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

        float currentValue = (float)fieldInfo.GetValue(data);

        if (newValue <= currentValue) return;

        fieldInfo.SetValue(data, newValue);

        string newJson = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, newJson);

        Debug.Log($"[SaveSuccess] Champ '{fieldName}' mis à jour : {currentValue} → {newValue}");
    }

}
