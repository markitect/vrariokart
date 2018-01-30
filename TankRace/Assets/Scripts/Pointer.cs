using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

[RequireComponent(typeof(LineRenderer))]
public class Pointer : MonoBehaviour {

    public XRNode TrackedNode;
    public float pointerDistance = 20f;

    LineRenderer pointerLine;

    StartMenuButton currentButton;
    
	// Use this for initialization
	void Start () {
        this.transform.position = Camera.main.transform.position;
        pointerLine = GetComponent<LineRenderer>();
        pointerLine.startColor = Color.red;
        pointerLine.startWidth = .01f;
        pointerLine.endColor = Color.clear;
        pointerLine.endWidth = .01f;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = InputTracking.GetLocalPosition(TrackedNode);
        this.transform.localRotation = InputTracking.GetLocalRotation(TrackedNode);

        pointerLine.SetPositions(new Vector3[] { this.transform.position, this.transform.position + (this.transform.forward * pointerDistance) });

        RaycastHit hitInfo;

        if(Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hitInfo))
        {
            var button = hitInfo.collider.GetComponent<StartMenuButton>();
            if(button != null)
            {
                button.IsHighlighted = true;
                this.currentButton = button;
            }
        }
        else
        {
            if (this.currentButton != null)
            {
                this.currentButton.IsHighlighted = false;
                this.currentButton = null;
            }
        }
	}
}
