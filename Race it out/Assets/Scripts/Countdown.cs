using System.Collections;
using TMPro;
using UnityEngine;
public class Countdown : MonoBehaviour
{
    [SerializeField]  TextMeshProUGUI countdown;
    //[SerializeField] GameObject lapTimer;
    public GameObject carControls;
    public GameObject aiCarControls;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ActivateControls");
        StartCoroutine("CountStart");
    }

   private IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        countdown.text = "3";
        yield return new WaitForSeconds(1f);
        
        countdown.text = "2";
        countdown.gameObject.GetComponent<Animator>().Play(0);
        yield return new WaitForSeconds(1f);
        
        countdown.text = "1";
        countdown.gameObject.GetComponent<Animator>().Play(0);
        yield return new WaitForSeconds(1f);
        countdown.gameObject.SetActive(false);

        //lapTimer.GetComponent<LapTImeManager>().enabled = true;
        carControls.GetComponent<CarController>().enabled = true;
        aiCarControls.GetComponent<CarAiEngine>().enabled = true;
    }

    private IEnumerator ActivateControls()
    {
        yield return new WaitForSeconds(0.1f);
        carControls = GameObject.FindGameObjectWithTag("Player");
    }
}
