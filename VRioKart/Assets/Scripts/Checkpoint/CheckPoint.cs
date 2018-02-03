using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool m_active = false;
    public bool m_isFinishLine = false;
    Renderer m_rend;
    public AudioSource m_checkpointSound;
    public Renderer m_finishLine;
	// Use this for initialization
	void Awake ()
    {
        m_rend = GetComponent<Renderer>();
        m_finishLine.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_active && m_rend.enabled == false)
        {
            m_rend.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.GetComponentInParent<CheckpointTracker>())
            {
                other.transform.GetComponentInParent<CheckpointTracker > ().CheckpointFunction(gameObject);
            }
        }
    }
    public void UpdateCheckpoint()
    {
        if (m_isFinishLine) //if player is on last checkpoint of last lap then
        {
            Debug.Log("finishline set true");
            SetupFinishline();
        }

        if (m_active)//turn off if active
        {
            if (m_checkpointSound)
            {
                m_checkpointSound.Play(); //fire off audio (Queue dramatic voice saying "Checkpoint!" or something.)
            }
            m_active = false;
            m_rend.enabled = false; //turn off collider mesh renderer
            //do particle effects maybe
            //update minimap maybe
        }
        else //turn on if inactive
        {
            m_active = true;
            m_rend.enabled = true; //turn on collider mesh renderer
            //update minimap maybe
        }


    }

    private void SetupFinishline()
    {
        Debug.Log("setup finish line");
        m_finishLine.enabled = true;
    }
}
