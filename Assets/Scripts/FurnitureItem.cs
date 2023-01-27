using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Furniture", menuName = "Furniture")]
public class FurnitureItem : ScriptableObject
{
    public string furnitureName;
    public Sprite furnitureImage;
    public GameObject furnitureModel;
    public int furnitureId;
}
