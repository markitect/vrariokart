using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool m_active = false;
    public bool m_isFinishLine = false;
    Renderer rend;
    public AudioSource m_checkpointSound;
	// Use this for initialization
	void Awake ()
    {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_active && rend.enabled == false)
        {
            rend.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.parent.GetComponent<CheckpointTracker>())
            {
                other.gameObject.transform.parent.GetComponent<CheckpointTracker>().CheckpointFunction(gameObject);
            }
        }
    }
    public void UpdateCheckpoint()
    {
        if (m_isFinishLine) //if player is on last checkpoint of last lap then
        {
            SetupFinishline();
        }
        if (m_active)//turn off if active
        {
            if (m_checkpointSound)
            {
                m_checkpointSound.Play(); //fire off audio (Queue dramatic voice saying "Checkpoint!" or something.)
            }
            m_active = false;
            rend.enabled = false; //turn off collider mesh renderer
            //do particle effects maybe
            //update minimap maybe
        }
        else //turn on if inactive
        {
            m_active = true;
            rend.enabled = true; //turn on collider mesh renderer
            //update minimap maybe
        }


    }

    private void SetupFinishline()
    {
        throw new NotImplementedException();
    }
}
