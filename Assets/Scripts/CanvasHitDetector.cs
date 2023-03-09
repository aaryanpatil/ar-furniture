using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CanvasHitDetector : MonoBehaviour 
{
    private GraphicRaycaster _graphicRaycaster;
    PointerEventData pointerEventData;
    
    private void Start()
    {
        // This instance is needed to compare between UI interactions and
        // game interactions with the mouse.
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    public bool IsPointerOverUI()
    {
        // Obtain the current touch position.
        var touchPosition = Input.GetTouch(0).position;

        // Create a pointer event data structure with the current mouse position.
        
        pointerEventData.position = touchPosition;
        // Use the GraphicRaycaster instance to determine how many UI items
        // the pointer event hits.  If this value is greater-than zero, skip
        // further processing.
        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(pointerEventData, results);
        return results.Count > 0;
    }
}