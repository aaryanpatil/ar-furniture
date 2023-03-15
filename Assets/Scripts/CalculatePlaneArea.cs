using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CalculatePlaneArea : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI areaText;
    [SerializeField] ARRaycastManager aRRaycastManager;
    ARPlaneManager aRPlaneManager;
    ARPlane lowestPlane = null;
    
    bool updateCalculation = false; 
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    float area;

    private void Awake() 
    {
        aRPlaneManager = FindObjectOfType<ARPlaneManager>();    
        areaText.text = "0 sq. ft";
    }

    public void CalculateArea()
    {    
        updateCalculation = !updateCalculation;
        area = 0f;
        areaText.gameObject.SetActive(updateCalculation);
    }

    
    private void Update()
    {
        if (updateCalculation)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    area = 0;
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) { return; }
                    if (aRRaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
                    {
                        foreach (var plane in aRPlaneManager.trackables)
                        {
                            if (lowestPlane == null)
                            {
                                lowestPlane = plane;
                            }
                            else 
                            {
                                if (plane.transform.position.z < lowestPlane.transform.position.z)
                                {
                                    lowestPlane = plane;
                                }
                            }
                            area += (lowestPlane.size.x * 3.28084f * lowestPlane.size.y * 3.28084f);
                        }
                    }
                }
            }
            areaText.text = area.ToString("F2") + "sq. ft";
        }
    }
}
