using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewFPS : MonoBehaviour
{
    private Text fpsText;

    private Dictionary<int, string> cachedNumberStrings = new();
    private int[] frameRateSamples;
    private int cacheNumbersAmount = 2000;
    private int averageFromAmount = 30;
    private int averageCounter = 0;
    private int currentAveraged;

    private void Start()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("isViewFPS", 1) == 1);
        fpsText = GetComponent<Text>();
    }

    void Awake()
    {
        for (int i = 0; i < cacheNumbersAmount; i++) {
            cachedNumberStrings[i] = i.ToString(); }

        frameRateSamples = new int[averageFromAmount];
    }

    private void Update()
    {
        int currentFrame = (int)Math.Round(1f / Time.smoothDeltaTime);
        frameRateSamples[averageCounter] = currentFrame;

        float sum = 0f;
        foreach (var frameRate in frameRateSamples){
            sum += frameRate;
        }
        currentAveraged = (int)Math.Round(sum / averageFromAmount);
        averageCounter = (averageCounter + 1) % averageFromAmount;

        fpsText.text = "FPS : " + (currentAveraged switch
        {
            var x when x >= 0 && x < cacheNumbersAmount => cachedNumberStrings[x],
            var x when x >= cacheNumbersAmount => $"> {cacheNumbersAmount}",
            var x when x < 0 => "< 0",
            _ => "?"
        });
    }
}
