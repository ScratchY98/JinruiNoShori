using UnityEngine;
using System.IO;
using System;

public class GameTimeTracker : MonoBehaviour
{
    private float sessionStartTime;
    private string path;
    private JSONSuccess data;

    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "success.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<JSONSuccess>(json);
        }
        else
        {
            data = new JSONSuccess { timePassed = 0 };
        }

        sessionStartTime = Time.realtimeSinceStartup;
    }

    void OnApplicationQuit()
    {
        float sessionDuration = Time.realtimeSinceStartup - sessionStartTime;
        data.timePassed += Mathf.FloorToInt(sessionDuration);

        if (ActualiseJSONSuccess.instance != null)
        {
            ActualiseJSONSuccess.instance.SaveSuccess(data.timePassed, "timePassed");
        }
        else
        {
            string newJson = JsonUtility.ToJson(data);
            File.WriteAllText(path, newJson);
        }
    }
}
