using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine.VR;

public class PlayerTank : MonoBehaviour
{
	// Mobile input data
	private bool m_Dragging;
	private Vector2 m_TouchPoint, m_StartDrag, m_EndDrag;

	private float thrust;
	private float turn;
	public Rigidbody rigidBody;

	private void Start()
	{
		rigidBody.freezeRotation = true;
	}

	private void Update()
    {
        turn = Input.GetAxis("Horizontal") / 10.0f;
        thrust = Input.GetAxis("Vertical");
        rigidBody.transform.RotateAround(rigidBody.transform.up, turn);
		rigidBody.transform.Translate(Vector3.forward * thrust);
	}
}
