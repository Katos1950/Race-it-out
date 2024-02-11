using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool canCollideWithPlayer = false;
    public bool canCollideWithOpponent = false;
    [SerializeField] Path path;

    private void OnTriggerEnter(Collider other)
    {
        int nextWaypointIndex = transform.GetSiblingIndex() + 1;
        if (nextWaypointIndex == path.transform.childCount)
        {
            nextWaypointIndex = 0;
        }
        if (other.tag == "Player" && canCollideWithPlayer)
        {
            canCollideWithPlayer = false;
            path.transform.GetChild(nextWaypointIndex).GetComponent<Waypoint>().canCollideWithPlayer = true;
            path.playerWaypointCount++;
            //Debug.Log("player cleared " + gameObject.name);
        }
        if(other.tag == "Opponent" && canCollideWithOpponent)
        {
            canCollideWithOpponent = false;
            path.transform.GetChild(nextWaypointIndex).GetComponent<Waypoint>().canCollideWithOpponent = true;
            path.opponentWaypointCount++;
            //Debug.Log("Opponent cleared " + gameObject.name);
        }
        if (path.playerWaypointCount != 0 && path.playerWaypointCount % path.transform.childCount == 0)
        {
            path.ActivateLapCompleteTrigger();
        }
    }
}
