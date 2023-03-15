using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class PlaneAreaBehavior : MonoBehaviour
{
    public TextMeshPro areaText;
    public ARPlane arPlane;
    
    private void Update()
    {
        areaText.transform.rotation = 
            Quaternion.LookRotation(areaText.transform.position - 
            Camera.main.transform.position);
    }

    private void ArPlane_BoundaryChanged(ARPlaneBoundaryChangedEventArgs obj)
    {
        areaText.text = CalculatePlaneArea(arPlane).ToString();
    }

    private float CalculatePlaneArea(ARPlane plane)  
    {        
        return plane.size.x * plane.size.y;
    }

    public void ToggleAreaView()
    {
        if(areaText.enabled)
            areaText.enabled = false;
        else
            areaText.enabled = true;
    }
}
