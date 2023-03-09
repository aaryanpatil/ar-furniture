using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneVisualizerEnabler : MonoBehaviour
{
    private ARPlaneManager planeManager;
    [SerializeField] private TextMeshProUGUI toggleButtonText;
    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        toggleButtonText.text = "Disable";
    }
    public void TogglePlaneDetection()
    {
        planeManager.enabled = !planeManager.enabled;
        string toggleButtonMessage = "";

        if(planeManager.enabled)
        {
            toggleButtonMessage = "Disable Plane Detection";
            SetAllPlanesActive(true);
        }
        else
        {
            toggleButtonMessage = "Enable Plane Detection";
            SetAllPlanesActive(false);
        }

        toggleButtonText.text = toggleButtonMessage;
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
