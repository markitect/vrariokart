using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    public List<GameObject> Checkpoints = new List<GameObject>();
    static public int currentCheckpoint = 0;
    static public int currentLap = 1;
    public int lapsToWin = 1;

	// Use this for initialization
	void Start ()
    {
        Checkpoints[0].GetComponent<CheckPoint>().UpdateCheckpoint(); //activate the first checkpoint
        Debug.Log("Lap " + currentLap);
    }
    // Update is called once per frame
    void Update (){}

    public void CheckpointFunction(GameObject obj)
    {
        if (CompareCheckpoint(obj))
        {
            if (currentCheckpoint != Checkpoints.Count -1)//if current checkpoint is not the last checkpoint
            {
                if (currentLap == lapsToWin && currentCheckpoint +1 == Checkpoints.Count - 1)//if coming to last checkpoint of last lap
                {
                    Checkpoints[currentCheckpoint + 1].GetComponent<CheckPoint>().m_isFinishLine = true;//turn on the finish line.
                }
                Checkpoints[currentCheckpoint + 1].GetComponent<CheckPoint>().UpdateCheckpoint();       //Update next Checkpoint
                Checkpoints[currentCheckpoint].GetComponent<CheckPoint>().UpdateCheckpoint();           //Update current Checkpoint
                currentCheckpoint++;
                
            }
            else
            {
                Checkpoints[0].GetComponent<CheckPoint>().UpdateCheckpoint();                           //Update first Checkpoint
                Checkpoints[currentCheckpoint].GetComponent<CheckPoint>().UpdateCheckpoint();           //Update current Checkpoint
                currentCheckpoint = 0;
                UpdateLaps();
            }
        }
    }

    bool CompareCheckpoint(GameObject obj)
    {
        return (obj == Checkpoints[currentCheckpoint]); //compare collided object against current checkpoint object
    }

    void UpdateLaps()//update the current lap
    {
        if(currentLap < lapsToWin)
        {
            currentLap++;
            Debug.Log("Lap " + currentLap);
        }
        else
        {
            FinishRace();
        }
    }

    private void FinishRace()
    {
        //for now since there are no enemies just let them win.  "Big Rigs, Your Winner!!!"
        WinRace();
    }

    void WinRace()
    {
        Debug.Log("Won the race!");
        //finish the race logic and do all the cool stuff that comes with winning a race.
    }

    void LoseRace()
    {
        //To be implemented after there are other people in the race.
    }
}
