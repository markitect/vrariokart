using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointTracker : MonoBehaviour
{
    public List<GameObject> Checkpoints = new List<GameObject>();
    static public int currentCheckpoint = 0;
    static public int currentLap = 1;
    public int lapsToWin = 1;

    public Dictionary<int, float> timeSplits = new Dictionary<int, float>();
    public List<float> LapTimes = new List<float>();

    public float startTime;

    public GameObject LapInfoUI;

	// Use this for initialization
	void Start ()
    {
        Checkpoints[0].GetComponent<CheckPoint>().UpdateCheckpoint(); //activate the first checkpoint
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update ()
    {
        var sb = new StringBuilder();
        sb.AppendLine(LapInfo());
        sb.AppendLine(SplitInfo());

        LapInfoUI.GetComponent<Text>().text = sb.ToString();
    }

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
            var endTime = Time.time;
            var currentCheckpointTime = endTime - startTime;
            if(timeSplits.ContainsKey(currentCheckpoint))
            {
                timeSplits[currentCheckpoint] = currentCheckpointTime;
            }
            else
            {
                timeSplits.Add(currentCheckpoint, currentCheckpointTime);
            }

            Debug.LogFormat("Checkpoint {0}: Time: {1}", currentCheckpoint, currentCheckpointTime);
            startTime = Time.time;

            if (currentLap == lapsToWin && CompareCheckpoint(obj))
            {
                obj.GetComponent<CheckPoint>().m_isFinishLine = true;
                obj.GetComponent<CheckPoint>().UpdateCheckpoint();
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

    string LapInfo()
    {
        return string.Format("Lap: {0}", currentLap);
    }

    string SplitInfo()
    {
        var sb = new StringBuilder();

        foreach (var splitInfo in this.timeSplits)
        {
            if (splitInfo.Value != 0)
            {
                sb.AppendLine(string.Format("Checkpoint {0}: {1}s", splitInfo.Key, splitInfo.Value));
            }
        }

        return sb.ToString();
    }
}
