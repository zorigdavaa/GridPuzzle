using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsive Camera Scaler
/// </summary>
public class CameraAspectRatioScaler : MonoBehaviour {

    /// <summary>
    /// Reference Resolution like 1920x1080
    /// </summary>
    public Vector2 ReferenceResolution;

    /// <summary>
    /// Zoom factor to fit different aspect ratios
    /// </summary>
    public Vector3 ZoomFactor = Vector3.one;

    /// <summary>
    /// Design time position
    /// </summary>
    [HideInInspector]
    public Vector3 OriginPosition;

    /// <summary>
    /// Start
    /// </summary>
    void Start () {
        OriginPosition = transform.position;
    }
	
	/// <summary>
    /// Update per Frame
    /// </summary>
	void Update () {

        if (ReferenceResolution.y == 0 || ReferenceResolution.x == 0)
            return;

        var refRatio = ReferenceResolution.x / ReferenceResolution.y;
        var ratio = (float)Screen.width / (float)Screen.height;

        transform.position = OriginPosition + transform.forward * (1f - refRatio / ratio) * ZoomFactor.z
                                            + transform.right   * (1f - refRatio / ratio) * ZoomFactor.x
                                            + transform.up      * (1f - refRatio / ratio) * ZoomFactor.y;


    }
}


// public class ResponsiveCamera : MonoBehaviour {

//     public Camera PortraitCamera;
//     public Camera LandscapeCamera;

//     // Use this for initialization
//     void Start () {
		
// 	}
	
// 	// Update is called once per frame
// 	void Update () {

//         PortraitCamera.enabled = Screen.width <= Screen.height;
//         PortraitCamera.GetComponent<AudioListener>().enabled = PortraitCamera.enabled;
//         LandscapeCamera.enabled = Screen.width > Screen.height;
//         LandscapeCamera.GetComponent<AudioListener>().enabled = LandscapeCamera.enabled;

//     }
// }