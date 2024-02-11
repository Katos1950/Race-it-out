using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    public int totalLapsCount = 3;
    int currentLapCount = 1;
    [SerializeField] GameObject currentLap;
    [SerializeField] GameObject totalLaps;

    private void Start()
    {
        currentLap.GetComponent<Text>().text = currentLapCount.ToString();
        totalLaps.GetComponent<Text>().text = totalLapsCount.ToString();
    }
    public void UpdateLapCounter()
    {
        currentLapCount++;
        if(currentLapCount > totalLapsCount)
        {
            currentLapCount = totalLapsCount;
        }
        currentLap.GetComponent<Text>().text = currentLapCount.ToString();

    }
}
