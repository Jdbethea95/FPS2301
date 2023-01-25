using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTrack : MonoBehaviour
{
    [SerializeField] List<WayPoint> wayPoints;

    bool complete = true;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            if (!wayPoints[i].Triggered)
                complete = false;
        }

        if (complete)
        {
            GameManager.instance.onTheClock = false;
            for (int i = 0; i < wayPoints.Count; i++)
            {
                wayPoints[i].ResetTrigger();
            }
        }else
            complete = true;
    }
}
