using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapComplete : MonoBehaviour
{
    [SerializeField] GameObject lapCompleteTrigger;
    [SerializeField] LapCounter lapCounter;

    public GameObject minText;
    public GameObject secText;
    public GameObject milliText;

    public int bestMinCount;
    public int bestSecCount;
    public float bestMiliCount;

    public GameObject bestMinText;
    public GameObject bestSecText;
    public GameObject bestMiliText;

    public float rawTime;

    //[SerializeField] GameObject lapTimeText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lapCounter.UpdateLapCounter();
            rawTime = PlayerPrefs.GetFloat("RawTime");

            if (LapTImeManager.minCount > 9)
            {
                minText.GetComponent<Text>().text = "0" + LapTImeManager.minCount + ".";
            }
            else
            {
                minText.GetComponent<Text>().text = LapTImeManager.minCount + ".";
            }

            if (LapTImeManager.secCount <= 9)
            {
                secText.GetComponent<Text>().text = "0" + LapTImeManager.secCount + ".";
            }
            else
            {
                secText.GetComponent<Text>().text = LapTImeManager.secCount + ".";
            }

            milliText.GetComponent<Text>().text = LapTImeManager.milliDisplay;

            PlayerPrefs.SetInt("MinSave", LapTImeManager.minCount);
            PlayerPrefs.SetInt("SecSave", LapTImeManager.secCount);
            PlayerPrefs.SetFloat("MiliSave", LapTImeManager.milliCount);
            PlayerPrefs.SetFloat("RawTime", LapTImeManager.rawTime);



            LapTImeManager.minCount = 0;
            LapTImeManager.secCount = 0;
            LapTImeManager.milliCount = 0;
            LapTImeManager.rawTime = 0;

            lapCompleteTrigger.SetActive(false);
            UpdateBestText();
        }
    }

    void UpdateBestText()
    {
        bestMinCount = PlayerPrefs.GetInt("MinSave");
        bestSecCount = PlayerPrefs.GetInt("SecSave");
        bestMiliCount = PlayerPrefs.GetFloat("MiliSave");

        bestMinText.GetComponent<Text>().text = "" + bestMinCount + ":";
        bestSecText.GetComponent<Text>().text = "" + bestSecCount + ".";
        bestMiliText.GetComponent<Text>().text = "" + bestMiliCount.ToString("F0");
    }
}
