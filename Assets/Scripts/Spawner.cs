using System.Collections.Generic;
using System.Collections;
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
    [SerializeField] float waitTime = 0.3f;
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

    // Enable Touch
    public void OnStartPlacing() 
    {
        Debug.Log("Touch Enabled");
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        StartCoroutine(AddTouchDelay());
    }

    IEnumerator AddTouchDelay()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }
    
    // Disable Touch
    public void OnStopPlacing() 
    {
        Debug.Log("Touch Disabled");
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;    
    }

    //Placement Logic
    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                GameObject obj = Instantiate(objectToSpawn[objectID], pose.position, objectToSpawn[objectID].transform.rotation);
                OnStopPlacing();
            }
        }
    }

    //Menu Logic
    public void SpawnObject(int id)
    {
        if (id > objectToSpawn.Count) { Debug.Log(id); }
        else
        {
            objectID = id;
            panelOpener.PlayAnimation();
        }      
    }
}
