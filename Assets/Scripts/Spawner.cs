using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

// Changed
[RequireComponent(typeof(ARRaycastManager), 
    typeof(ARPlaneManager))]

public class Spawner : MonoBehaviour
{
    public List<GameObject> objectToSpawn;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private int objectID;

    PanelOpener panelOpener;

    private void Awake() 
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        panelOpener = FindObjectOfType<PanelOpener>();
        objectID = 0;
    }

    private void OnEnable() 
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() 
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;    
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(objectToSpawn[objectID], pose.position, objectToSpawn[objectID].transform.rotation);
            }
        }
    }

    public void SpawnObject(int id)
    {
        if (id > objectToSpawn.Count) { Debug.Log(id); }
        else
        {
            // Debug.Log(id);
            objectID = id;
            Debug.Log(objectID);
            panelOpener.PlayAnimation();
            // Instantiate(objectToSpawn[objectID], transform.position, objectToSpawn[objectID].transform.rotation);
            // remove
        }      
    }
}
