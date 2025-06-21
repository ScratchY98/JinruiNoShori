using UnityEngine;
using System.IO;

public class GameTimeTracker : MonoBehaviour
{
    [SerializeField] private int trackerCooldown = 1;
    private int totalTimePassed;

    void Start()
    {
        string pathToJSON = Application.persistentDataPath + "/success.json";

        if (File.Exists(pathToJSON))
        {
            string json = File.ReadAllText(pathToJSON);
            JSONSuccess data = JsonUtility.FromJson<JSONSuccess>(json);
            totalTimePassed = data.timePassed;
        }
        else
            totalTimePassed = 0;

        InvokeRepeating("SaveSessionTime", trackerCooldown, trackerCooldown);
    }

    private void SaveSessionTime()
    {
        totalTimePassed += trackerCooldown;
        ActualiseJSONSuccess.instance.SaveSuccess(totalTimePassed, "timePassed");
    }
}

