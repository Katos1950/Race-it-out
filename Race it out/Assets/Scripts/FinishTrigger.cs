using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] Path path;
    [SerializeField] LapCounter lapCounter;
    private void OnTriggerEnter(Collider other)
    {
        int totalWaypoints = path.transform.childCount * lapCounter.totalLapsCount;
        if(other.tag == "Player" && path.playerWaypointCount >= totalWaypoints)
        {
            StartCoroutine("LoadWinOrLoseScreen", "Win Scene");
        }
        if (other.tag == "Opponent" && path.opponentWaypointCount >= totalWaypoints)
        {
            StartCoroutine("LoadWinOrLoseScreen", "Lose Scene");
        }
    }

    private IEnumerator LoadWinOrLoseScreen(string scene)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(scene);
    }
}
