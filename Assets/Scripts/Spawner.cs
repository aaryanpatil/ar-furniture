using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;

// Changed
[RequireComponent(typeof(ARRaycastManager), 
    typeof(ARPlaneManager))]

public class Spawner : MonoBehaviour
{
    public List<GameObject> objectToSpawn;
    [SerializeField] float waitTime = 0.3f;
    [SerializeField] Vector3 directionOfRotation;
    [SerializeField] float speedOfRotation;
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public Camera aRCamera;
    public Text stateText;

    private int objectID;
    private int currentObjectID;

    PanelOpener panelOpener;
    CanvasHitDetector canvasHitDetector;

    private GameObject spawnedObj;
    private GameObject newObjectToSpawn;
    private List<GameObject> listOfActiveObjects;
    private Pose pose;
    private bool doRotateObject;
    private bool isPointerOverGameObject = false;
    int indexOfActiveObject = -1;

    float startPos;
    float direction;

    bool hasPlane;
    float startPosAbove;
    float startPosBelow;

    UnityEngine.Touch touchAbove;
    UnityEngine.Touch touchBelow;

    private void Awake() 
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        panelOpener = FindObjectOfType<PanelOpener>();
        canvasHitDetector = FindObjectOfType<CanvasHitDetector>();
        objectID = 0;
        doRotateObject = false;
    }

    /*******************************/
    //Enable Touch
    public void OnStartPlacement() 
    {
        Debug.Log("Touch Enabled");
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        indexOfActiveObject = 0;
        StartCoroutine(AddTouchDelay());
    }

    IEnumerator AddTouchDelay()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        // EnhancedTouch.Touch.onFingerDown += FingerDown;
    }
    
    // Disable Touch
    public void OnStopPlacing() 
    {
        Debug.Log("Touch Disabled");
        EnhancedTouch.TouchSimulation.Disable();
        // EnhancedTouch.Touch.onFingerDown -= FingerDown; 
        EnhancedTouch.EnhancedTouchSupport.Disable();   
    }

    void PlaceObject()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
            if (aRRaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                if (spawnedObj == null)
                {
                    pose = hits[0].pose;
                    spawnedObj = Instantiate(objectToSpawn[objectID], pose.position, pose.rotation);
                    listOfActiveObjects.Add(spawnedObj);
                }
                // else
                // {
                //     pose = hits[0].pose;
                //     newObjectToSpawn =  Instantiate(objectToSpawn[objectID], pose.position, pose.rotation);
                //     spawnedObj = newObjectToSpawn;
                //     listOfActiveObjects.Add(spawnedObj); 
                // }
            }

        }
    }
    //Placement Logic
    // private void FingerDown(EnhancedTouch.Finger finger)
    // {
    //     if (isPointerOverGameObject) return;
    //     if (finger.index != 0 || doRotateObject) 
    //     {
    //         return;
    //     }
    //     if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
    //     {
    //         foreach (ARRaycastHit hit in hits)
    //         {
    //             if (spawnedObj == null)
    //             {
    //                 pose = hit.pose;
    //                 spawnedObj = Instantiate(objectToSpawn[objectID], pose.position, pose.rotation);
    //                 listOfActiveObjects.Add(spawnedObj);
    //             }
    //             else
    //             {
    //                 //alreadycommented
    //                 // pose = hit.pose;
                
    //                 // spawnedObj.transform.position = pose.position;
    //                 // listOfActiveObjects[indexOfActiveObject + 1].transform.position = pose.position;
    //             }  
    //         }
    //     }
    // }



    void Update()
    {
        isPointerOverGameObject = canvasHitDetector.IsPointerOverUI();
        // if (isPointerOverGameObject)
        //     stateText.text = "UI";
        // else
        //     stateText.text = "Not UI";
        PlaceObject();
        EnhancedTouch.Touch.onFingerMove += MoveObject;
        // if (!doRotateObject) { return; }
        RotateObject();
    }

    void MoveObject(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0 || doRotateObject) 
        {
            return;
        }
        if (Input.touchCount > 1) return;
        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (ARRaycastHit hit in hits)
            {
                if (spawnedObj == null)
                {
                    return;
                }
                else
                {
                    pose = hit.pose;
                
                    spawnedObj.transform.position = pose.position;
                    // listOfActiveObjects[indexOfActiveObject + 1].transform.position = pose.position;
                }  
            }
        }
    }

    public void StartRotation()
    {
        doRotateObject = true;
    }

    public void StopRotation()
    {
        doRotateObject = false;
    }

    void RotateObject()
    {
        if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)    
        {
            UnityEngine.Touch currentTouch = Input.GetTouch(1);

            switch (currentTouch.phase)
            {
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = currentTouch.position.x;
                    break;
                case TouchPhase.Moved:
                    direction = Mathf.Sign(currentTouch.position.x - startPos);
                    break;
            }
        }
        spawnedObj.transform.Rotate(direction * directionOfRotation * speedOfRotation * Time.deltaTime);
        
        // if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Moved ||Input.GetTouch(1).phase == TouchPhase.Moved)
        // {
        //     if (Input.GetTouch(0).position.y > Input.GetTouch(1).position.y)
        //     {
        //         touchAbove = Input.GetTouch(0);
        //         touchBelow = Input.GetTouch(1);
        //     }
        //     else if (Input.GetTouch(0).position.y < Input.GetTouch(1).position.y)
        //     {
        //         touchAbove = Input.GetTouch(1);
        //         touchBelow = Input.GetTouch(0);
        //     }
        //     startPosAbove = touchAbove.position.x;
        //     startPosBelow = touchBelow.position.x;
        // }
        
        //     if (touchAbove.position.x > startPosAbove)
        //     {
        //         direction = 1;
        //     }
        //     else if (touchAbove.position.x < startPosAbove)
        //     {
        //         direction = -1;
        //     }
        //     else if (touchBelow.position.x < startPosBelow)
        //     {
        //         direction = 1;
        //     }
        //     else if (touchBelow.position.x > startPosBelow)
        //     {
        //         direction = -1;
        //     }
    
        // spawnedObj.transform.Rotate(direction * directionOfRotation * speedOfRotation * Time.deltaTime);
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
    
    /*******************************/
    // private void Update() 
    // {
    //     // Ray ray = aRCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));      
    //     // Debug.DrawRay (ray.origin, ray.direction * 10, Color.blue);
    //     // Check if there is a touch
    //     if (Input.touchCount > 0)
    //     {
    //         if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
    //         {
    //             GameObject obj = null;
    //             // Check if finger is over a UI element
    //             if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    //             {
    //                 return;
    //             }
    //             else
    //             {
    //                 if (aRRaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
    //                 {
    //                     // foreach (ARRaycastHit hit in hits)
    //                     // {
                            
    //                         if (Input.GetTouch(0).phase == TouchPhase.Began)
    //                         {
    //                             Pose pose = hits[0].pose;
    //                             obj = Instantiate(objectToSpawn[objectID], pose.position, pose.rotation);
    //                         }
    //                         else if (Input.GetTouch(0).phase == TouchPhase.Moved)
    //                         {
    //                             obj.transform.position = hits[0].pose.position;
    //                         }
    //                     // }
    //                 }
    //             }
    //         }
    //     }
            
    // }  

    // public void MoveObject()
    // {
        
    // }
    // public void SpawnObject(int id)
    // {
    //     if (id > objectToSpawn.Count) { Debug.Log(id); }
    //     else
    //     {
    //         objectID = id;
    //         panelOpener.PlayAnimation();
    //     }      
    // }

    // private void OnEnable() 
    // {
    //     Debug.Log("Touch Enabled");
    //     EnhancedTouch.TouchSimulation.Enable();
    //     EnhancedTouch.EnhancedTouchSupport.Enable(); 
    // }

    // private void OnDisable() 
    // {
    //     Debug.Log("Touch Disabled");
    //     EnhancedTouch.TouchSimulation.Disable();
    //     EnhancedTouch.EnhancedTouchSupport.Disable(); 
    // }

    // public void SpawnObject(int id)
    // {
    //     if (id > objectToSpawn.Count) { Debug.Log(id); }
    //     else
    //     {
    //         currentObjectID = id;
    //         Debug.Log(currentObjectID);
    //         panelOpener.PlayAnimation();
    //     }      
    // }
}
