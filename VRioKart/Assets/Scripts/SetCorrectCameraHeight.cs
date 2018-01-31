using UnityEngine;
using UnityEngine.XR;

public enum TrackingSpace
{
    Seated,
    RoomScale
}

public class SetCorrectCameraHeight : MonoBehaviour {

    [Header("Camera Settings")]
    [Tooltip("Decide if experience is Room Scale or Seated. Note this option does nothing for mobile VR experiences, these experience will default to Seated")]
    public TrackingSpace trackingSpace = TrackingSpace.Seated;

    [Tooltip("Camera Height - overwritten by device settings when using Room Scale ")]
    public float seatedCameraYOffset = 1.36144f;

    [Tooltip("GameObject to move to desired height")]
    public GameObject cameraContainer;

    void Start ()
    {
        SetCameraHeight();
    }

    void SetCameraHeight()
    {
        if (trackingSpace == TrackingSpace.Seated)
        {
            XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        }
        else if (trackingSpace == TrackingSpace.RoomScale)
        {
            if (XRDevice.model.Contains("Rift") || XRDevice.model.Contains("Vive") || XRSettings.loadedDeviceName == "WindowsMR")
            {
				XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
                seatedCameraYOffset = 0;
            }
        }

        InputTracking.Recenter();

        //Move camera to correct height
        if (cameraContainer)
        {
            cameraContainer.transform.position = new Vector3(cameraContainer.transform.position.x, seatedCameraYOffset, cameraContainer.transform.position.z);
        }
    }

  
}

