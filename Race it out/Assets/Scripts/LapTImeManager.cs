using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTImeManager : MonoBehaviour
{
    public static int minCount;
    public static int secCount;
    public static float milliCount;
    public static string milliDisplay;

    public GameObject minText;
    public GameObject secText;
    public GameObject milliText;

    public static float rawTime;

    void Update()
    {
        milliCount += Time.deltaTime * 10;
        rawTime += Time.deltaTime;
        milliDisplay = milliCount.ToString("F0");
        milliText.GetComponent<Text>().text = milliDisplay;

        if(milliCount >= 10)
        {
            milliCount = 0;
            secCount++;
        }

        if(secCount <= 9)
        {
            secText.GetComponent<Text>().text = "0" + secCount + ".";
        }
        else
        {
            secText.GetComponent<Text>().text = secCount + ".";
        }

        if(secCount >=60)
        {
            secCount = 0;
            minCount++;
        }

        if(minCount > 9)
        {
            minText.GetComponent<Text>().text = "0" + minCount + ".";
        }
        else
        {
            minText.GetComponent<Text>().text = minCount + ".";
        }
    }
}
